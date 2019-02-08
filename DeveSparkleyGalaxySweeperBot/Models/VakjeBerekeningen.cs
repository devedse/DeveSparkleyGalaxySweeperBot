using System.Collections.Generic;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class VakjeBerekeningen
    {
        public List<VakjeSet> Sets { get; set; } = new List<VakjeSet>();
        public VakjeSet TheBigUnrevealedSet { get; set; }

        public BerekendVakjeType BerekendVakjeType { get; set; }

        public decimal BerekendeVakjeKans
        {
            get
            {
                if (Sets.Count == 0)
                {
                    if (TheBigUnrevealedSet == null)
                    {
                        return 0;
                    }
                    return TheBigUnrevealedSet.BerekendeKansVoorDitSet;
                }

                return Sets.Max(t => t.BerekendeKansVoorDitSet);
            }
        }
    }
}
