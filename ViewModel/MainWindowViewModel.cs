using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using BattleshipAudioGame.Serviços;

namespace BattleshipAudioGame.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;
        private object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
            {
                CurrentView = new BoardViewModel();
            }

        private void Navigate(string dest)
        {
            CurrentView = dest switch
            {
                //"Placement" => new PlacementViewModel(Navigate), // criar mais tarde
                //"Game" => new BattleViewModel(Navigate),    // criar mais tarde
                "Start" => new StartGameViewModel(Navigate),
                _ => CurrentView
            };

            if (dest == "Exit")
            {
                Application.Current.Shutdown();
            }
        }
    }
}
