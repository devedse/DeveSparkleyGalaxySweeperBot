using DeveSparkleyGalaxySweeperBot.Helpers;
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

        public GalaxySweeperSignalRHandler(string bearerToken, GalaxySweeperApiHelper apiHelper)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://galaxysweeper.com/hubs/minesweeper", t => t.AccessTokenProvider = () => Task.FromResult(bearerToken))
                .Build();
            this.apiHelper = apiHelper;
        }

        public void HandleGameSweeperGameMessage(GalaxySweeperGame game)
        {
            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);


            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);



            //Nu is alle data goed
            bool doorGaan = true;
            int hoeveelsteKeer = 0;
            while (doorGaan)
            {
                doorGaan = BepaalBommen(deVakjesArray, width, height);
                Console.WriteLine($"{hoeveelsteKeer}: Bommen gevonden: {TwoDimensionalArrayHelper.Flatten(deVakjesArray).Count(t => t != null && t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)}");
                hoeveelsteKeer++;
            }

            var alleVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray);

            List<Vakje> bommenDieIkMoetKlikken = alleVakjes.Where(t => t != null && t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom).ToList();

            foreach (var bomGevonden in bommenDieIkMoetKlikken)
            {
                Console.WriteLine($"Bom: ({bomGevonden.X},{bomGevonden.Y})");
            }

            var deBom = bommenDieIkMoetKlikken.FirstOrDefault();
            GalaxyVisualizator.RenderToConsole(deVakjesArray);

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



        private static bool BepaalBommen(Vakje[,] deVakjesArray, int width, int height)
        {
            bool erIsIetsBerekend = false;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        // || t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom
                        var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
                        var unrevealedTilesOmMeHeenZonderGuaranteedNoBom = vakje.SurroundingVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
                        if (vakje.IsNumber && vakje.Number - bommenOmMeHeen == unrevealedTilesOmMeHeenZonderGuaranteedNoBom.Count)
                        {
                            foreach (var unrevealed in unrevealedTilesOmMeHeenZonderGuaranteedNoBom)
                            {
                                if (unrevealed.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                                {
                                    unrevealed.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                                    erIsIetsBerekend = true;
                                }
                            }
                            continue;
                        }


                        var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed);
                        if (vakje.IsNumber && vakje.Number == bommenOmMeHeen)
                        {
                            foreach (var unrevealed in unrevealedTilesOmMeHeen)
                            {
                                if (unrevealed.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom)
                                {
                                    unrevealed.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedNoBom;
                                    erIsIetsBerekend = true;
                                }
                            }
                        }

                    }
                }
            }

            return erIsIetsBerekend;
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
