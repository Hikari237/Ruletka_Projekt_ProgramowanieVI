using System;
using System.Collections.Generic;

namespace Ruletka.Models
{
    public class Revolver
    {
        
        public List<bool> Chambers { get; set; } = new List<bool>();
        public int CurrentChamberIndex { get; set; }

        public Revolver()
        {
         
            Reset();
        }

        public void Reset()
        {
            Chambers.Clear();
            for (int i = 0; i < 6; i++) Chambers.Add(false);
            CurrentChamberIndex = 0;
        }
    }
}