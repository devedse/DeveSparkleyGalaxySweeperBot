using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweperSignalRHandler
    {
        private HubConnection connection;
        private Random random = new Random();
        private string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NDlkNGI4YS1mZWYwLTQ2MDUtYTNhYy05NGY1OTYxMDc0YzYiLCJuYmYiOjE1MzY4NjE2OTksImV4cCI6MTU0MjA0NTY5OSwiaWF0IjoxNTM2ODYxNjk5LCJpc3MiOiJodHRwczovL2Rldi5nYWxheHlzd2VlcGVyLmNvbSIsImF1ZCI6Imh0dHBzOi8vZGV2LmdhbGF4eXN3ZWVwZXIuY29tIn0.RDki1s_C4D17-s2OCXj3URZEg-bMGOVXPr1s7en2SRU";

        public GalaxySweperSignalRHandler()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://galaxysweeper.com/hubs/minesweeper", t => t.AccessTokenProvider = () => Task.FromResult(accessToken))
                .Build();
        }

        public void HandleGameSweeperGameMessage(GalaxySweeperGame game)
        {
            Vakje[,] deVakjesArray = new Vakje[game.field.Count, game.field[0].Length];

            for (int x = 0; x < game.field.Count; x++)
            {
                var line = game.field[x];
                for (int y = 0; y < line.Length; y++)
                {
                    var character = line[y];
                    if (character != '#')
                    {
                        var vakje = new Vakje(character, x, y);
                        deVakjesArray[x, y] = vakje;
                    }
                }
            }

            LogVakjes(deVakjesArray);

            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x, y - 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x + 1, y - 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x - 1, y);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x + 1, y);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x - 1, y + 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x, y + 1);
                    }
                }
            }


            var bommenDieIkMoetKlikken = new List<Vakje>();

            //Nu is alle data goed

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {

                        var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
                        var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed).ToList();
                        if (vakje.IsNumber && vakje.Number - bommenOmMeHeen == unrevealedTilesOmMeHeen.Count)
                        {
                            foreach (var unrevealed in unrevealedTilesOmMeHeen)
                            {
                                Console.WriteLine($"Dit is een bom: X: {unrevealed.X} Y: {unrevealed.Y}");
                                bommenDieIkMoetKlikken.Add(unrevealed);
                            }
                        }


                    }
                }
            }




            var deBom = bommenDieIkMoetKlikken.FirstOrDefault();
            if (game.myTurn == true && deBom != null)
            {
                Sweep(game.id, deBom.X, deBom.Y);
            }
            else
            {           
                var vakjesIenumerable = TwoDimensionalArrayHelper.Flatten(deVakjesArray);
                var vakjeBepaling = vakjesIenumerable.Where(t => t != null && t.Revealed == false).ToList();

                var hetVakjeWatWeGaanKlikken = vakjeBepaling[random.Next(0, vakjeBepaling.Count)];
                
                Sweep(game.id, hetVakjeWatWeGaanKlikken.X, hetVakjeWatWeGaanKlikken.Y);
            }

        }

        public void Sweep(string gameId, int row, int column)
        {
            //9890b1f2-b87b-4043-ba6a-62037ae921b5
            string referer = $"https://galaxysweeper.com/game/{gameId}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                var content = new StringContent("{row: " + row + ", column: " + column + "}", Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync($"https://galaxysweeper.com/api/games/{gameId}/sweep", content).Result;
            }
        }

        public void AddVakjeToNeightboursIfNotNull(Vakje hetVakje, Vakje[,] deVakjesArray, int x, int y)
        {
            var hetPotentieleNeighbourVakje = GetVakjeFromArray(deVakjesArray, x, y);
            if (hetPotentieleNeighbourVakje != null)
            {
                hetVakje.SurroundingVakjes.Add(hetPotentieleNeighbourVakje);
            }
        }

        public Vakje GetVakjeFromArray(Vakje[,] deVakjesArray, int x, int y)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            if (y >= 0 && y < height && x < width && x >= 0)
            {
                var vakje = deVakjesArray[x, y];
                if (vakje != null && vakje.Value != '#')
                {
                    return vakje;
                }
            }
            return null;
        }

        private void LogVakjes(Vakje[,] deVakjesArray)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        Console.Write(vakje.Value + " ");
                    }
                    else
                    {
                        Console.Write("# ");
                    }
                }

                Console.WriteLine();
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
