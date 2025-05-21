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
    public class StartGameViewModel : BaseViewModel
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

        public StartGameViewModel(Action<string> navigate, VoiceRecognitionService voice
            )
        {

            _navigate = navigate;
            _voice = voice;



            PlayCommand = new RelayCommand(_ => _navigate("Placement"));
            TutorialCommand = new RelayCommand(_ => ShowTutorial());
            ExitAudioCommand = new RelayCommand(_ => _navigate("exit"));

            /* ---------- inicia voz ---------- */
            _voice.OnCommand += HandleVoice;
            _voice.Speak("Welcome to Battleship!!"
+                       "Say play, tutorial, or exit!!");
            string[] cmds = { "play", "tutorial", "Exit" };
            _voice.StartRecognition(cmds);          // usa engine padrão


            
        }



        /* ---- Lógica de voz ---- */
        private void HandleVoice(string txt)
        {
            if (txt.Contains("play"))
            {
                _voice.OnCommand -= HandleVoice;
                _voice.StopRecognition();
                _navigate("Placement");
            }
          
            else if (txt.Contains("tutorial"))
                ShowTutorial();
            else if (txt.Contains("exit"))
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

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;          // evita repetir no finalizador
            _voice.OnCommand -= HandleVoice; // desliga handler
            _voice.StopRecognition();        // liberta microfone
        }


    }
}
