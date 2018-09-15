using System;

namespace DeveSparkleyGalaxySweeperBot.Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var a = new GalaxySweeperBotFlow();
            a.StartBot();

            Console.WriteLine("Press any key to exit the application");
            Console.ReadKey();
        }
    }
}
