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
        var choices = new Choices("play", "stop", "exit","yes","no");
        _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(choices)));
        _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        _recognizer.SetInputToDefaultAudioDevice();
        _recognizer.RecognizeAsync(RecognizeMode.Multiple);

    }

    private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        Console.WriteLine($"Recognized: {e.Result.Text}");
        if (e.Result.Text == "play")
        {
            Button_Click(this, new RoutedEventArgs());
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Children.Clear();
        _synthesizer.Speak("Hello, welcome to the Battleship Audio Game!");

        var newText = new TextBlock
        {
            Text = "Do you wanna hear the tutorial?",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20
        };
        

        MainContent.Children.Add(newText);
        _synthesizer.Speak("Do you wanna hear the tutorial?");

    }
        
}