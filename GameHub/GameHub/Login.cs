using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GameHub
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public static User loggedUser = null;

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ok = false;
            foreach(User u in Form1.users)
            {
                if (u.email == textBox3.Text && u.password == textBox4.Text)
                {
                    ok = true;
                    loggedUser = u;
                }
            }
            if(ok)
            {
                MessageBox.Show("Succesful login!");
                this.Close();
            }
            else
                MessageBox.Show("Invalid fields");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Signup signup = new Signup();
            signup.ShowDialog();
            this.Close();
        }
    }
}
