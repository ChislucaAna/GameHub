using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameHub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static HashSet<User> users = new HashSet<User>();

        public void create_db()
        {
            StreamReader reader = new StreamReader("Users.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(' ');
                users.Add(new User(fields[0], fields[1], fields[2], fields[3], Int32.Parse(fields[4]),
                    Int32.Parse(fields[5]), Int32.Parse(fields[6]), Int32.Parse(fields[7])));
            }
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            show_highscore();
            create_db();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Show();
            //Atunci cand tu inchizi formul valoare variabilei statice ramane
            if (Login.loggedUser != null)
            {
                this.Text = Login.loggedUser.email;
                show_highscore();
            }
        }

        public void show_highscore()
        {
            if (Login.loggedUser != null)
            {
                label2.Text = "All time highscore:";
                label2.Text += Login.loggedUser.gethighscore().ToString();
            }
            else
            {
                label2.Text = "You are not logged in. Log into " +
                    "your account to see your highscore.";
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game2048 game = new Game2048();
            game.ShowDialog();
            this.Show();
            show_highscore();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Breakout breakout = new Breakout();
            breakout.ShowDialog();
            this.Show();
            show_highscore();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Snake snake = new Snake();
            snake.ShowDialog();
            this.Show();
            show_highscore();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Space space = new Space();
            space.ShowDialog();
            this.Show();
            show_highscore();
        }
    }
}
