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
using System.Windows.Forms.VisualStyles;

namespace GameHub
{
    public partial class Game2048 : Form
    {
        public Game2048()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        int score = 0;
        static int[,] tilevalues = new int[4,4];
        Dictionary<int, Color> numberColors = new Dictionary<int, Color>
        {
            { 2, Color.LightBlue },
            { 4, Color.LightGreen },
            { 8, Color.LightCoral },
            { 16, Color.LightPink },
            { 21, Color.LightGoldenrodYellow }, // Arbitrary color choice for 21
            { 32, Color.LightSalmon },
            { 64, Color.LightSeaGreen },
            { 128, Color.LightSkyBlue },
            { 256, Color.LightSteelBlue },
            { 512, Color.LightSlateGray },
            { 1024, Color.LightYellow },
            { 2048, Color.Gold }
        };

        public void start_game() //generate the value 2 on two random tiles
        {
            Random rnd = new Random();
            for (int cnt = 1; cnt <= 2; cnt++)
                generate_new_value();
            update_visuals();
        }

        public void generate_new_value() //assign 2 value to a random empty tile
        {
            Random rnd;
            int i=0, j=0;
            try
            {
                do
                {
                    rnd = new Random();
                    i = rnd.Next(1, 4);
                    j = rnd.Next(1, 4);
                    Thread.Sleep(50);
                }
                while (tilevalues[i, j] != 0);
                tilevalues[i, j] = 2;
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
                MessageBox.Show(i.ToString());
                MessageBox.Show(j.ToString());
            }
        }

        public void update_visuals()
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(PictureBox))
                {
                    c.Refresh();
                }
            }
        }

        private void Game2048_Load(object sender, EventArgs e)
        {
            start_game();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Array.Clear(tilevalues, 0, tilevalues.Length);
            start_game();
        }

        public static void Swap(int row1, int col1, int row2, int col2)
        {
            if (row1 < 0 || row1 >= tilevalues.GetLength(0) || col1 < 0 || col1 >= tilevalues.GetLength(1) ||
                row2 < 0 || row2 >= tilevalues.GetLength(0) || col2 < 0 || col2 >= tilevalues.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("Indices must be within the bounds of the array.");
            }
            (tilevalues[row1, col1], tilevalues[row2, col2]) = (tilevalues[row2, col2], tilevalues[row1, col1]);
        }

        private void Game2048_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyCode == Keys.W)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        while (tilevalues[i, j] == 0 && tilevalues[i + 1, j] != 0)
                        {
                            Swap(i, j, i + 1, j);
                            update_visuals();
                            i--;
                            if (i == 0) break;
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.A)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 2; j++)
                    {
                            while (tilevalues[i, j] == 0 && tilevalues[i, j + 1] != 0)
                            {
                                Swap(i, j, i, j + 1);
                                update_visuals();
                                j--;
                                if (j == 0) break;
                            }
                    }
                }
            }
            else if (e.KeyCode == Keys.S)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int i = 2; i >= 1; i--)
                    {
                         while(tilevalues[i, j] != 0 && tilevalues[i + 1, j] == 0)
                         {
                             Swap(i, j, i + 1, j);
                             update_visuals();
                             i++;
                             if(i==3) break;
                         }
                    }
                }
            }
            else if (e.KeyCode == Keys.D)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 2; j >=1; j--)
                    {
                        while (tilevalues[i, j] != 0 && tilevalues[i, j + 1] == 0)
                        {
                            Swap(i, j, i, j + 1);
                            update_visuals();
                            j++;
                            if (j == 3) break;
                        }
                    }
                }
            }
        }

        private void tile_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 12);
            Brush black = new SolidBrush(Color.Black);
            //in each picturebox tag we have its position in the 
            //matrix (i,j coords)
            string position = pictureBox.Tag.ToString();
            string[] coords = position.Split(' ');
            int value = tilevalues[Int32.Parse(coords[0]), Int32.Parse(coords[1])];
            if (value != 0)
            {
                Rectangle rect = new Rectangle(0, 0, 100, 100);
                g.FillRectangle(new SolidBrush(numberColors[value]),rect);
                g.DrawString(value.ToString(), font, black, 10, 10);
            }
        }
    }
}
