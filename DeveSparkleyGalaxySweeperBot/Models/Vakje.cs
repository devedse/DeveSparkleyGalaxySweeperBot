using System;
using System.Collections.Generic;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    class Vakje
    {
        public string Value { get; set; }
        public int Number => IsNumber ? int.Parse(Value) : -1;
        public bool Revealed => Value != "." && Value != "#";
        public bool IsBomb => Value == "R" || Value == "B";
        public bool IsNumber => Revealed == true && IsBomb == false;
        public List<Vakje> SurroundingVakjes { get; set; }
    }
}
