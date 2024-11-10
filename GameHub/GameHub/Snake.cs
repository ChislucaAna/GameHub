using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace GameHub
{
    public partial class Snake : Form
    {
        public Snake()
        {
            InitializeComponent();
        }

        Body[] snake = new Body[200];
        Rock[] obstacles = new Rock[50];
        Food[] foods = new Food[20];
        int nr_of_obstacles;
        int nr_of_food;
        int obstacle_size;
        float goalx = 0, goaly = 0; //the point that the snake aims to reach in the current moment
        bool selected_difficulty = false;
        int snake_length = 0; //the current length of the snake
        int score = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            start_game();
        }

        public void clear_game_data()
        {
            Array.Clear(snake, 0, snake.Length);
            Array.Clear(foods, 0, foods.Length);
            Array.Clear(obstacles, 0, obstacles.Length); 
        }

        public void start_game()
        {
            clear_game_data();
            if (selected_difficulty)
            {
                get_parameters(); //depending on the difficulty chosen
                generate_obstacles();
                generate_snake_head();
                generate_food();

                timer1.Enabled = true;
                timer1.Start();
                pictureBox1.Refresh();
            }
            else
                MessageBox.Show("Please select difficulty before attempting a new game");
        }

        public void get_parameters()
        {
            if (comboBox1.SelectedIndex == 0) //easy difficulty
            {
                nr_of_obstacles = 5; nr_of_food = 7; obstacle_size = 30;
                timer1.Interval = 500; //snake's speed
            }
            else if (comboBox1.SelectedIndex == 1) //medium difficulty
            {
                nr_of_obstacles = 10; nr_of_food = 5; obstacle_size = 35;
                timer1.Interval = 1000;
            }
            else if (comboBox1.SelectedIndex == 1) //hard difficulty
            {
                nr_of_obstacles = 15; nr_of_food = 3; obstacle_size = 40;
                timer1.Interval = 1000;
            }
        }

        public void generate_obstacles()
        {
            Random rnd = new Random();
            for (int i = 1; i <= nr_of_obstacles; i++)
            {
                int x = rnd.Next(20, this.pictureBox1.Width - 20);
                int y = rnd.Next(20, this.pictureBox1.Height - 20);
                obstacles[i] = new Rock(x, y);
                Thread.Sleep(10);
            }
        }

        public void generate_snake_head()
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = rnd.Next(20, this.pictureBox1.Width - 20);
                y = rnd.Next(20, this.pictureBox1.Height - 20);
                Thread.Sleep(50);
            }
            while (!place_safe(x, y));
            snake[0] = new Body(x, y);
            goalx = snake[0].x + 5;
            goaly = snake[0].y + 5;
        }

        public bool place_safe(float x, float y)//verify obstacle existance at give coords
        {
            Rectangle aim = new Rectangle(Convert.ToInt32(x),Convert.ToInt32( y), 5, 5);
            foreach (Rock t in obstacles)
            {
                if (t != null)
                {
                    Rectangle obstacle = new Rectangle(t.x, t.y, obstacle_size, obstacle_size);
                    if (aim.IntersectsWith(obstacle))
                        return false;
                }
            }
            return true;
        }

        public void generate_food()
        {

            Random rnd = new Random();
            int x, y;
            for (int i = 1; i <= nr_of_food; i++)
            {
                do
                {
                    x = rnd.Next(20, this.pictureBox1.Width - 20);
                    y = rnd.Next(20, this.pictureBox1.Height - 20);
                    Thread.Sleep(50);
                }
                while (!place_safe(x, y));
                foods[i] = new Food(x, y);
            }
        }

        public void Paint_food(PaintEventArgs e)
        {
            SolidBrush red = new SolidBrush(Color.Red);
            foreach (Food f in foods)
            {
                if (f != null)
                {
                    Rectangle shape = new Rectangle(f.x, f.y, Food.size, Food.size);
                    e.Graphics.FillEllipse(red, shape);
                }
            }
        }

        public void Paint_snake(PaintEventArgs e)
        {
            SolidBrush darkgreen = new SolidBrush(Color.DarkGreen);
            foreach (Body b in snake)
            {
                if (b != null)
                {
                    Rectangle shape = new Rectangle(Convert.ToInt32(b.x), Convert.ToInt32(b.y), Body.size, Body.size);
                    e.Graphics.FillEllipse(darkgreen, shape);
                }
            }
        }

        public void Paint_obstacles(PaintEventArgs e)
        {
            SolidBrush darkgrey = new SolidBrush(Color.DarkGray);
            foreach (Rock r in obstacles)
            {
                if (r != null)
                {
                    Rectangle shape = new Rectangle(r.x, r.y, obstacle_size, obstacle_size);
                    e.Graphics.FillRectangle(darkgrey, shape);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Paint_food(e);
            Paint_snake(e);
            Paint_obstacles(e);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            goalx = e.X;
            goaly = e.Y;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            modify_position();
            if (game_lost())
            {
                stop_game();
                MessageBox.Show("You lost");
                is_new_highscore();
            }
            if (snake_intersects_food())
            {
                add_new_cell();
                if (no_food_left_on_map())
                    generate_food();
            }
            pictureBox1.Refresh();
        }

        public bool game_lost()
        {
            if (!place_safe(snake[0].x, snake[0].y))
                return true;
            else if (snake_hit_own_body())
                return true;
            else if (snake_left_map())
                return true;
            return false;
        }

        public bool snake_hit_own_body()
        {
            Rectangle head = new Rectangle(Convert.ToInt32(snake[0].x), Convert.ToInt32(snake[0].y), Body.size, Body.size);
            for (int i = 3; i <= snake_length; i++) //the first few body parts dont count as a loss 
            {
                if (snake[i] != null)
                {
                    Rectangle body = new Rectangle(Convert.ToInt32(snake[i].x), Convert.ToInt32(snake[i].y), Body.size, Body.size);
                    if (head.IntersectsWith(body))
                        return true;
                }
            }
            return false;
        }

        bool snake_left_map()
        {
            if (snake[0].x < 0 || snake[0].x > pictureBox1.Width ||
                snake[0].y < 0 || snake[0].y > pictureBox1.Height)
                return true;
            return false;
        }

        public void stop_game()
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        public void add_new_cell()
        {
            //vezi daca e ok
            Body aux = new Body(snake[snake_length].x - Body.size, snake[snake_length].y - Body.size);
            snake[++snake_length] = aux;
        }

        public bool snake_intersects_food()
        {
            Rectangle head = new Rectangle(Convert.ToInt32(snake[0].x), Convert.ToInt32(snake[0].y), Body.size, Body.size);
            foreach (Food f in foods)
            {
                if (f != null)
                {
                    Rectangle food = new Rectangle(f.x, f.y, Food.size, Food.size);
                    if (food.IntersectsWith(head))
                    {
                        f.x = -100;
                        f.y = -100;
                        modify_score(1);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool no_food_left_on_map()
        {
            foreach (Food f in foods)
            {
                if (f != null)
                {
                    if (f.x != -100 && f.y != -100)
                        return false;
                }
            }
            return true;
        }

        public void modify_position()
        {
            for (int i = snake_length; i >= 1; i--)
            {
                if (snake[i] != null)
                {
                    snake[i].x = snake[i - 1].x; snake[i].y = snake[i - 1].y;
                }
            }
            find_next_position();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_difficulty = true;
        }

        public void find_next_position() //find y by using the slope of the current line of movement
        {
            //1 aflii panta dreptei pe care trebuie sa se miste sarpele pt a ajunge la punctul-destinatie;
            //m=(y-y0)/(x-x0)
            //eu stiu ca x ul si y ul vreau sa mi se modifice in total cu 5px
            if (goalx == snake[0].x) //m ar veni infinit
            {
                if (snake[0].y < goaly)
                    snake[0].y+=5;
                else
                    if (snake[0].y > goaly)
                        snake[0].y-=5;
            }
            else
            if (goaly == snake[0].y)
            {
                if (snake[0].x < goalx)
                    snake[0].x += 5;
                else
                    if (snake[0].x > goalx)
                    snake[0].x -= 5;
            }
            else
            {
                float m = (snake[0].y - goaly) / (snake[0].x - goalx);
                //modfx+modfy=5 vreau ca pozitia reliativa a sarpelui pe harta sa se modifice cu 5
                //(snake[0].y+dify) / (snake[0].x+difx) =m vreau sa ma misc pe dreapta cu panta m
                float modfy = (5 * m) / (m + 1);
                float modfx = 5 / (m + 1);
                    if (snake[0].x < goalx)
                        snake[0].x += Math.Abs(modfx);
                    else
                        snake[0].x -= Math.Abs(modfx);
                    if (snake[0].y < goaly)
                        snake[0].y += Math.Abs(modfy);
                    else
                        snake[0].y -= Math.Abs(modfy);
                //MessageBox.Show(snake[0].x.ToString());
                //MessageBox.Show(snake[0].y.ToString());

            }
        }

        public void is_new_highscore()
        {
            if (Login.loggedUser != null)
            {
                if (score > Login.loggedUser.max_score_2048)
                {
                    MessageBox.Show("You managed a new highscore! Congrats!");
                    Login.loggedUser.max_score_2048 = score;
                }
            }
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            if (Login.loggedUser != null)
                label4.Text += Login.loggedUser.max_score_2048.ToString();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        public void modify_score(int value)
        {
            score += value;
            label3.Text = "SCORE:";
            label3.Text += score.ToString();
        }
    }
}
