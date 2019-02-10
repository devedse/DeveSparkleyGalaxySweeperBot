using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System;

namespace DeveSparkleyGalaxySweeperBot.Stats
{
    public class BommenBepalerStatsIteratieVondst
    {
        public BommenBepalerStatsIteratie Iteratie { get; }
        public Vakje Vakje { get; }
        public VondstType VondstType { get; }

        public BommenBepalerStatsIteratieVondst(BommenBepalerStatsIteratie iteratie, Vakje vakje, VondstType vondstType)
        {
            Iteratie = iteratie;
            Vakje = vakje;
            VondstType = vondstType;
        }

        public void Log(ILogger logger)
        {
            logger.WriteLine($"\t\t{VondstType} -> {Vakje}");
        }
    }
}
