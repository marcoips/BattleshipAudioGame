using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipAudioGame;
using BattleshipAudioGame.Model;

namespace BattleshipAudioGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SpeechRecognitionEngine _recognizer;
    private SpeechSynthesizer _synthesizer;

    private string _currentContext = string.Empty;

    private string _selectedPosition = string.Empty;
    private string _selectedDirection = string.Empty;

    private BoardViewModel playerBoardViewModel;
    private BoardViewModel cpuBoardViewModel;

    private List<Navio> shipsToPlace;

    public MainWindow()
    {
        InitializeComponent();
        _synthesizer = new SpeechSynthesizer();

        _currentContext = "start";

        // Speech recognition
        _recognizer = new SpeechRecognitionEngine();

        var positionChoices = new Choices();
        for (char letter = 'A'; letter <= 'J'; letter++) // Letters A-J
        {
            for (int number = 1; number <= 10; number++) // Numbers 1-10
            {
                positionChoices.Add($"{letter}{number}");
            }
        }
        var fixedChoices = new Choices("play", "stop", "exit", "yes", "no", "horizontal", "vertical");

        var grammarBuilder = new GrammarBuilder();
        grammarBuilder.Append(new Choices(fixedChoices, positionChoices));
        var grammar = new Grammar(grammarBuilder);

        _recognizer.LoadGrammar(grammar);
        _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        _recognizer.SetInputToDefaultAudioDevice();
        _recognizer.RecognizeAsync(RecognizeMode.Multiple);

        playerBoardViewModel = new BoardViewModel();
        cpuBoardViewModel = new BoardViewModel();
        GenerateCpuShips(cpuBoardViewModel);

        // Initialize the list of ships to place
        shipsToPlace = new List<Navio>
        {
            new Navio("Carrier", 5, false, new List<string>()),
            new Navio("Battleship", 4, false, new List<string>()),
            new Navio("Cruiser", 3, false, new List<string>()),
            new Navio("Submarine", 3, false, new List<string>()),
            new Navio("Destroyer", 2, false, new List<string>())
        };
    }

    private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        Console.WriteLine($"Recognized: {e.Result.Text}");

        // Exit command
        if (e.Result.Text == "exit")
        {
            _synthesizer.Speak("Exiting the game. Goodbye!");
            Application.Current.Shutdown(); // Close the application
            return;
        }

        // Different phases of the game
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
            string input = e.Result.Text;

            if (input.Length >= 2 && char.IsLetter(input[0]) && int.TryParse(input.Substring(1), out int number))
            {
                _selectedPosition = $"{char.ToUpper(input[0])}{number}";
                _synthesizer.Speak($"You selected position {_selectedPosition}, sounds correct?");
                _currentContext = "confirm_position";
            }
        }
        else if (_currentContext == "confirm_position")
        {
            if (e.Result.Text == "yes")
            {
                _synthesizer.Speak("Position confirmed. Now say the direction: horizontal or vertical?");
                _currentContext = "select_direction";
            }
            else if (e.Result.Text == "no")
            {
                _synthesizer.Speak("Position not confirmed. Please say the position again.");
                _currentContext = "game";
            }
        }
        else if (_currentContext == "select_direction")
        {
            if (e.Result.Text == "horizontal" || e.Result.Text == "vertical")
            {
                _selectedDirection = e.Result.Text;
                _synthesizer.Speak($"You selected {_selectedDirection} direction. Placing the ship...");

                if (TryPlaceShip(_selectedPosition, _selectedDirection))
                {
                    if (shipsToPlace.Count > 0)
                    {
                        _synthesizer.Speak($"The {shipsToPlace[0].nome_navio} is next. Please place it.");
                        _currentContext = "game"; // Continue placing ships
                    }
                    else
                    {
                        _synthesizer.Speak("All ships have been placed. Starting the game.");
                        _currentContext = "start_game"; // Transition to the game phase
                    }
                }
                else
                {
                    _synthesizer.Speak("Invalid placement. Please try again.");
                    _currentContext = "game";
                }
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

        // Create a container to hold both grids
        var container = new Grid();
        container.ColumnDefinitions.Add(new ColumnDefinition());
        container.ColumnDefinitions.Add(new ColumnDefinition());

        // Create the player's grid
        var playerGrid = CreateBoardGrid(playerBoardViewModel, "Player Board");
        Grid.SetColumn(playerGrid, 0);
        container.Children.Add(playerGrid);

        // Create the CPU's grid (static)
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
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = cell.Background
            };

            // Add the button to the grid
            Grid.SetRow(button, cell.Row + 2);
            Grid.SetColumn(button, cell.Column + 1);
            gridContainer.Children.Add(button);
        }

        return gridContainer;
    }

    private List<string> GeneratePositionsCPU(int shipSize, List<string> occupiedPositions)
    {
        var random = new Random();
        var positions = new List<string>();
        bool isValidPlacement = false;

        while (!isValidPlacement)
        {
            positions.Clear();

            // Randomly choose orientation: 0 = horizontal, 1 = vertical
            var orientation = random.Next(0, 2);

            // Randomly choose starting position
            var startRow = random.Next(0, 10);
            var startCol = random.Next(0, 10);

            // Generate positions based on orientation
            for (int i = 0; i < shipSize; i++)
            {
                if (orientation == 0) // Horizontal
                {
                    if (startCol + shipSize > 10) break; // Ensure it fits horizontally
                    positions.Add($"{(char)('A' + startRow)}{startCol + i + 1}");
                }
                else // Vertical
                {
                    if (startRow + shipSize > 10) break; // Ensure it fits vertically
                    positions.Add($"{(char)('A' + startRow + i)}{startCol + 1}");
                }
            }

            // Check if all positions are valid and not overlapping
            if (positions.Count == shipSize && !positions.Any(pos => occupiedPositions.Contains(pos)))
            {
                isValidPlacement = true;
            }
        }

        return positions;
    }

    private void GenerateCpuShips(BoardViewModel boardViewModel)
    {
        var occupiedPositions = new List<string>();

        // Add ships to the CPU's board with random positions
        var cpuCarrier = new Navio("Carrier", 5, false, GeneratePositionsCPU(5, occupiedPositions));
        occupiedPositions.AddRange(cpuCarrier.localizacao);

        var cpuBattleship = new Navio("Battleship", 4, false, GeneratePositionsCPU(4, occupiedPositions));
        occupiedPositions.AddRange(cpuBattleship.localizacao);

        var cpuCruiser = new Navio("Cruiser", 3, false, GeneratePositionsCPU(3, occupiedPositions));
        occupiedPositions.AddRange(cpuCruiser.localizacao);

        var cpuDestroyer = new Navio("Destroyer", 2, false, GeneratePositionsCPU(2, occupiedPositions));
        occupiedPositions.AddRange(cpuDestroyer.localizacao);

        var cpuSubmarine = new Navio("Submarine", 3, false, GeneratePositionsCPU(3, occupiedPositions));
        occupiedPositions.AddRange(cpuSubmarine.localizacao);

        boardViewModel.Navios = new List<Navio> { cpuCarrier, cpuBattleship, cpuCruiser, cpuDestroyer, cpuSubmarine };

        // Update the grid cells to reflect the CPU's ships
        foreach (var ship in boardViewModel.Navios)
        {
            foreach (var position in ship.localizacao)
            {
                var row = position[0] - 'A'; // Convert row letter to index (e.g., 'A' -> 0)
                var col = int.Parse(position.Substring(1)) - 1; // Convert column number to index (e.g., "1" -> 0)

                var cell = boardViewModel.Cells.FirstOrDefault(c => c.Row == row && c.Column == col);
                if (cell != null)
                {
                    cell.Content = ship.nome_navio[0].ToString(); // Use the first letter of the ship's name
                    cell.Background = Brushes.Red; // Change background color to indicate ship presence
                }
            }
        }
    }

    private bool TryPlaceShip(string startPosition, string direction)
    {
        if (shipsToPlace.Count == 0)
        {
            _synthesizer.Speak("All ships have been placed. Starting the game.");
            return false;
        }

        // Get the next ship to place
        var currentShip = shipsToPlace[0];
        int shipSize = currentShip.tamanho_navio;

        // Parse the starting position
        int startRow = startPosition[0] - 'A';
        int startCol = int.Parse(startPosition.Substring(1)) - 1;

        var positions = new List<string>();

        for (int i = 0; i < shipSize; i++)
        {
            if (direction == "horizontal")
            {
                if (startCol + i >= 10) return false; // Out of bounds
                positions.Add($"{(char)('A' + startRow)}{startCol + i + 1}");
            }
            else if (direction == "vertical")
            {
                if (startRow + i >= 10) return false; // Out of bounds
                positions.Add($"{(char)('A' + startRow + i)}{startCol + 1}");
            }
        }

        // Check for overlap with existing ships
        if (positions.Any(pos => playerBoardViewModel.Navios.Any(ship => ship.localizacao.Contains(pos))))
        {
            return false;
        }

        // Place the ship
        currentShip.localizacao = positions;
        playerBoardViewModel.Navios.Add(currentShip);

        // Update the grid cells
        foreach (var position in positions)
        {
            var row = position[0] - 'A';
            var col = int.Parse(position.Substring(1)) - 1;

            var cell = playerBoardViewModel.Cells.FirstOrDefault(c => c.Row == row && c.Column == col);
            if (cell != null)
            {
                cell.Content = currentShip.nome_navio[0].ToString();
                cell.Background = Brushes.Green;
            }
        }

        // Remove the placed ship from the list
        shipsToPlace.RemoveAt(0);

        // Refresh the player's grid
        DisplayGrid();

        // Check if there are more ships to place
        if (shipsToPlace.Count > 0)
        {
            _synthesizer.Speak($"The {shipsToPlace[0].nome_navio} is next. Please place it.");
        }
        else
        {
            _synthesizer.Speak("All ships have been placed. Starting the game.");
        }

        return true;
    }
}
