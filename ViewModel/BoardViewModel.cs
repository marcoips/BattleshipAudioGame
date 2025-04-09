using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipAudioGame.Model;
using System.Collections.ObjectModel;

namespace BattleshipAudioGame.ViewModel
{
    //sera o ViewModel responsável por todo o tabuleiro
    //ViewModel para representar o tabuleiro (Board).
    public class BoardViewModel
    {
        private Board _board;

        //Cells é um ObservableCollection para que a interface possa ser atualizada automaticamente se a lista mudar 
        public ObservableCollection<CellViewModel> Cells { get; set; }

        public BoardViewModel(int rows, int columns)
        {
            // cria uma instancia do modelo board
            _board = new Board(rows, columns);

            // Cria uma coleção observável para armazenar as células
            Cells = new ObservableCollection<CellViewModel>();
            // Preenche a coleção de células com as células do tabuleiro
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Cells.Add(new CellViewModel(_board.Cells[r, c]));
                }
            }
        }


        //Metodo para colocar navios automaticamente ou manualmente.
        public void PlaceShipDefoult()
        {
            //criar um navio tamnho 3
            Ship ship = new Ship("navio", 3);

            //colocar o navio na posicao 0,0, vertical
            _board.PlaceShip(ship, new Position(0, 0), true);

            //Criar o resto

        }


        //Metedo para exucutar um ataque em determinada célula do tabuleiro

        public bool AttackCell(CellViewModel cellM)
        {
            var pos = new Position(cellM.Row, cellM.Column);
            bool hit = _board.Attack(pos);

            //forçar o cellviewmodel a autalizar a propriedade IsHit
            cellM.IsHit = true;

            return hit;

        }
    }
}
