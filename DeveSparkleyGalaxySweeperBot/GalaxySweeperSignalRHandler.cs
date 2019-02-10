using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperSignalRHandler
    {
        private HubConnection _connection;
        private readonly GalaxySweeperBot _galaxySweeperBot;
        private readonly GalaxySweeperApiHelper _galaxySweeperApiHelper;
        private readonly ILogger _logger;

        public GalaxySweeperSignalRHandler(string bearerToken, GalaxySweeperBot galaxySweeperBot, GalaxySweeperApiHelper galaxySweeperApiHelper, ILogger logger)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://galaxysweeper.com/hubs/minesweeper", t => t.AccessTokenProvider = () => Task.FromResult(bearerToken))
                .Build();
            _galaxySweeperBot = galaxySweeperBot;
            _galaxySweeperApiHelper = galaxySweeperApiHelper;
            _logger = logger;
        }

        public void StartConnection()
        {
            _connection.On<GalaxySweeperGame>("gameUpdated", (game) =>
            {
                Console.WriteLine($"gameUpdated message ontvangen");

                _galaxySweeperBot.DetermineBestMove(game, true);

                if (game.isFinished)
                {
                    _galaxySweeperApiHelper.AcceptInvite(game.opponent.challengeToken);
                }
            });

            _connection.On<GalaxySweeperGame>("inviteAccepted", (game) =>
            {
                Console.WriteLine($"inviteAccepted message ontvangen");
            });

            try
            {
                _connection.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
