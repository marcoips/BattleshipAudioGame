using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Model
{
    public class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Cell[,] Cells { get; set; }
        public List<Ship> Ships { get; set; }

        public Board (int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new Cell[rows, columns];
            Ships = new List<Ship>();

            // Inicia todas as celulas do tabuleiro
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Cells[r, c] = new Cell(r, c);
                }
            }
        }
        // Tenta posicionar um navio no tabuleiro (vertical ou horizontal) a partir de uma posição inicial.
        // Retorna true se a colocação foi bem-sucedida, caso contrário false.

        //PlaceShip faz a lógica de colocar um navio vertical ou horizontalmente.
        public bool PlaceShip(Ship ship, Position start, bool isVertical)
        {
            // Lógica para colocar o navio no tabuleiro
            // Verifica se há espaço, se não sobrepõe outro navio, etc.
            // Se puder colocar, marca as células com HasShip = true e adiciona a lista de positions do navio
            // Retorna true/false caso seja possível ou não
            
            if(isVertical)
            {
                //verufica os limites
                if ((start.Row + ship.Size) > Rows)
                {
                    return false;
                }

                //Verifica se nenhuma célula está ocupada
                for ( int i= 0; i < ship.Size; i++)
                {
                    if(Cells[start.Row + i, start.Column].HasShip)
                    {
                        return false;
                    }
                }

                // se tudo certo
                for(int i = 0; i < ship.Size; i++)
                {
                    Cells[start.Row + i, start.Column].HasShip = true;
                    
                    ship.Positions.Add(new Position(start.Row + i, start.Column));
                }


            }
            else
            {
                //verifica na horizpntal 
                if (start.Column + ship.Size > Columns)
                {
                    return false;
                }

                for (int i = 0; i < ship.Size; i++)
                {
                    if (Cells[start.Row, start.Column + i].HasShip)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < ship.Size; i++)
                {
                    Cells[start.Row, start.Column + i].HasShip = true;
                    ship.Positions.Add(new Position(start.Row, start.Column + i));
                }
            }
            Ships.Add(ship);
            return true;
        }

        // Executa um ataque em uma célula, marcando como atingida.
        // Retorna true se o ataque acertou um navio, false se foi água.

        //Attack marca a célula como atingida e verifica se havia navio.
        public bool Attack(Position position)
        {
            // Marca célula como IsHit
            // Verifica se há navio naquela posição
            // Se houver, registra se o navio foi afundado (IsSunk)
            // Retorna true se acerto (hit), false se erro (miss)

            // Marca a celula como atingida
            Cells[position.Row, position.Column].IsHit = true;

            if(Cells[position.Row, position.Column].HasShip)
            {
                // Se a célula tem um navio, procura qual
                foreach (var ship in Ships)
                {
                    if (ship.Positions.Contains(position))
                    {
                        // verifica se afundou
                        bool allHits = true;
                        foreach (var pos in ship.Positions)
                        {
                            if (!Cells[pos.Row, pos.Column].IsHit)
                            {
                                allHits = false;
                                break;
                            }
                        }

                        if (allHits)
                        {
                            ship.IsSunk = true;
                           
                        }
                        return true; // Acertou no navio

                    }
                }
            }
            return false; // Errou (água)

        }


    }
}
