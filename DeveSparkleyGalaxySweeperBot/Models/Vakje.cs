using System;
using System.Collections.Generic;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class Vakje
    {
        public char Value { get; set; }
        public int Number => IsNumber ? int.Parse(Value.ToString()) : -1;
        public bool Revealed => Value != '.' && Value != '#';
        public bool IsBomb => Value == 'R' || Value == 'B';
        public bool IsNumber => Revealed == true && IsBomb == false;
        public List<Vakje> SurroundingVakjes { get; set; }

        public Vakje(char value)
        {
            Value = value;
            SurroundingVakjes = new List<Vakje>();
        }
    }
}
