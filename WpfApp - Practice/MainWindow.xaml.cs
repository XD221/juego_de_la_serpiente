using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApp___Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        public MainWindow()
        {
            InitializeComponent();
            this.gameBoard.Width = 790;
            this.gameBoard.Height = 790;
            game = new Game(this.gameBoard, 1, this.game_message, this.game_point);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Trace.WriteLine(e.Key.ToString());
            switch(e.Key)
            {
                case Key.Left:
                    game.playerMoveLeft();
                    break;
                case Key.Right:
                    game.playerMoveRight();
                    break;
                case Key.Up:
                    game.playerMoveUp();
                    break;
                case Key.Down:
                    game.playerMoveDown();
                    break;
                case Key.Space:
                    if(game.isNewGameEnable()) game.newGame();
                    else
                    {
                        //
                    }
                    break;
            }
        }
    }
}
