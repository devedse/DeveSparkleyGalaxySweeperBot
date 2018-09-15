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
        private string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjYThkM2NhNi03MGY2LTQ3ZTEtOGVhMi1jYWEwMDVjODA0YTAiLCJuYmYiOjE1MzY4Njk3NTAsImV4cCI6MTU0MjA1Mzc1MCwiaWF0IjoxNTM2ODY5NzUwLCJpc3MiOiJodHRwczovL2Rldi5nYWxheHlzd2VlcGVyLmNvbSIsImF1ZCI6Imh0dHBzOi8vZGV2LmdhbGF4eXN3ZWVwZXIuY29tIn0.bvqFlgixfpmAcG_Jb4zhIkNp_GxqDpFLsC5pOyKQCKs";

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
