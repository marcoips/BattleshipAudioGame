using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Speech.Synthesis;
using BattleshipAudioGame.services; // Namespace para síntese de fala    

namespace BattleshipAudioGame.ViewModel
{
    // ViewModel principal, que gere o jogo.
    public class MainWindowViewModel : BaseViewModel
    {
        
        public BoardViewModel PlayerBoard { get; set; }
        public BoardViewModel EnemyBoard { get; set; }

        //comandos 
        public ICommand StartGameCommand { get; }
        public ICommand AttackCommand { get; }

        //Propriedades de fala (speech)
        private SpeechService _speechService;

        public MainWindowViewModel()
        {
            // Inicializa os tabuleiros do jogador e do inimigo (10x10)
            PlayerBoard = new BoardViewModel(10, 10);
            EnemyBoard = new BoardViewModel(10, 10);

            // Inicializa o serviço de fala
            _speechService = new SpeechService();

            // Inicializa os comandos
            StartGameCommand = new RelayCommand(StartGame);
            AttackCommand = new RelayCommand(AttackCell);
        }


        private void StartGame(object parameter)
        {
            //Posicionar navios no tabuleiro
            PlayerBoard = new BoardViewModel(10, 10);
            EnemyBoard = new BoardViewModel(10, 10);

            //Fala algo no ínicio do jogo
            _speechService.Speak("Jogo iniciado! .");
        }


        private void AttackCell(object parameter)
        {
            // Verifica se o parâmetro é uma célula válida
            if (parameter is CellViewModel cellVm)
            {
                // Executa o ataque na célula
                bool hit = EnemyBoard.AttackCell(cellVm);
                // Se a célula foi atingida, fala "Hit"
                if (hit)
                {
                    _speechService.Speak($"Acertou! Posição {cellVm.Row},{cellVm.Column}.");
                }
                else
                {
                    _speechService.Speak($"água! Posição {cellVm.Row},{cellVm.Column}.");
                }
            }
        }

    }
}
