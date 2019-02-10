using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperSignalRHandler
    {
        private HubConnection connection;
        private Random random = new Random();
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
            BommenBepaler.BepaalBommenMulti(deVakjesArray);

            var alleVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray);

            List<Vakje> bommenDieIkMoetKlikken = alleVakjes.Where(t => t != null && t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom).ToList();

            foreach (var bomGevonden in bommenDieIkMoetKlikken)
            {
                Console.WriteLine($"Bom: ({bomGevonden.X},{bomGevonden.Y})");
            }

            var deBom = bommenDieIkMoetKlikken.FirstOrDefault();
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            if (game.myTurn == true && deBom != null)
            {
                apiHelper.Sweep(game.id, deBom.X, deBom.Y);
            }
            else
            {
                var vakjesIenumerable = TwoDimensionalArrayHelper.Flatten(deVakjesArray);
                var vakjeBepaling = vakjesIenumerable.Where(t => t != null && t.Revealed == false).ToList();

                var vakjesMetBomErnaast = vakjeBepaling.Where(t => t.SurroundingVakjes.Any(z => z != null && z.IsBomb)).ToList();
                if (vakjesMetBomErnaast.Any())
                {
                    var hetVakjeWatWeGaanKlikken = vakjesMetBomErnaast[random.Next(0, vakjesMetBomErnaast.Count)];
                    apiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                }
                else
                {
                    var hetVakjeWatWeGaanKlikken = vakjeBepaling[random.Next(0, vakjeBepaling.Count)];
                    apiHelper.Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
                }
            }

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
