using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BattleshipAudioGame.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _disposed;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            if (_disposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
            _disposed = true;
        }
        protected virtual void Dispose(bool disposing)
        {
            /*  Se o ViewModel filho tiver handlers, timers, serviços,
                desligue-os aqui.  */
        }

        ~BaseViewModel() => Dispose(false);
    }
    
}
