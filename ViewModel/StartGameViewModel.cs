using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BattleshipAudioGame.services;

namespace BattleshipAudioGame.ViewModel
{
    public class StartGameViewModel : BaseViewModel, IDisposable
    {
        // Propriedades e métodos específicos para a lógica de negócios da tela inicial do jogo.
        // Aqui você pode adicionar propriedades como NomeDoJogador, Dificuldade, etc.
        // e métodos para iniciar o jogo, mostrar tutoriais, etc.

        private readonly SpeechService _speechService;
        //comandos que a view (XMAL) VAI INAR NOS BOTOES
        public ICommand PlayGameCommand { get; }
        public ICommand TutorialCommand { get; }
        public ICommand ExitCommand { get; }

        //Podemos ter um callback 
        private Action<string> _navigateAction;

        public StartGameViewModel(Action<string> navigateAction = null)
        {
            _navigateAction = navigateAction;

            _speechService = new SpeechService();

            // Inicializa os comandos
            PlayGameCommand = new RelayCommand(_ => PlayGame());
            TutorialCommand = new RelayCommand(_ => Tutorial());
            ExitCommand = new RelayCommand(_ => Exit());

            // Assina o evento de voz
            _speechService.OnSpeechRecognized += OnSpeechRecognized;


        }

        public void Initialize()
        {
            _speechService.Speak("Welcome to Battleship!!! Say play. listen to tutorial. or exit.");

            // Prepara o array de strings que queremos reconhecer
            var commands = new string[] { "jogar", "ouvir tutorial", "tutorial", "sair" };
            _speechService.StartRecognition(commands, "pt-PT");
        }

        public void Cleanup()
        {
            _speechService.StopRecognition();
        }

        private void OnSpeechRecognized(string recognized)
        {
            if (string.IsNullOrEmpty(recognized)) return;

            // Exemplo bem simples
            if (recognized.Contains("jogar"))
            {
                PlayGame();
            }
            else if (recognized.Contains("tutorial"))
            {
                Tutorial();
            }
            else if (recognized.Contains("sair"))
            {
                Exit();
            }
        }

        private void PlayGame()
        {
            // Aqui podemos navegar para outra tela, chamar _navigateAction("Game"), etc.
            _speechService.Speak("Iniciando o jogo!");
            _navigateAction?.Invoke("Game");
            // ou algo do tipo
        }

        private void Tutorial()
        {
            _speechService.Speak("Este é o tutorial do Batalha Naval...");
        }

        private void Exit()
        {
            // Fecha aplicação ou chama um callback de navegação
            _speechService.Speak("Até breve!");
            App.Current.Shutdown();
        }

        public void Dispose()
        {
            Cleanup();
            _speechService.Dispose();
        }
    }
}
