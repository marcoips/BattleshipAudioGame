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
        public BoardViewModel PlayerBoard { get; }
        public ICommand ContinueCommand { get; }


        public PlacementViewModel(Action<string> navigate)
        {
            _navigate = navigate;
            ContinueCommand = new RelayCommand(_ => _navigate("Game"));// placeholder

            //1. criar tabuleiro
            PlayerBoard = new BoardViewModel { BoardTitle = "Tabuleiro do Jogador" };

            // COLOCAR NAVIOS
            AdicionarFrotaExemplo();
        }
        private void AdicionarFrotaExemplo()
        {
            var navios = new List<Navio>
            {
                new Navio("Carrier",     5, false, new() { "A1","A2","A3","A4","A5" }),
                new Navio("Battleship",  4, false, new() { "B1","B2","B3","B4" }),
                new Navio("Cruiser",     3, false, new() { "C1","C2","C3" }),
                new Navio("Destroyer",   2, false, new() { "D1","D2" }),
                new Navio("Submarine",   3, false, new() { "E1","E2","E3" })
            };

            PlayerBoard.Navios = navios;

            /* pinta as células (código copiado/adaptado do DisplayGrid) */
            foreach (var ship in navios)
            {
                foreach (var pos in ship.localizacao)
                {
                    int row = pos[0] - 'A';           // 'A'→0
                    int col = int.Parse(pos[1..]) - 1; // "1"→0
                    var cell = PlayerBoard.Cells.First(c => c.Row == row && c.Column == col);
                    cell.Content = ship.nome_navio[0].ToString();
                    cell.Background = Brushes.Green;
                }
            }
        }

    }
}
