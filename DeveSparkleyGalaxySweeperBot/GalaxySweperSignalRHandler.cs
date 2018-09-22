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

        public void StartConnection()
        {
            connection.On<GalaxySweeperGame>("gameUpdated", (game) =>
            {
                Console.WriteLine($"gameUpdated message ontvangen");
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
