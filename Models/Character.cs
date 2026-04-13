using System.Collections.Generic;

namespace Ruletka.Models
{
    public class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Sympathy { get; set; }
        public int Deaths { get; set; }
        public int Wins { get; set; } 
        public List<Item> Inventory { get; set; }

        public Character(string name, int health, int sympathy = 0)
        {
            Name = name;
            Health = health;
            Sympathy = sympathy;
            Deaths = 0;
            Wins = 0; 
            Inventory = new List<Item>();
        }

        public override string ToString() => Name;
    }
}