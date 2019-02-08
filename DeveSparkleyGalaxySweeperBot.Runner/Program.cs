using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DeveSparkleyGalaxySweeperBot.Runner
{
    public class Program
    {


        public static void Main(string[] args)
        {







            Console.WriteLine("Hello World!");

            //StartBot();
            DoeIetsAnders();

            Console.WriteLine("Press any key to exit the application");
            Console.ReadKey();
        }

        public static void StartBot()
        {

            var a = new GalaxySweeperBotFlow(GalaxySweeperConfig.AccessToken);
            a.StartBot();
        }

        public static void DoeIetsAnders()
        {
            //var a = new GalaxySweeperBotFlow(GalaxySweeperConfig.AccessToken);
            var ccc = new GalaxySweeperApiHelper(GalaxySweeperConfig.AccessToken);
            //var b = new GalaxySweeperSignalRHandler(GalaxySweeperConfig.AccessToken, ccc);
            var fakeGame = JsonConvert.DeserializeObject<GalaxySweeperGame>(File.ReadAllText("SampleGameData.txt"));

            var allGames = ccc.GetGames().Result;


            foreach (var game in allGames)
            {
                var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);
                Console.WriteLine($"{game.id}:");
                GalaxyVisualizator.RenderToConsole(deVakjesArray);
            }
        }


    }
}
