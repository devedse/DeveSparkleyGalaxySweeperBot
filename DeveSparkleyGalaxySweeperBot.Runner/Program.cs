using DeveCoolLib.DeveConsoleMenu;
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
            Console.WriteLine("The bot is starting");

            var logger = CreateLogger();

            StartBot(logger);
            DoeIetsAnders(logger);

            Console.WriteLine("Press any key to exit the application");
            Console.ReadLine();
        }

        private static ILogger CreateLogger()
        {
            ILogger selectedLogger = null;

            Console.WriteLine();

            var selectLoggerMenu = new ConsoleMenu(ConsoleMenuType.KeyPress);
            selectLoggerMenu.MenuOptions.Add(new ConsoleMenuOption("Color Logger", () => selectedLogger = DefaultLoggerFactory.CreateLoggerForConsoleApp()));
            selectLoggerMenu.MenuOptions.Add(new ConsoleMenuOption("FaST Logger", () => selectedLogger = DefaultLoggerFactory.CreateLoggerForConsoleAppFast()));

            selectLoggerMenu.RenderMenu();
            selectLoggerMenu.WaitForResult();

            return selectedLogger;
        }

        private static BotConfig CreateBotConfig()
        {
            return BotConfig.Level9;
        }

        public static void StartBot(ILogger logger)
        {
            var a = new GalaxySweeperBotFlow(GalaxySweeperConfig.AccessToken, CreateBotConfig(), logger);
            a.StartBot();
        }

        public static void DoeIetsAnders(ILogger logger)
        {
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
