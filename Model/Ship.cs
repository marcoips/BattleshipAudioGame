using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Model
{
    public class Ship
    {

        public string Name { get; set; }
        public int Size { get; set; }
        public bool IsSunk { get; set; }
        //cada Positions indica uma coordenada, linha e coluna
        public List<Position> Positions { get; set; } 

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            IsSunk = false;
            Positions = new List<Position>();
        }




    }
}
