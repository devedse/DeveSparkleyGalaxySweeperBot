using System.Collections.Generic;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class VakjeBerekeningen
    {
        public List<VakjeSet> Sets { get; set; } = new List<VakjeSet>();
        public BerekendVakjeType BerekendVakjeType { get; set; }

        public float BerekendeVakjeKans => Sets.Min(t => t.BerekendeKansVoorDitSet);
    }
}
