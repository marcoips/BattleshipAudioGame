using BattleshipAudioGame.Serviços;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BattleshipAudioGame.ViewModel
{
    public class StartGameViewModel : BaseViewModel, IDisposable
    {

        private readonly VoiceRecognitionService _voice;
        private readonly Action<string> _navigate;

        public ICommand PlayCommand { get; }
        public ICommand TutorialCommand { get; }
        public ICommand ExitCommand { get; }


        /* ------------- Commands ------------- */
        public ICommand PlayAudioCommand { get; }
        public ICommand PlaySilentCommand { get; }
        public ICommand ExitAudioCommand { get; }

        public StartGameViewModel(Action<string> navigate)
        {

            _navigate = navigate;
            _voice = new VoiceRecognitionService();


            /* ---------- inicia voz ---------- */
            _voice.OnCommand += HandleVoice;
            _voice.Speak("Welcome to Battleship!!"
+                       "Say play, tutorial, or exit!!");
            string[] cmds = { "Play", "tutorial", "Exit" };
            _voice.StartRecognition(cmds);          // usa engine padrão


            PlayCommand = new RelayCommand(_ => _navigate("Placement"));
            TutorialCommand = new RelayCommand(_ => ShowTutorial());
            ExitAudioCommand = new RelayCommand(_ => _navigate("Exit"));
        }



        /* ---- Lógica de voz ---- */
        private void HandleVoice(string txt)
        {
            if (txt.Contains("play"))
                _navigate("Placement");
            else if (txt.Contains("tutorial"))
                ShowTutorial();
            else if (txt.Contains("Exit"))
                _navigate("Exit");
        }

        

        private void ShowTutorial()
        {
            string path = "C:\\Users\\joao1\\source\\repos\\BattleShipAudioGame\\Flas\\Tutorial.txt";

            if(File.Exists(path))
            {
                string text = File.ReadAllText(path);
                _voice.Speak(text);
            }
            else
            {
                _voice.Speak("Sorry, the tutorial file was not found.");
            }
        }

        public void Dispose() => _voice.Dispose();

    }
}
