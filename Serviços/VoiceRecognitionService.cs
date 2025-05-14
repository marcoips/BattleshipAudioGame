using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Serviços
{
    public class VoiceRecognitionService : IDisposable
    {
        private SpeechRecognitionEngine? _recognizer;
        private readonly SpeechSynthesizer _synth;

        public event EventHandler<string> CommandRecognized;
        public event Action<string>? OnCommand;     // dispara “play”, “vertical”, ...

        public VoiceRecognitionService()
        {
            _synth = new SpeechSynthesizer
            {
                Volume = 100,   // 0‑100
                Rate = 0      // -10…10
            };

        }

        /* ------------ Síntese ------------ */
        public void Speak(string text) => _synth.SpeakAsync(text);

        /* ------------ Reconhecimento ----- */
        public void StartRecognition(string[] commands)
        {
            StopRecognition(); // descarta engine anterior

            _recognizer = new SpeechRecognitionEngine();
            _recognizer.SetInputToDefaultAudioDevice();

            var grammar = new Grammar(new GrammarBuilder(new Choices(commands)));
            _recognizer.LoadGrammar(grammar);

           _recognizer.SpeechRecognized += (_, e) =>
           {
               var txt = e.Result.Text?.ToLowerInvariant();
               if (!string.IsNullOrWhiteSpace(txt))
                   OnCommand?.Invoke(txt);
           };

            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }


        public void StopRecognition()
        {
            if (_recognizer == null) return;
            _recognizer.RecognizeAsyncStop();
            _recognizer.Dispose();
            _recognizer = null;
        }

        

        /* ------------ Limpeza ------------ */
        public void Dispose()
        {
            StopRecognition();
            _synth.Dispose();
        }
    }
}
