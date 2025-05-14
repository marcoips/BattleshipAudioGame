using BattleshipAudioGame.Model;
using System;
using System.Collections.Generic;
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


        public PlacementViewModel(Action<string> navigate)
        {
            _navigate = navigate;
            ContinueCommand = new RelayCommand(_ => _navigate("Game"));// placeholder
            


            //1. criar tabuleiro
            PlayerBoard = new BoardViewModel { BoardTitle = "Tabuleiro do Jogador" };
            CpuBoard = new BoardViewModel { BoardTitle = "Tabuleiro do CPU" };

            /* 1️⃣  cria realmente as listas de navios */
            CriarFrotaJogador();
            GerarFrotaCPU();

            // COLOCAR NAVIOS
            PlayerBoard.PreencherNavios(PlayerBoard.Navios, Brushes.Green);
            CpuBoard.PreencherNavios(CpuBoard.Navios, Brushes.Red);

            
        }

        private void CriarFrotaJogador()
        {
            PlayerBoard.Navios = new List<Navio>
            {
                new("Carrier",     5,false,new(){ "A1","A2","A3","A4","A5"}),
                new("Battleship",  4,false,new(){ "B1","B2","B3","B4"}),
                new("Cruiser",     3,false,new(){ "C1","C2","C3"}),
                new("Destroyer",   2,false,new(){ "D1","D2"}),
                new("Submarine",   3,false,new(){ "E1","E2","E3"})
            };
        }

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
