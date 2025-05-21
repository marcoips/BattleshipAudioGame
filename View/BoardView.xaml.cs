using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipAudioGame.View
{
    /// <summary>
    /// Interação lógica para BoardView.xam
    /// </summary>
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();
            Loaded += (_, __) =>
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[BoardView] PlaceCommand == {(PlaceCommand is null ? "NULL" : "OK")}");
            };
        }
        /*  Comando que a célula irá invocar  */
        public static readonly DependencyProperty PlaceCommandProperty =
            DependencyProperty.Register(nameof(PlaceCommand),
                typeof(ICommand), typeof(BoardView));

        public ICommand PlaceCommand
        {
            get => (ICommand)GetValue(PlaceCommandProperty);
            set => SetValue(PlaceCommandProperty, value);
        }
    }
}
