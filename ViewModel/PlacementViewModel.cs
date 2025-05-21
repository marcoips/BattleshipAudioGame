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
        private readonly Random _random = new ();

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


        public PlacementViewModel(Action<string> navigate)
        {
            _navigate = navigate;
            //ContinueCommand = new RelayCommand(_ => _navigate("Game"));// placeholder
            


            //1. criar tabuleiro
            PlayerBoard = new BoardViewModel { BoardTitle = "Tabuleiro do Jogador" };
            CpuBoard = new BoardViewModel { BoardTitle = "Tabuleiro do CPU" };

            /* 1️⃣  cria realmente as listas de navios */
            PrepararFrotaJogador();
            //CriarFrotaJogador();
            GerarFrotaCPU();

            /* 3. comandos ------------------ */
            AlterarOrientacaoCommand = new RelayCommand(_ => AlternarOrientacao());

            ColocarNavioCommand = new RelayCommand(
                param => ColocarNavio(param as GridCell)/*,
                param => !PodeContinuar*/);

            ContinueCommand = new RelayCommand(
                _ => _navigate("Game"), _ => PodeContinuar); // só ativa no fim


            // COLOCAR NAVIOS

            CpuBoard.PreencherNavios(CpuBoard.Navios, Brushes.Red);

            
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
            var posicoes = GerarPosicoes (cell.Row, cell.Column, navio.tamanho_navio,_horizontal);

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

                CommandManager.InvalidateRequerySuggested(); //  ← força CanExecute a refazer
                
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


        private Navio CriarAleatorio (string nome, int tamanho, List<string> ocupadas)
        {
            var loc = GeneratePositionsCPU(tamanho, ocupadas);
            ocupadas.AddRange(loc);
            return new Navio(nome,tamanho, false, loc);

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

                for(int i = 0; i< tamanho; i++)
                {
                    if (orient == 0)
                    {
                        if(c0 + tamanho>10)
                        {
                            posicoes.Clear();
                            break;
                        }
                        posicoes.Add($"{(char)('A' + r0)}{c0 + i + 1}");

                    }
                    else
                    {
                        if(r0 + tamanho > 10)
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

    }


}
