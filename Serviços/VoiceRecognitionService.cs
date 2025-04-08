using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Serviços
{
    public class VoiceRecognitionService
    {
        private SpeechRecognitionEngine _recognizer;
        
        public event EventHandler<string> CommandRecognized;

        public VoiceRecognitionService()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }




    }
}
