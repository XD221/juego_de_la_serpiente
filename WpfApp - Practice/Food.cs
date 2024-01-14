using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp___Practice
{
    internal class Food
    {
        private int board_width;
        private int board_height;
        private int width;
        private int height;
        private int width_limit;
        private int height_limit;
        private int point;
        private int width_space;
        private int height_space;
        private double border;
        private Object[] food_data;

        private int generate_value_autoConfig = 0;
        public Food(int board_width, int board_height, double border, int size, int width_space, int height_space)
        {
            this.board_width = board_width;
            this.board_height = board_height;
            this.width = size;
            this.height = size;
            this.border = border;
            this.width_limit = (int)((this.board_width - this.width) - (this.border));
            this.height_limit = (int)((this.board_height - this.height) - (this.border));
            this.point = 0;
            this.width_space = width_space;
            this.height_space = height_space;
        }

        private int generate_process(int space, int limit)
        {
            int last_generator_value = this.generate_value_autoConfig;//Dont have a used, It's necesary use this with that actually autoConfig; // resolve
            double multiplier_limit = limit / space;
            if (this.generate_value_autoConfig == 0 || (this.generate_value_autoConfig >= multiplier_limit)) this.generate_value_autoConfig = (int)Math.Floor(multiplier_limit);
            int result = 0;
            Random range = new Random();
            int number_1 = range.Next(1, this.generate_value_autoConfig);
            int number_2 = range.Next(1, this.generate_value_autoConfig);
            while(number_1 == number_2) number_2 = range.Next(1, this.generate_value_autoConfig);
            if(number_1 > number_2) result = space * range.Next(number_2, number_1);
            else result = space * range.Next(number_1, number_2);
            //result = space * range.Next(1, this.generate_value_autoConfig);
            result = result < this.border ? (int)this.border + 1 : result;
            return result;
        }

        public double[] generateCoord()
        {
            Random range = new Random();
            double x = this.generate_process(this.width_space, this.width_limit);//range.Next((int)this.border,this.width_limit);
            double y = this.generate_process(this.height_space, this.height_limit);//range.Next((int)this.border,this.height_limit);
            return new double[] { x, y };
        }

        public Object[] generate(double[] coord)
        {
            Ellipse food = new Ellipse();
            food.Width = this.width;
            food.Height = this.height;
            food.Fill = System.Windows.Media.Brushes.Blue;
            this.food_data = new Object[3] { food, coord[0], coord[1] }; ;
            return this.food_data;
        }

        public Ellipse eat()
        {
            Ellipse result = this.food_data[0] as Ellipse;
            this.food_data = null;
            this.point++;
            return result;
        }

        public String printPoint()
        {
            return this.point.ToString();
        }

        public int[] getCoord()
        {
            return new int[2] { (int)(double)this.food_data[1], (int)(double)this.food_data[2] };
        }

        public Boolean getIsEat()
        {
            return this.food_data is null ? true : false;
        }

        public List<Object> resetFood()
        {
            List<Object> result = new List<object>(this.food_data);
            this.food_data = null;
            this.point = 0;
            return result;
        }
    }
}