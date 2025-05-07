using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BattleshipAudioGame.ViewModel
{
    public class StartGameViewModel : BaseViewModel 
    {
        private readonly Action<string> _navigate;

        public ICommand PlayCommand { get; }
        public ICommand TutorialCommand { get; }
        public ICommand ExitCommand { get; }

        public StartGameViewModel(Action<string> navigate)
        {
            _navigate = navigate;

            PlayCommand = new RelayCommand(_ => _navigate("Placement"));
            TutorialCommand = new RelayCommand(_ => ShowTutorial());
            ExitCommand = new RelayCommand(_ => _navigate("Exit"));
        }
        
        private void ShowTutorial()
        {
            MessageBox.Show("Tutorial placeholder (vamos substituir por voz depois).");
        }

    }
}
