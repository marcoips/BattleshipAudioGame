using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;                   //  ← para usar .First(.)
using BattleshipAudioGame.Model;
using System.Collections.Generic;   //  ← IEnumerable<>

namespace BattleshipAudioGame;

/// <summary>
/// Representa um tabuleiro 10×10; guarda as células e (opcionalmente)
/// a lista de navios colocados.
/// </summary>
public class BoardViewModel
{
    public ObservableCollection<string> RowLabels { get; }
    public ObservableCollection<string> ColumnLabels { get; }
    public ObservableCollection<GridCell> Cells { get; }
    public List<Navio> Navios { get; set; } = new();      // pode vir vazio
    public string BoardTitle { get; set; } = "Tabuleiro";

    public BoardViewModel()
    {
        RowLabels = new ObservableCollection<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        ColumnLabels = new ObservableCollection<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        Cells = new ObservableCollection<GridCell>();
        for (int r = 0; r < 10; r++)
            for (int c = 0; c < 10; c++)
                Cells.Add(new GridCell { Row = r, Column = c, Content = string.Empty });
    }

    /// <summary>
    /// Pinta rapidamente as células ocupadas pelos navios indicados.
    /// </summary>
    public void PreencherNavios(IEnumerable<Navio> navios, Brush cor)
    {
        foreach (var ship in navios)
        {
            foreach (var pos in ship.localizacao)
            {
                int row = pos[0] - 'A';                     // 'A'→0
                int col = int.Parse(pos[1..]) - 1;          // "1"→0

                var cell = Cells.First(c => c.Row == row && c.Column == col);
                cell.Content = ship.nome_navio[0].ToString();
                cell.Background = cor;                      // usa a cor recebida
            }
        }
    }
}

public class GridCell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Content { get; set; } = string.Empty;
    public Brush Background { get; set; } = Brushes.LightGray;
}
