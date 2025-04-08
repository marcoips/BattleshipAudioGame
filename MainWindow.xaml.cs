using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System;

namespace BattleshipAudioGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SpeechRecognitionEngine _recognizer;
    private SpeechSynthesizer _synthesizer;
    public MainWindow()
    {
        InitializeComponent();
        _synthesizer = new SpeechSynthesizer();

        //speech recognition
        _recognizer = new SpeechRecognitionEngine();
        var choices = new Choices("play", "stop", "exit");
        _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("play")));
        _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        _recognizer.SetInputToDefaultAudioDevice();
        _recognizer.RecognizeAsync(RecognizeMode.Multiple);

    }

    private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        Console.WriteLine($"Recognized: {e.Result.Text}");
        if (e.Result.Text == "play")
        {
            _synthesizer.Speak("Hello, welcome to the Battleship Audio Game!");
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _synthesizer.Speak("Hello, welcome to the Battleship Audio Game!");

    }
        
}