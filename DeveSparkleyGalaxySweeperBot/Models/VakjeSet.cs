using System.Collections.Generic;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class VakjeSet
    {
        public int CountVanBommenDieErMoetenZijn { get; }
        public List<Vakje> Vakjes { get; } = new List<Vakje>();

        public float BerekendeKansVoorDitSet => CountVanBommenDieErMoetenZijn / (float)Vakjes.Count;

        public VakjeSet(int bomCount, List<Vakje> vakjesInSet)
        {
            CountVanBommenDieErMoetenZijn = bomCount;
            Vakjes = vakjesInSet;
        }
    }
}
