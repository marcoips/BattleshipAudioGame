using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis; // Namespace para síntese de fala

// Classe simples para realizar a fala de textos.
namespace BattleshipAudioGame.services
{
    public class SpeechService
    {

        private SpeechSynthesizer _synthesizer;

        public SpeechService()
        {
            _synthesizer = new SpeechSynthesizer();
        }

        // Lê em voz alta o texto fornecido.
        public void Speak(string text)
        {
            //  personalizar volume, velocidade, etc. Exemplo:
            // _synthesizer.Volume = 100;  0 a 100
            // _synthesizer.Rate = 0;   -10 a 10

            _synthesizer.Speak(text);


        }

    }
}
