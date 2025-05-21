using BattleshipAudioGame.Model;
using BattleshipAudioGame.Serviços;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BattleshipAudioGame.ViewModel
{
    public class PlacementViewModel : BaseViewModel
    {
        private readonly Action<string> _navigate;
        private readonly Random _random = new();
        public BoardViewModel CpuBoard { get; }
        public BoardViewModel PlayerBoard { get; }
        public ICommand ContinueCommand { get; }
        private int _navioIndex = 0;
        private bool _horizontal = true;
        private List<Navio> __naviosAColocar;



        public ICommand AlterarOrientacaoCommand { get; }
        public ICommand ColocarNavioCommand { get; }
        public string OrientacaoTexto => _horizontal ? "Horizontal" : "Vertical";
        public string NomeNavioAtual => __naviosAColocar[_navioIndex].nome_navio;
        public bool PodeContinuar { get; private set; } = false;

        //Rechociemnto de voz
        private readonly VoiceRecognitionService _voice;
        private readonly bool _audioOn;
        private enum Fase { Orientacao, Coordenadas }
        private Fase _fase;

        public PlacementViewModel(Action<string> navigate, VoiceRecognitionService voice, bool audioOn = true)
        {
            _navigate = navigate;
            //ContinueCommand = new RelayCommand(_ => _navigate("Game"));// placeholder

            _voice = voice;
            _audioOn = audioOn;

            


            //1. criar tabuleiro
            PlayerBoard = new BoardViewModel { BoardTitle = "Tabuleiro do Jogador" };
            CpuBoard = new BoardViewModel { BoardTitle = "Tabuleiro do CPU" };

            /* 1️⃣  cria realmente as listas de navios */
            PrepararFrotaJogador();
            //CriarFrotaJogador();
            GerarFrotaCPU();
            CpuBoard.PreencherNavios(CpuBoard.Navios, Brushes.Red);

            /* 3. comandos ------------------ */
            AlterarOrientacaoCommand = new RelayCommand(_ => AlternarOrientacao());
            ColocarNavioCommand = new RelayCommand(
                param => ColocarNavio(param as GridCell), p => !PodeContinuar);
            ContinueCommand = new RelayCommand(
                _ => _navigate("Game"), _ => PodeContinuar); // só ativa no fim
           


            if (_audioOn)
            {
                IniciarDialogo();
            }

        }

        private void PrepararFrotaJogador()
        {
            __naviosAColocar = new List<Navio>
            {
                new("Carrier",     5,false,new()),
                new("Battleship",  4,false,new()),
                new("Cruiser",     3,false,new()),
                new("Destroyer",   2,false,new()),
                new("Submarine",   3,false,new())
            };
            //ainda nao preenche nada -será feito navio a navio
            PlayerBoard.Navios = new List<Navio>();
        }

        /*private void CriarFrotaJogador()
        {
            PlayerBoard.Navios = new List<Navio>
            {
                new("Carrier",     5,false,new(){ "A1","A2","A3","A4","A5"}),
                new("Battleship",  4,false,new(){ "B1","B2","B3","B4"}),
                new("Cruiser",     3,false,new(){ "C1","C2","C3"}),
                new("Destroyer",   2,false,new(){ "D1","D2"}),
                new("Submarine",   3,false,new(){ "E1","E2","E3"})
            };
        }*/

        public void GerarFrotaCPU()
        {
            var ocupadas = new List<string>();
            CpuBoard.Navios = new List<Navio>
            {
                CriarAleatorio("Carrier",5,  ocupadas),
                CriarAleatorio("Battleship",4,ocupadas),
                CriarAleatorio("Cruiser",3,  ocupadas),
                CriarAleatorio("Destroyer",2,ocupadas),
                CriarAleatorio("Submarine",3,ocupadas),
            };
        }

        private void AlternarOrientacao()
        {
            _horizontal = !_horizontal;
            OnPropertyChanged(nameof(OrientacaoTexto));
        }


        private void ColocarNavio(GridCell? cell)
        {
            Debug.WriteLine($"CLICK {cell?.Row},{cell?.Column}");
            if (PodeContinuar || cell is null) return; // já terminado ou clique inválido

            var navio = __naviosAColocar[_navioIndex];
            var posicoes = GerarPosicoes(cell.Row, cell.Column, navio.tamanho_navio, _horizontal);

            if (!ValidarPosicoes(posicoes))
            {
                return;
            }
            Debug.WriteLine("PASSOU validação");
            navio.localizacao = posicoes;
            PlayerBoard.Navios.Add(navio);
            PlayerBoard.PreencherNavios(new[] { navio }, Brushes.Green);
            Debug.WriteLine("PINTADO");
            _navioIndex++;

            if (_navioIndex >= __naviosAColocar.Count)
            {
                // frota completa — ativar botão Continuar
                PodeContinuar = true;
                OnPropertyChanged(nameof(PodeContinuar));

                CommandManager.InvalidateRequerySuggested(); //   força CanExecute a refazer

            }
            else
            {
                OnPropertyChanged(nameof(NomeNavioAtual));
            }

        }

        private List<string> GerarPosicoes(int row, int col, int tamanho, bool horizontal)
        {
            var pos = new List<string>();
            for (int i = 0; i < tamanho; i++)
            {
                int r = row + (horizontal ? 0 : i);
                int c = col + (horizontal ? i : 0);
                if (r >= 10 || c >= 10) return new(); // fora do tabuleiro
                pos.Add($"{(char)('A' + r)}{c + 1}");
            }
            return pos;
        }

        private bool ValidarPosicoes(List<string> posicoes)
        {
            if (posicoes.Count == 0) return false;
            return !PlayerBoard.Navios.Any(n => n.localizacao.Any(posicoes.Contains));
        }


        private Navio CriarAleatorio(string nome, int tamanho, List<string> ocupadas)
        {
            var loc = GeneratePositionsCPU(tamanho, ocupadas);
            ocupadas.AddRange(loc);
            return new Navio(nome, tamanho, false, loc);

        }

        private List<string> GeneratePositionsCPU(int tamanho, List<string> ocupadas)
        {
            var posicoes = new List<string>();
            var ok = false;


            while (!ok)
            {
                posicoes.Clear();

                int orient = _random.Next(0, 2); // 0 = horizontal, 1 = vertical
                int r0 = _random.Next(0, 10);
                int c0 = _random.Next(0, 10);

                for (int i = 0; i < tamanho; i++)
                {
                    if (orient == 0)
                    {
                        if (c0 + tamanho > 10)
                        {
                            posicoes.Clear();
                            break;
                        }
                        posicoes.Add($"{(char)('A' + r0)}{c0 + i + 1}");

                    }
                    else
                    {
                        if (r0 + tamanho > 10)
                        {
                            posicoes.Clear();
                            break;
                        }
                        posicoes.Add($"{(char)('A' + r0 + i)}{c0 + 1}");
                    }
                }

                ok = posicoes.Count == tamanho &&
                     !posicoes.Any(p => ocupadas.Contains(p));

            }

            return posicoes;
        }


        //------------------------------- comandos voz---------------------
        private void IniciarDialogo()
        {
            _fase = Fase.Orientacao;
            _voice.OnCommand += HandleVoice;
            _voice.Speak($"Vamos posicionar o, {NomeNavioAtual}. Diga horizontal ou vertical.");
            _voice.StartRecognition(new[] { "horizontal", "vertical" });
        }

        private void HandleVoice(string txt)
        {
            if (_fase == Fase.Orientacao)
            {
                if (txt.Contains("horizontal") || txt.Contains("vertical"))
                {
                    _horizontal = txt.Contains("horizontal");
                    _voice.Speak($"{OrientacaoTexto}. Now say the coordinate, for example row 3 column 5.");
                    _voice.StartRecognition(FrasesCoordenadas());
                    _fase = Fase.Coordenadas;
                }
            }
            else if (_fase == Fase.Coordenadas)
            {
                var cell = ParseCoordenada(txt); // devolve Gridcell ou nul
                if (cell == null)
                {
                    _voice.Speak("I don't understand. Say row and column, for example row 4 column 7!");
                    return;
                }
                if (!PodeContinuar && ValidarPosicoes(GerarPosicoes(cell.Row, cell.Column, __naviosAColocar[_navioIndex].tamanho_navio, _horizontal)))
                {
                    ColocarNavio(cell);               // reutiliza método visual
                    if (!PodeContinuar)
                    {
                        _fase = Fase.Orientacao;
                        _voice.Speak($"Good. Próximo navio: {NomeNavioAtual}. Diga horizontal ou vertical.");
                        _voice.StartRecognition(new[] { "horizontal", "vertical" });
                    }
                    else
                    {
                        _voice.Speak("Fleet complete! Say continue to begin the battle.");
                        _voice.StartRecognition(new[] { "continue" });
                    }
                }
                else
                {
                    _voice.Speak("Invalid position, try another coordinate.");
                }
            }
            else if (txt.Contains("continue") && PodeContinuar)
            {
                _voice.StopRecognition();
                _navigate("Game");
            }


        }


        private string[] FrasesCoordenadas()
        {
            var frases = new List<string>();
            for (int r = 1; r <= 10; r++)
                for (int c = 1; c <= 10; c++)
                    frases.Add($"row {r} column {c}");
            return frases.ToArray();
        }


        private GridCell? ParseCoordenada(string txt)
        {
            // espera “linha 3 coluna 5”
            var nums = System.Text.RegularExpressions.Regex
                         .Matches(txt, @"\d+")
                         .Select(m => int.Parse(m.Value))
                         .ToArray();
            if (nums.Length != 2) return null;
            int r = nums[0] - 1;   // 0-based
            int c = nums[1] - 1;
            if (r < 0 || r > 9 || c < 0 || c > 9) return null;
            return PlayerBoard.Cells.First(cell => cell.Row == r && cell.Column == c);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            _voice.OnCommand -= HandleVoice;
            _voice.StopRecognition();
        }






    }
}
