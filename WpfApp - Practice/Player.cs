using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp___Practice
{
    internal class Player
    {
        public static int lvl;
        public static double px = 380;
        public static double py = 200;
        private int partBody;
        private List<List<Object>> body;
        private Boolean xy; // true = Positive / false = Negative 
        private Boolean z;  // true = Horizontal / false = Vertical
        private Boolean isMove;
        private double border;
        private int height;
        private int width;
        private int speed; // time per seconds
        private Boolean isDead;
        private Boolean limit;
        private List<Object> head;
        public Player(double border, int player_width, int player_height) 
        {
            this.limit = false;
            this.isDead = false;
            this.isMove = false;
            this.xy = false;
            this.z = false;
            this.border = border;
            this.height = player_height;
            this.width = player_width;
            this.loadPlayer();
        }

        private Boolean bodyNoFood()  // Restriction to across the body
        {
            List<Object> head = this.body[0];
            List<List<Object>> body = this.body.GetRange(1,this.body.Count() - 1);
            List<List<Object>> result = body.FindAll(element => (((int)(double)element[1] == (int)(double)head[1]) && ((int)(double)element[2] == (int)(double)head[2]))); // Find 
            return (result.Count() > 0) ? true : false; // false = the head not across the body / true = the head across the body

        }

        public Boolean isBodyHere(int x, int y)
        {
            Boolean result = false;
            List<List<Object>> result_data = this.body.FindAll(element => ((((int)(double)element[1] >= x - this.width) && ((int)(double)element[1] <= x + this.width)) && (((int)(double)element[2] >= y - this.height) && ((int)(double)element[2] <= y + this.height)))); // Find 
            if (result_data.Count() > 0) result = true;
            return result;
        }

        public int getSpeed()
        {
            return speed;
        }

        public void loadPlayer()
        {
            switch (lvl)
            {
                case 0:
                    this.speed = 300;
                    break;
                case 1:
                    this.speed = 180;
                    break;
                case 2:
                    this.speed = 120;
                    break;
            }
            this.partBody = 0;
            this.body = new List<List<Object>>();
        }

        public Object[] generatepartBody()
        {
            this.partBody += 1;
            Ellipse newBodyPart = new Ellipse();
            List<Object> data = new List<Object>();
            // Size
            newBodyPart.Height = this.height;
            newBodyPart.Width = this.width;
            double x = px;
            double y = py;
            if (this.body.Count == 0)
            {
                newBodyPart.Fill = System.Windows.Media.Brushes.Green;
                Canvas.SetZIndex(newBodyPart, 999);
                data.Add(newBodyPart); // Physical
                data.Add(x); // Coord X
                data.Add(y); // Coord Y
                this.head = data;
            }
            else
            {
                newBodyPart.Fill = System.Windows.Media.Brushes.Red;
                Canvas.SetZIndex(newBodyPart, 998);
                data.Add(newBodyPart); // Physical
                x = ((double)this.body[this.body.Count - 1][1]) + (
                    this.z ?
                    (xy ? -(this.width) : +(this.width)) :
                    0
                    );
                x = ((x - this.border) - this.width) <= 0 ? 0 : x;
                y = ((double)this.body[this.body.Count - 1][2]) + (
                    z ? 
                    0:
                    (xy ? -(this.height) : +(this.height))
                    );
                y = ((y - this.border) - this.height) <= 0 ? 0 : y;
                data.Add(x); // Coord X
                data.Add(y); // Coord Y
            }
            this.body.Add(data);
            return new Object[3] { newBodyPart, x, y };
        }

        public void move()
        {
            if(this.body.Count() > 0)
            {
                List<Object> head = this.body[0];
                List<Object> tail = this.body[this.body.Count() - 1];
                Boolean limit_x = false;
                Boolean limit_y = false;
                // Move head
                if (this.z)
                {
                    double width = (double)head[1] + (this.xy ? +(this.width) : -(this.width));
                    double width_limit = ((px * 2) - this.width) - (this.border);
                    if ((width - this.border) <= 0)
                    {
                        limit_x = true;
                        if (!this.limit)
                        {
                            head[1] = this.border;  // X
                            Canvas.SetLeft(head[0] as Ellipse, (double)head[1]);
                        }
                    }
                    else if(width >= width_limit)
                    {
                        limit_x = true;
                        if (!this.limit)
                        {
                            head[1] = width_limit;  // X
                            Canvas.SetLeft(head[0] as Ellipse, (double)head[1]);
                        }
                    }
                    else if(!((width - this.border) <= 0) && !(width >= width_limit))
                    {
                        head[1] = width;  // X
                        Canvas.SetLeft(head[0] as Ellipse, (double)head[1]);
                    }
                }
                else
                {
                    double height = (double)head[2] + (this.xy ? +(this.height) : -(this.height));
                    double height_limit = ((py * 2) - this.height) - (this.border);
                    if ((height - this.border) <= 0)
                    {
                        limit_y = true;
                        if (!this.limit)
                        {
                            head[2] = this.border; // Y
                        Canvas.SetTop(head[0] as Ellipse, (double)head[2]);
                        }
                    }
                    else if (height >= height_limit)
                    {
                        limit_y = true;
                        if (!this.limit)
                        {
                            head[2] = height_limit;  // Y
                            Canvas.SetTop(head[0] as Ellipse, (double)head[2]);
                        }
                    }
                    else if(!((height - this.border) <= 0) && !(height >= height_limit))
                    {
                        head[2] = height; // Y
                        Canvas.SetTop(head[0] as Ellipse, (double)head[2]);
                    }
                }
                // isDead
                if (this.limit && (limit_x || limit_y)) this.isDead = true;

                // change limit to false
                else if (this.limit && (!limit_x && !limit_y)) this.limit = false;

                // Move tail
                if (this.body.Count() > 1){
                    if (!this.limit)
                    {
                        if (this.z)
                        {
                            double width = (double)head[1] + (this.xy ? -(this.width) : +(this.width));
                            double height = (double)head[2];
                            tail[1] = width;  // X
                            tail[2] = height; // Y
                            Canvas.SetTop(tail[0] as Ellipse, (double)tail[2]);
                            Canvas.SetLeft(tail[0] as Ellipse, (double)tail[1]);
                        }
                        else
                        {
                            double width = (double)head[1];
                            double height = (double)head[2] + (this.xy ? -(this.height) : +(this.height));
                            tail[1] = width;  // X
                            tail[2] = height; // Y
                            Canvas.SetTop(tail[0] as Ellipse, (double)tail[2]);
                            Canvas.SetLeft(tail[0] as Ellipse, (double)tail[1]);
                        }
                        this.body.Remove(tail);
                        this.body.Insert(1, tail);
                    }
                }

                // change limit to true
                if (!this.limit && (limit_x || limit_y)) this.limit = true;
                // Eat body
                if (this.bodyNoFood()) this.isDead = true;

                // enable move
                isMove = false;
            }
        }
        //public void move()
        //{
        //    // z = true  | xy true = right / false = left
        //    // z = false | xy true = bottom / false = top
        //    foreach(List<Object> obj in this.body)
        //    {
        //        double width = (double)obj[1] + (this.xy ? +(this.width) : -(this.width));
        //        double height = (double)obj[2] + (this.xy ? +(this.height) : -(this.height));
        //        if (this.z)
        //        {
        //            obj[1] = width;  // X
        //            Canvas.SetLeft(obj[0] as Ellipse, (double)obj[1]);
        //        }
        //        else
        //        {
        //            obj[2] = height; // Y
        //            Canvas.SetTop(obj[0] as Ellipse, (double)obj[2]);
        //        }
        //    }
        //}

        public void moveUp()
        {
            if (this.z && !this.isMove)
            {
                this.z = false;
                this.xy = false;
                this.isMove = true;
            }
        }
        public void moveLeft()
        {
            if (!this.z && !this.isMove)
            {
                this.z = true;
                this.xy = false;
                this.isMove = true;
            }
        }
        public void moveRight()
        {
            if(!this.z && !this.isMove)
            {
                this.z = true;
                this.xy = true;
                this.isMove = true;
            }
        }
        public void moveDown()
        {
            if (this.z && !this.isMove)
            {
                this.z = false;
                this.xy = true;
                this.isMove = true;
            }
        }

        public Boolean getIsDead()
        {
            return this.isDead;
        }

        public List<Object> getHead()
        {
            return this.head;
        }

        public List<List<Object>> clearPlayer()
        {
            List<List<Object>> result = new List<List<Object>>(this.body);
            this.body = null;
            return result;
        }

        public int[] getSize()
        {
            return new int[2] { this.width, this.height };
        }
    }
}
