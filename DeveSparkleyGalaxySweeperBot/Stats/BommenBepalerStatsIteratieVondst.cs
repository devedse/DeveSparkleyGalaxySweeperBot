using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System;

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

        public void Log(ILogger logger)
        {
            logger.WriteLine($"\t\t{VondstType} -> {Vakje}");
        }
    }
}
