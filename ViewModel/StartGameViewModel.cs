using BattleshipAudioGame.Serviços;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Action<bool> _setAudio;
        private readonly Action<string> _navigate;

        public ICommand PlayCommand { get; }
        public ICommand TutorialCommand { get; }
        public ICommand ExitCommand { get; }


        /* ------------- Commands ------------- */
        public ICommand PlayAudioCommand { get; }
        public ICommand PlaySilentCommand { get; }
        public ICommand ExitAudioCommand { get; }

        public StartGameViewModel(Action<string> navigate, Action <bool> setAudioFlag)
        {

            _navigate = navigate;
            _setAudio = setAudioFlag;
            _voice = new VoiceRecognitionService();

            /* ---------- botões (opcionais) ---------- */
            PlayAudioCommand = new RelayCommand(_ => StartGame(true));
            PlaySilentCommand = new RelayCommand(_ => StartGame(false));
            ExitAudioCommand = new RelayCommand(_ => _navigate("Exit"));

            /* ---------- diálogo de voz ---------- */
            _voice.OnCommand += HandleIntroVoice;
            _voice.StartRecognition(new[] { "áudio", "silêncio", "sair" });
            _voice.Speak("Bem‑vindo ao Batalha Naval. "
                       + "Deseja jogar com áudio? Diga 'áudio' ou 'silêncio'.");

            PlayCommand = new RelayCommand(_ => _navigate("Placement"));
            TutorialCommand = new RelayCommand(_ => ShowTutorial());
            ExitAudioCommand = new RelayCommand(_ => _navigate("Exit"));
        }

        private void HandleIntroVoice(string cmd)
        {
            if (cmd.Contains("áudio")) StartGame(true);
            else if (cmd.Contains("silêncio")) StartGame(false);
            else if (cmd.Contains("sair")) _navigate("Exit");
        }

        private void StartGame(bool audio)
        {
            _setAudio(audio);          // grava escolha global
            _voice.StopRecognition();  // menu acabou
            _navigate("Placement");
        }

        public void Dispose() => _voice.Dispose();

        private void ShowTutorial()
        {
            MessageBox.Show("Tutorial placeholder (vamos substituir por voz depois).");
        }

    }
}
