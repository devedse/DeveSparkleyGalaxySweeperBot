using System.Collections.Generic;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class VakjeSetDeluxe
    {
        public int MinCountGuaranteedBombs { get; set; }
        public int MinCountGuaranteedNotBombs { get; set; }
        public List<Vakje> Vakjes { get; } = new List<Vakje>();

        public decimal BerekendeKansVoorDitSet
        {
            get
            {
                if (Vakjes.Count == MinCountGuaranteedBombs)
                {
                    return 1;
                }
                if (Vakjes.Count == MinCountGuaranteedNotBombs)
                {
                    return 0;
                }
                return MinCountGuaranteedBombs / (decimal)Vakjes.Count;
            }
        }

        public VakjeSetDeluxe(int minCountGuaranteedBombs, int minCountGuaranteedNotBombs, List<Vakje> vakjesInSet)
        {
            MinCountGuaranteedBombs = minCountGuaranteedBombs;
            MinCountGuaranteedNotBombs = minCountGuaranteedNotBombs;
            Vakjes = vakjesInSet;
        }
    }
}
