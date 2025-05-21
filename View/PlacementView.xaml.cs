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
    /// Interação lógica para PlacementView.xam
    /// </summary>
    public partial class PlacementView : UserControl
    {
        public PlacementView()
        {
            InitializeComponent();

            Unloaded += (_, __) =>
            (DataContext as IDisposable)?.Dispose();
        }

    }
}
