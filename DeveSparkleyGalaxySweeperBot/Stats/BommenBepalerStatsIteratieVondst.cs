using DeveSparkleyGalaxySweeperBot.Models;

namespace DeveSparkleyGalaxySweeperBot.Stats
{
    public class BommenBepalerStatsIteratieVondst
    {
        public Vakje Vakje { get; }
        public VondstType VondstType { get; }

        public BommenBepalerStatsIteratieVondst(Vakje vakje, VondstType vondstType)
        {
            Vakje = vakje;
            VondstType = vondstType;
        }
    }
}
