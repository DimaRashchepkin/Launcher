using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    public class Player
    {
        private int id = 0;
        public int Id { get { return id; } }

        private string name = "";
        public string Name { get { return name; } }

        private string password = "";
        public string Password { get { return password; } }

        public Player(int id, string name, string password)
        {
            this.id = id;
            this.name = name;
            this.password = password;
        }

        public Player() { }
    }
}
