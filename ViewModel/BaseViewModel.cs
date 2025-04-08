using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BattleshipAudioGame.ViewModel
{
    // Classe base que implementa INotifyPropertyChanged,
    // permite notificar a interface quando uma propriedade é alterada.
    public class BaseViewModel : INotifyPropertyChanged
    {
        // Implementação do INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
