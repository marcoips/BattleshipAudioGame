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
using BattleshipAudioGame;

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
                DisplayGrid();
            }
        }
        else if (_currentContext == "game")
        {
            if (e.Result.Text == "stop")
            {
                _synthesizer.Speak("Game paused. Say 'play' to continue.");
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

        _synthesizer.Speak("Game Start. Place your ships");

        // Create an instance of the BoardViewModel for both player and CPU
        var playerBoardViewModel = new BoardViewModel();
        var cpuBoardViewModel = new BoardViewModel();

        // Create a container to hold both grids
        var container = new Grid();
        container.ColumnDefinitions.Add(new ColumnDefinition());
        container.ColumnDefinitions.Add(new ColumnDefinition());

        // Create the player's grid
        var playerGrid = CreateBoardGrid(playerBoardViewModel, "Player Board");
        Grid.SetColumn(playerGrid, 0);
        container.Children.Add(playerGrid);

        // Create the CPU's grid
        var cpuGrid = CreateBoardGrid(cpuBoardViewModel, "CPU Board");
        Grid.SetColumn(cpuGrid, 1);
        container.Children.Add(cpuGrid);

        // Add the container to the MainContent
        MainContent.Children.Add(container);

        // Update the context to "game"
        _currentContext = "game";
    }

    private Grid CreateBoardGrid(BoardViewModel boardViewModel, string title)
    {
        // Create a Grid to display the board
        var gridContainer = new Grid();

        // Define 12 rows (1 extra for title) and 11 columns (1 extra for labels)
        for (int i = 0; i < 12; i++)
        {
            gridContainer.RowDefinitions.Add(new RowDefinition());
        }
        for (int i = 0; i < 11; i++)
        {
            gridContainer.ColumnDefinitions.Add(new ColumnDefinition());
        }

        // Add the title at the top
        var titleLabel = new TextBlock
        {
            Text = title,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20,
            FontWeight = FontWeights.Bold
        };
        Grid.SetRow(titleLabel, 0);
        Grid.SetColumnSpan(titleLabel, 11);
        gridContainer.Children.Add(titleLabel);

        // Add row labels (A-J) to the first column
        for (int row = 1; row <= 10; row++)
        {
            var label = new TextBlock
            {
                Text = boardViewModel.RowLabels[row - 1],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
            };

            Grid.SetRow(label, row);
            Grid.SetColumn(label, 0);
            gridContainer.Children.Add(label);
        }

        // Add column labels (1-10) to the first row
        for (int col = 1; col <= 10; col++)
        {
            var label = new TextBlock
            {
                Text = boardViewModel.ColumnLabels[col - 1],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
            };

            Grid.SetRow(label, 1);
            Grid.SetColumn(label, col);
            gridContainer.Children.Add(label);
        }

        // Add buttons to the grid (10x10 starting from row 2, column 1)
        foreach (var cell in boardViewModel.Cells)
        {
            var button = new Button
            {
                Content = cell.Content, // Bind content from the ViewModel
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Add the button to the grid
            Grid.SetRow(button, cell.Row + 2);
            Grid.SetColumn(button, cell.Column + 1);
            gridContainer.Children.Add(button);
        }

        return gridContainer;
    }
}