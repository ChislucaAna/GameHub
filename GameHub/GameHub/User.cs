using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameHub
{
    public class User
    {
        public string last_name;
        public string first_name;
        public string email;
        public string password;
        public int max_score_snake;
        public int max_score_2048;
        public int max_score_breakout;
        public int max_score_space;

        //for form created user
        public User(string last_name, string first_name, string email, string password)
        {
            this.last_name = last_name;
            this.first_name = first_name;
            this.email = email;
            this.password = password;
        }

        //for existing user
        public User(string last_name, string first_name, string email, string password, int max_score_snake, int max_score_2048, int max_score_breakout, int max_score_space)
        {
            this.last_name = last_name;
            this.first_name = first_name;
            this.email = email;
            this.password = password;
            this.max_score_snake = max_score_snake;
            this.max_score_2048 = max_score_2048;
            this.max_score_breakout = max_score_breakout;
            this.max_score_space = max_score_space;
        }

        public static void validate(string last_name, string first_name
            ,string email,string password,string confirmpass)
        {
            if(last_name == null || first_name==null || email==null ||password
                ==null || confirmpass==null) 
                throw new Exception("Null Fields are not allowed");
            if(password!=confirmpass)
                throw new Exception("Paawords dont match");
            if (!email.Contains("@") || !email.Contains("."))
                throw new Exception("Email isnt valid");
        }

        public int gethighscore()
        {
            int max = this.max_score_2048;
            if (max_score_breakout > max)
               max = max_score_breakout;
            if (max_score_snake > max)
              max = max_score_snake;
            if (max_score_space > max)
              max = max_score_space;
            return max;
        }
    }
}
