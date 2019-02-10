using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Stats
{
    public class BommenBepalerStatsIteratie
    {
        public List<BommenBepalerStatsIteratieVondst> Vondsten = new List<BommenBepalerStatsIteratieVondst>();

        private Stopwatch wElapsed = new Stopwatch();

        public TimeSpan Elapsed => wElapsed.Elapsed;

        public int IteratieNummer { get; set; }

        public BommenBepalerStatsIteratie()
        {
            wElapsed.Start();
        }

        public void FinishTimingIteration()
        {
            wElapsed.Stop();
        }

        public void Log(ILogger logger)
        {
            logger.WriteLine(string.Empty);
            logger.WriteLine($" === Iteratie {IteratieNummer} done in {Elapsed} ===");
            logger.WriteLine(string.Empty);
            foreach (VondstType vondstType in EnumHelper.GetEnumValues<VondstType>())
            {
                var vondstenOfThisType = Vondsten.Count(t => t.VondstType == vondstType);
                logger.WriteLine($"\t{vondstType}: {vondstenOfThisType}");
            }
            logger.WriteLine(string.Empty);

            foreach (var vondst in Vondsten)
            {
                vondst.Log(logger);
            }

            logger.WriteLine(string.Empty);
            logger.WriteLine(string.Empty);
        }
    }
}
