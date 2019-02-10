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
                switch (BerekendVakjeType)
                {
                    case BerekendVakjeType.GuaranteedBom:
                        return 1;
                    case BerekendVakjeType.GuaranteedNoBom:
                        return 0;
                    case BerekendVakjeType.Unknwon:
                    default:
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
}
