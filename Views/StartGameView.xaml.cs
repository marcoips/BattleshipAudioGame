using System.Windows;
using BattleshipAudioGame.services; // Namespace para síntese de fala
using System.Windows.Controls;
using System.Windows.Navigation;
using BattleshipAudioGame.ViewModel;


namespace BattleshipAudioGame.Views
{
    /// <summary>
    /// Interação lógica para StartGameView.xam
    /// </summary>
    public partial class StartGameView : Page
    {
        private SpeechService _speechService;
        private StartGameViewModel _viewModel;
        public StartGameView()
        {
            InitializeComponent();

            // Cria o VM (ou poderia receber de fora)
            _viewModel = new StartGameViewModel();
            this.DataContext = _viewModel;

            // Liga eventos de carregamento do UserControl
            this.Loaded += StartGameView_Loaded;
            this.Unloaded += StartGameView_Unloaded;

        }
        private void StartGameView_Loaded(object sender, RoutedEventArgs e)
        {
            // Chama o Initialize do VM
            _viewModel.Initialize();
        }

        private void StartGameView_Unloaded(object sender, RoutedEventArgs e)
        {
            // Chama o Cleanup do VM
            _viewModel.Cleanup();
        }
      
    }
}
