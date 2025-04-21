using System.Collections.ObjectModel;

namespace BattleshipAudioGame;

public class BoardViewModel
{
    public ObservableCollection<string> RowLabels { get; set; }
    public ObservableCollection<string> ColumnLabels { get; set; }
    public ObservableCollection<GridCell> Cells { get; set; }

    public BoardViewModel()
    {
        // Initialize row and column labels
        RowLabels = new ObservableCollection<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        ColumnLabels = new ObservableCollection<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        // Initialize grid cells
        Cells = new ObservableCollection<GridCell>();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Cells.Add(new GridCell
                {
                    Row = row,
                    Column = col,
                    Content = string.Empty // No content initially
                });
            }
        }
    }
}

public class GridCell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Content { get; set; }
}
