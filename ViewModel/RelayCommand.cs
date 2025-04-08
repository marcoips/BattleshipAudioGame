using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace BattleshipAudioGame.ViewModel
{
    // Classe simples para implementar ICommand sem precisar criar uma nova classe de comando
    // para cada ação.
    public class RelayCommand : ICommand
    {
        //Action<object> é o método que será executado quando o comando for chamado
        private readonly Action<object> _execute;
        //Func<object, bool> é a lógica que define se o comando pode ou não ser executado
        private readonly Func<object, bool> _canExecute;


        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}
