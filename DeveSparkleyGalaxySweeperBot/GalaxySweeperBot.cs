using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using DeveSparkleyGalaxySweeperBot.Stats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperBot
    {
        private readonly GalaxySweeperApiHelper _galaxySweeperApiHelper;
        private readonly BotConfig _botconfig;
        private readonly ILogger _logger;

        public GalaxySweeperBot(GalaxySweeperApiHelper galaxySweeperApiHelper, BotConfig botconfig, ILogger logger)
        {
            _galaxySweeperApiHelper = galaxySweeperApiHelper;
            _botconfig = botconfig;
            _logger = logger;
        }

        public void AcceptInvitesForAllGames(IEnumerable<GalaxySweeperGame> games)
        {
            var opponents = games.Select(t => t.opponent.challengeToken).Distinct().ToList();

            foreach (var opponent in opponents)
            {
                var gamesAgainstThisOpponent = games.Where(t => t.opponent.challengeToken == opponent).ToList();
                if (gamesAgainstThisOpponent.All(t => t.isFinished == true))
                {
                    _logger.WriteLine("Found an opponent without active games");
                    _galaxySweeperApiHelper.AcceptInvite(opponent);
                }
            }
        }

        public void DetermineBestMove(GalaxySweeperGame game, bool executeMove)
        {
            if (game.isFinished)
            {
                return;
            }

            if (!game.myTurn)
            {
                executeMove = false;
            }

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);





            //Nu is alle data goed
            var stats = BommenBepaler.BepaalBommenMulti(deVakjesArray, _botconfig);
            stats.Log(_logger);

            var unrevealedVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).Where(t => !t.Revealed).OrderByDescending(t => t.VakjeBerekeningen.BerekendeVakjeKans).ToList();
            var potentialBombs = unrevealedVakjes.Where(t => t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
            var guaranteedBombs = potentialBombs.Where(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom).ToList();

            List<Vakje> vakjesMetBomErnaast;
            if (_botconfig.AlwaysAvoidClickingOpenFields)
            {
                vakjesMetBomErnaast = unrevealedVakjes.Where(t => t.SurroundingVakjes.Any(z => z != null && z.IsBomb)).ToList();
            }
            else
            {
                vakjesMetBomErnaast = potentialBombs.Where(t => t.SurroundingVakjes.Any(z => z != null && z.IsBomb)).ToList();
            }

            GalaxyVisualizator.RenderToConsole(deVakjesArray, _logger);

            _logger.WriteLine(string.Empty);

            _logger.WriteLine("Best chance bombs (top 5):");
            foreach (var maybeBom in potentialBombs.Take(5))
            {
                _logger.Write($"\t{maybeBom.ToString()}");
                if (maybeBom.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)
                {
                    var vondst = stats.GetVondstVoorVakje(maybeBom);
                    if (vondst != null)
                    {
                        ConsoleColor c = ConsoleColor.DarkGreen;

                        if (vondst.Vakje.SurroundingVakjes.Any(t => stats.GetVondstVoorVakje(t)?.VondstType == VondstType.SetsBasedGuaranteedNoBomb))
                        {
                            c = ConsoleColor.DarkYellow;
                        }

                        if (vondst.VondstType == VondstType.SetsBasedGuaranteedBomb)
                        {
                            c = ConsoleColor.Magenta;
                        }


                        _logger.Write($"\t\t Gevonden in iteratie {vondst.Iteratie.IteratieNummer}. Type: {vondst.VondstType}", c);
                    }
                }
                _logger.WriteLine("");
            }
            _logger.WriteLine(string.Empty);

            _logger.WriteLine("Vakjes die op z'n minst een bom er naast hebben (dus sowieso geen 0 zijn):");
            foreach (var maybeBom in vakjesMetBomErnaast.Take(5))
            {
                _logger.WriteLine($"\t{maybeBom.ToString()}");
            }

            _logger.WriteLine(string.Empty);


            var deBom = guaranteedBombs.FirstOrDefault();
            if (deBom != null)
            {
                _logger.WriteLine($"Beste keuze (Guaranteed bom): {deBom}", ConsoleColor.DarkGreen);
                if (game.myTurn)
                {
                    _logger.WriteLine("Sweeping...", ConsoleColor.Red);
                    _galaxySweeperApiHelper.Sweep(game.id, deBom.X, deBom.Y);
                }
            }
            else
            {
                if (vakjesMetBomErnaast.Any())
                {
                    var hetVakjeWatWeGaanKlikken = vakjesMetBomErnaast.First();
                    _logger.WriteLine($"Beste keuze (Vakje met bom ernaast): {hetVakjeWatWeGaanKlikken}", ConsoleColor.DarkCyan);
                    if (game.myTurn)
                    {
                        _logger.WriteLine("Sweeping...", ConsoleColor.Red);
                        _galaxySweeperApiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                    }
                }
                else
                {
                    var hetVakjeWatWeGaanKlikken = potentialBombs.First();
                    _logger.WriteLine($"Beste keuze (Hoogste kans): {hetVakjeWatWeGaanKlikken}", ConsoleColor.DarkBlue);
                    if (game.myTurn)
                    {
                        _logger.WriteLine("Sweeping...", ConsoleColor.Red);
                        _galaxySweeperApiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                    }
                }
            }

            _logger.WriteLine(string.Empty);
            _logger.WriteLine(string.Empty);

        }
    }
}
