using System;
using Ruletka.Models;

namespace Ruletka.Logic
{
    public class GameEngine
    {
        public Character Player { get; set; }
        public Character Death { get; set; }
        public Revolver Weapon { get; set; }
        public bool IsPlayerTurn { get; set; }

        private Random _random = new Random();

        public GameEngine(string playerName)
        {
            Player = new Character(playerName, 100);
            Death = new Character("Śmierć", 100);
            Weapon = new Revolver();
            IsPlayerTurn = true;

           
            Player.Inventory.Add(new Item("Brudny Bandaż (+30 HP)", 30, 0));      // Leczy 30 HP
            Player.Inventory.Add(new Item("Dobre Cygaro", -5, 15, false));       // Usuwa 5 hp, ale Śmierć to szanuje (+15 Sympatii)
            Player.Inventory.Add(new Item("Zardzewiała Piersiówka (+15 HP) ", 15, 5)); // Leczy 15 HP i daje +5 Sympatii
        }

        public void SpinCylinder()
        {
            Weapon.Reset();
            int bulletPosition = _random.Next(0, 6);
            Weapon.Chambers[bulletPosition] = true;
            Weapon.CurrentChamberIndex = _random.Next(0, 6);
        }

        public bool PullTrigger(Character target)
        {
            bool isBullet = Weapon.Chambers[Weapon.CurrentChamberIndex];
            if (isBullet)
            {
                target.Health -= 25;
                Weapon.Chambers[Weapon.CurrentChamberIndex] = false;
            }
            Weapon.CurrentChamberIndex = (Weapon.CurrentChamberIndex + 1) % 6;
            return isBullet;
        }

        public void NextTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
        }
    }
}