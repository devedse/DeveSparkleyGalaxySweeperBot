using System;
using System.Collections.Generic;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    class Vakje
    {
        public string Value { get; set; }
        public bool Revealed => Value != "." && Value != "#";
        public bool IsBomb => Value == "R" || Value == "B";
        public int Number;
        public List<Vakje> SurroundingVakjes { get; set; }
    }
}
