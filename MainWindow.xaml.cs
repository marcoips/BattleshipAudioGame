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

    private string _currentContext = string.Empty;
    public MainWindow()
    {
        InitializeComponent();
        _synthesizer = new SpeechSynthesizer();

        _currentContext = "start";

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

        //exit geral
        if (e.Result.Text == "exit")
        {
            _synthesizer.Speak("Exiting the game. Goodbye!");
            Application.Current.Shutdown(); // Close the application
            return; // Exit the method to prevent further processing
        }


        //diferentes fases do jogo
        if (_currentContext == "start")
        {
            if (e.Result.Text == "play")
            {
                Button_Click(this, new RoutedEventArgs());
            }

        }
        else if (_currentContext == "tutorial")
        {
            if (e.Result.Text == "yes")
            {
                SpeakTutorial();
                _currentContext = string.Empty; // Reset context
            }
            else if (e.Result.Text == "no")
            {
                _synthesizer.Speak("Alright, skipping the tutorial.");
                _currentContext = string.Empty; // Reset context
            }
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
        _currentContext = "tutorial";


    }

    private void SpeakTutorial()
    {
        _synthesizer.Speak("The Battleship Audio Game is a voice-controlled game where you command your fleet to sink enemy ships. Use commands like fire, move, and scan to play. Good luck!");

        DisplayGrid();
    }

    private void DisplayGrid()
    {
        MainContent.Children.Clear();

        var grid = new Grid();

        // Define 10 rows and 10 columns
        for (int i = 0; i < 10; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
        }

        // Letters for rows (A-J)
        string[] rowLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        // Add buttons to each cell
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                var button = new Button
                {
                    Content = $"{rowLabels[row]}{col + 1}", // Combine letter and number
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                // Add the button to the grid
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                grid.Children.Add(button);
            }
        }

        // Add the grid to the MainContent container
        MainContent.Children.Add(grid);

        // Update the context to "game"
        _currentContext = "game";
    }

}