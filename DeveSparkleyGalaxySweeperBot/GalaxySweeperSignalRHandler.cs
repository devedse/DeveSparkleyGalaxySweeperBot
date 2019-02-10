using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperSignalRHandler
    {
        private HubConnection connection;
        private readonly GalaxySweeperApiHelper apiHelper;
        private readonly ILogger logger;

        public GalaxySweeperSignalRHandler(string bearerToken, GalaxySweeperApiHelper apiHelper, ILogger logger)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://galaxysweeper.com/hubs/minesweeper", t => t.AccessTokenProvider = () => Task.FromResult(bearerToken))
                .Build();
            this.apiHelper = apiHelper;
            this.logger = logger;
        }

        public void HandleGameSweeperGameMessage(GalaxySweeperGame game)
        {
            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);





            //Nu is alle data goed
            var stats = BommenBepaler.BepaalBommenMulti(deVakjesArray);
            stats.Log(logger);


            var potentialBombs = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).Where(t => !t.Revealed).Where(t => t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).OrderByDescending(t => t.VakjeBerekeningen.BerekendeVakjeKans).ToList();
            var guaranteedBombs = potentialBombs.Where(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom).ToList();
            var vakjesMetBomErnaast = potentialBombs.Where(t => t.SurroundingVakjes.Any(z => z != null && z.IsBomb)).ToList();

            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            logger.WriteLine(string.Empty);

            logger.WriteLine("Best chance bombs (top 5):");
            foreach (var maybeBom in potentialBombs.Take(5))
            {
                var stringToLog = $"\t{maybeBom.ToString()}";
                if (maybeBom.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)
                {
                    stringToLog += stats.CreateLogStringVanWaarDezeGevondenIs(maybeBom);
                }
                logger.WriteLine(stringToLog);
            }
            logger.WriteLine(string.Empty);

            logger.WriteLine("Vakjes die op z'n minst een bom er naast hebben (dus sowieso geen 0 zijn):");
            foreach (var maybeBom in vakjesMetBomErnaast.Take(5))
            {
                logger.WriteLine($"\t{maybeBom.ToString()}");
            }

            logger.WriteLine(string.Empty);


            var deBom = guaranteedBombs.FirstOrDefault();
            if (deBom != null)
            {
                logger.WriteLine($"Beste keuze (Guaranteed bom): {deBom}", ConsoleColor.DarkGreen);
                if (game.myTurn)
                {
                    apiHelper.Sweep(game.id, deBom.X, deBom.Y);
                }
            }
            else
            {
                if (vakjesMetBomErnaast.Any())
                {
                    var hetVakjeWatWeGaanKlikken = vakjesMetBomErnaast.First();
                    logger.WriteLine($"Beste keuze (Vakje met bom ernaast): {hetVakjeWatWeGaanKlikken}", ConsoleColor.DarkCyan);
                    if (game.myTurn)
                    {
                        apiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                    }
                }
                else
                {
                    var hetVakjeWatWeGaanKlikken = potentialBombs.First();
                    logger.WriteLine($"Beste keuze (Hoogste kans): {hetVakjeWatWeGaanKlikken}", ConsoleColor.DarkBlue);
                    if (game.myTurn)
                    {
                        apiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                    }
                }
            }

            logger.WriteLine(string.Empty);
            logger.WriteLine(string.Empty);
        }

        public void StartConnection()
        {
            connection.On<GalaxySweeperGame>("gameUpdated", (game) =>
            {
                Console.WriteLine($"gameUpdated message ontvangen");

                HandleGameSweeperGameMessage(game);
            });

            connection.On<GalaxySweeperGame>("inviteAccepted", (game) =>
            {
                Console.WriteLine($"inviteAccepted message ontvangen");
            });

            try
            {
                connection.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
