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

            StartBot();
            //DoeIetsAnders();

            Console.WriteLine("Press any key to exit the application");
            Console.ReadKey();
        }

        public static void StartBot()
        {

            var a = new GalaxySweeperBotFlow();
            a.StartBot();
        }

        public static void DoeIetsAnders()
        {
            var a = new GalaxySweeperBotFlow();
            var b = new GalaxySweperSignalRHandler();
            var fakeGame = JsonConvert.DeserializeObject<GalaxySweeperGame>(File.ReadAllText("SampleGameData.txt"));

            var deVakjesArray = b.CreateVakjesArray(fakeGame);


            b.LogVakjesDeluxe(deVakjesArray);
        }
    }
}
