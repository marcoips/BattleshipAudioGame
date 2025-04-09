

using System.Windows;
using System.Windows.Controls;
using BattleshipAudioGame.Views;


namespace BattleshipAudioGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //Navega para a page StartGameView assim que a janela é carregada
        MainFrame.Navigate(new StartGameView());
    }
}
