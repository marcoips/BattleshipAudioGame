using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis; // Namespace para síntese de fala
using System.Speech.Recognition; // Namespace para reconhecimento de fala
using System.Windows; // Namespace para interação com a interface do usuário
using System;

// Classe simples para realizar a fala de textos.
namespace BattleshipAudioGame.services
{
    public class SpeechService :IDisposable
    {

        private readonly SpeechSynthesizer _synthesizer;
        private SpeechRecognitionEngine _recognizer;

        //Evento que a VM pode assinar para saber quando algo foi reconhecido
        public event Action<string> OnSpeechRecognized;

        //Construtor
        public SpeechService()
        {
            // Inicializa o sintetizador de fala
            _synthesizer = new SpeechSynthesizer
            {
                Volume = 100, // Volume de 0 a 100
                Rate = 0 // Taxa de fala de -10 a 10
            };

        }

        // Lê em voz alta o texto fornecido.
        public void Speak(string text)
        {
            //  personalizar volume, velocidade, etc. Exemplo:
          
            _synthesizer.Speak(text);

        }

        // Inicia o reconhecimento de voz com base em um array de comandos conhecidos.
        public void StartRecognition(string[] commands, string cultureName = "en-US")
        {
            try
            {
                _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo(cultureName));

                //Definimos o mivrofone padrão
                _recognizer.SetInputToDefaultAudioDevice();

                //criamos a gramatica simples
                var choices = new Choices(commands);
                var gb = new GrammarBuilder(choices);
                var grammar = new Grammar(gb);
                _recognizer.LoadGrammar(grammar);

                //Assinamos o evento de reconhecimento
                _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

                //Iniciamos o reconhecimento 
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception ex)
            {
                // Caso ocorra um erro, podemos logar ou tratar de alguma forma
                Console.WriteLine($"Erro ao iniciar o reconhecimento de voz: {ex.Message}");
            }
            
        }

        // Para o reconhecimento e descarta a engine.
        public void StopRecognition()
        {
            if (_recognizer != null)
            {
                _recognizer.RecognizeAsyncStop();
                _recognizer.SpeechRecognized -= Recognizer_SpeechRecognized;
                _recognizer.Dispose();
                _recognizer = null;
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Verifica se o resultado é válido
            var recognizedText = e.Result.Text?.ToLower();
            OnSpeechRecognized?.Invoke(recognizedText);
        }

        public void Dispose()
        {
            // Libera os recursos do sintetizador de fala
           
            StopRecognition();
            _synthesizer?.Dispose();
        }
    }
}
