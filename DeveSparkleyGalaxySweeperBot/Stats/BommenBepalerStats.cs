﻿using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Stats
{
    public class BommenBepalerStats
    {
        public List<BommenBepalerStatsIteratie> Iteraties = new List<BommenBepalerStatsIteratie>();

        public TimeSpan TotalElapsed => new TimeSpan(Iteraties.Sum(t => t.Elapsed.Ticks));

        public void Log(ILogger logger)
        {
            logger.WriteLine($"Done in {TotalElapsed}. Total iterations: {Iteraties.Count}");

            foreach (var iteratie in Iteraties)
            {
                iteratie.Log(logger);
            }
        }

        public BommenBepalerStatsIteratieVondst GetVondstVoorVakje(Vakje vakje)
        {
            return Iteraties.SelectMany(t => t.Vondsten).FirstOrDefault(t => t.Vakje == vakje);
        }
    }
}
