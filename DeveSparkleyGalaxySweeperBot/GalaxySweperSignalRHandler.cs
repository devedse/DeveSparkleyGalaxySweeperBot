using DeveSparkleyGalaxySweeperBot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweperSignalRHandler
    {
        private HubConnection connection;
        private string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0ODhjNWEyMy04MmRkLTRmYmItYWNiZi01MmE3ZWVkOGRiMzgiLCJuYmYiOjE1MzY5MzMwMDksImV4cCI6MTU0MjExNzAwOSwiaWF0IjoxNTM2OTMzMDA5LCJpc3MiOiJodHRwczovL2Rldi5nYWxheHlzd2VlcGVyLmNvbSIsImF1ZCI6Imh0dHBzOi8vZGV2LmdhbGF4eXN3ZWVwZXIuY29tIn0.UMKdleQRzx3L2dQhNtjomfzJJS2FwPhSoaTruPnXYpA";

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
                        var vakje = new Vakje(character);
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
