using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Model
{
    public class Cell
    {

        public Position Position { get; set; }
        public bool IsHit { get; set; } // se já foi atingida
        public bool HasShip { get; set; } //se tem parte do navio nessa celula

        public Cell(int row, int column)
        {
            Position = new Position(row, column);
            IsHit = false;
            HasShip = false;
        }



    }
}
