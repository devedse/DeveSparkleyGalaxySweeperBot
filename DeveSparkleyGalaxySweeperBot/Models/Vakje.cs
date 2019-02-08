using System.Collections.Generic;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class Vakje
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Value { get; set; }
        public int Number => IsNumber ? int.Parse(Value.ToString()) : -1;
        public bool Revealed => Value != '.' && Value != '#';
        public bool IsBomb => Value == 'R' || Value == 'B';
        public bool IsNumber => Revealed == true && IsBomb == false;
        public List<Vakje> SurroundingVakjes { get; set; }
        public VakjeBerekeningen VakjeBerekeningen { get; set; }

        public Vakje(char value, int x, int y)
        {
            Value = value;
            SurroundingVakjes = new List<Vakje>();
            VakjeBerekeningen = new VakjeBerekeningen();
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{Value} -> {VakjeBerekeningen.BerekendVakjeType}";
        }
    }
}
