using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp___Practice
{
    internal class Game
    {
        private Canvas board;
        private double limit = 25;
        private Player j1;
        private Food food;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private TextBlock game_message;
        private TextBlock game_point;
        private Boolean new_game;

        private int player_width;
        private int player_height;
        private int food_size;
        public Game(Canvas board, int lvl, TextBlock game_message, TextBlock game_point)
        {
            this.new_game = false;
            this.board = board;
            this.game_message = game_message;
            this.game_point = game_point;
            Player.lvl = lvl;
            Player.px = (int)(this.board.Width / 2);
            Player.py = (int)(this.board.Height / 2);
            this.player_width = 17;
            this.player_height = 17;
            this.food_size = 15;
            this.loadGame();
        }

        public static System.Windows.Threading.DispatcherTimer Delay(int s, int ms, EventHandler e)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, s, ms);
            dispatcherTimer.Tick += new EventHandler(e);
            dispatcherTimer.Start();
            return dispatcherTimer;
        }

        private void win()
        {

        }

        private void lose()
        {
            this.game_message.Text = "PERDISTE";
        }

        public void pauseGame()
        {
            //
        }

        public Boolean isNewGameEnable()
        {
            return this.new_game;
        }

        public void newGame()
        {
            foreach(List<Object> data in this.j1.clearPlayer())
            {
                this.board.Children.Remove(data[0] as Ellipse);
            }
            this.board.Children.Remove(this.food.resetFood()[0] as Ellipse);
            this.game_message.Text = "";
            this.game_point.Text = "0";
            this.dispatcherTimer.Stop();
            this.dispatcherTimer = null;
            this.loadGame();
        }

        public void returnGame()
        {
            //
        }

        public void loadGame()
        {
            //Player
            this.j1 = new Player(this.limit, this.player_width, this.player_height);
            Object[] j1_data = j1.generatepartBody();
            Canvas.SetLeft(j1_data[0] as Ellipse, (double)j1_data[1]);
            Canvas.SetTop(j1_data[0] as Ellipse, (double)j1_data[2]);
            this.board.Children.Add(j1_data[0] as Ellipse);
            {
                Object[] bodyPart = j1.generatepartBody();
                Canvas.SetLeft(bodyPart[0] as Ellipse, (double)bodyPart[1]);
                Canvas.SetTop(bodyPart[0] as Ellipse, (double)bodyPart[2]);
                this.board.Children.Add(bodyPart[0] as Ellipse);
            }
            {
                Object[] bodyPart = j1.generatepartBody();
                Canvas.SetLeft(bodyPart[0] as Ellipse, (double)bodyPart[1]);
                Canvas.SetTop(bodyPart[0] as Ellipse, (double)bodyPart[2]);
                this.board.Children.Add(bodyPart[0] as Ellipse);
            }
            this.dispatcherTimer = Delay(0, this.j1.getSpeed(), this.timer_Tick);

            // Food
            this.food = new Food((int)this.board.Width, (int)this.board.Height, this.limit, this.food_size, this.player_width, this.player_height);
            double[] coord = this.food.generateCoord();
            while (this.j1.isBodyHere((int)coord[0], (int)coord[1])) {
                coord = this.food.generateCoord();
            }
            Object[] food_data = this.food.generate(coord);
            Canvas.SetLeft(food_data[0] as Ellipse, (double)food_data[1]);
            Canvas.SetTop(food_data[0] as Ellipse, (double)food_data[2]);
            this.board.Children.Add(food_data[0] as Ellipse);
        }

        public void playerMoveUp()
        {
            this.j1.moveUp();
        }

        public void playerMoveDown()
        {
            this.j1.moveDown();
        }

        public void playerMoveLeft()
        {
            this.j1.moveLeft();
        }

        public void playerMoveRight()
        {
            this.j1.moveRight();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!this.j1.getIsDead()) this.j1.move();
            else
            {
                this.new_game = true;
                dispatcherTimer.Stop();
                this.lose();
            }
            if (this.food.getIsEat())
            {
                // body
                Object[] bodyPart = j1.generatepartBody();
                Canvas.SetLeft(bodyPart[0] as Ellipse, (double)bodyPart[1]);
                Canvas.SetTop(bodyPart[0] as Ellipse, (double)bodyPart[2]);
                this.board.Children.Add(bodyPart[0] as Ellipse);
                // food
                double[] coord = this.food.generateCoord();
                while (this.j1.isBodyHere((int)coord[0], (int)coord[1]))
                {
                    coord = this.food.generateCoord();
                }
                Object[] food_data = this.food.generate(coord);
                Canvas.SetLeft(food_data[0] as Ellipse, (double)food_data[1]);
                Canvas.SetTop(food_data[0] as Ellipse, (double)food_data[2]);
                this.game_point.Text = this.food.printPoint();
                this.board.Children.Add(food_data[0] as Ellipse);
            }
            else
            {// Eat Food
                if ((((double)this.j1.getHead()[1] >= (this.food.getCoord()[0] - this.j1.getSize()[0])) && ((double)this.j1.getHead()[1] <= (this.food.getCoord()[0] + this.j1.getSize()[0]))) /* X */ && (((double)this.j1.getHead()[2] >= (this.food.getCoord()[1] - this.j1.getSize()[1])) && ((double)this.j1.getHead()[2] <= (this.food.getCoord()[1] + this.j1.getSize()[1]))) /* Y */)
                {
                    this.board.Children.Remove(this.food.eat());
                }
            }
        }
    }
}
