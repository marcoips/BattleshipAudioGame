using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipAudioGame.Model;

namespace BattleshipAudioGame.ViewModel
{
    // ViewModel para cada célula. Expõe propriedades que a interface vai utilizar.
    public class CellViewModel : BaseViewModel
    {
        private Cell _cell;

        public int Row => _cell.Position.Row;
        public int Column => _cell.Position.Column;

        public bool IsHit
        {
            get => _cell.IsHit;
            set
            {
                if (_cell.IsHit != value)
                {
                    _cell.IsHit = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public bool HasShip => _cell.HasShip; // Propriedade para verificar se a célula tem um navio

        // Propriedade que descreve o estado (útil para a tela e fala).

        public string Status
        {
            get
            {
                if (!IsHit)
                {
                    return "Não atingido!";
                }
                else
                {  // Se o navio foi atingido, verifica se a célula tem um navio
                    return HasShip ? "Navio Atingido" : "Água";
                }
                
            }
        }
        public CellViewModel(Cell cell)
        {
            _cell = cell;
        }

    }

}






