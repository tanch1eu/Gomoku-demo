using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ConsoleGames_Demo
{
    public class Player
    {
        private string name;

        public string Name { get => name; set => name = value; }

        public Player(string name)
        {
            this.Name = name;
        }
    }
}
