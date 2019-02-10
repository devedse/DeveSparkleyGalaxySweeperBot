using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            StartBot();
            DoeIetsAnders();

            Console.WriteLine("Press any key to exit the application");
            Console.ReadLine();
        }

        private static ILogger CreateLogger()
        {
            return DefaultLoggerFactory.CreateLoggerForConsoleAppFast();
        }

        private static BotConfig CreateBotConfig()
        {
            return BotConfig.Level9;
        }

        public static void StartBot()
        {
            var logger = CreateLogger();
            var a = new GalaxySweeperBotFlow(GalaxySweeperConfig.AccessToken, CreateBotConfig(), logger);
            a.StartBot();
        }

        public static void DoeIetsAnders()
        {
            var logger = CreateLogger();

            var galaxySweeperApiHelper = new GalaxySweeperApiHelper(GalaxySweeperConfig.AccessToken, logger);
            var galaxySweeperBot = new GalaxySweeperBot(galaxySweeperApiHelper, CreateBotConfig(), logger);

            var fakeGame = JsonConvert.DeserializeObject<GalaxySweeperGame>(File.ReadAllText("SampleGameData.txt"));

            var allGames = galaxySweeperApiHelper.GetGames().Result;

            foreach (var game in allGames.Where(t => !t.isFinished))
            {
                galaxySweeperBot.DetermineBestMove(game, true);
            }

            galaxySweeperBot.AcceptInvitesForAllGames(allGames);
        }
    }
}
