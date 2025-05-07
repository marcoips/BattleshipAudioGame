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
        public object CurrentView
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
                CurrentView = new StartGameViewModel(Navigate);
            }

        private void Navigate(string dest)
        {
            System.Diagnostics.Debug.WriteLine($"NAVIGATE → {dest}");
            CurrentView = dest switch
            {
                "Placement" => new PlacementViewModel(Navigate), // criar mais tarde
                //"Game" => new BattleViewModel(Navigate),    // criar mais tarde
                "Start" => new StartGameViewModel(Navigate),
                _ => CurrentView
            };
            System.Diagnostics.Debug.WriteLine($"CurrentView = {CurrentView?.GetType().Name}");

            if (dest == "Exit")
            {
                Application.Current.Shutdown();
            }
        }
    }
}
