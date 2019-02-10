using System;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class ConsoleLoggerFastNoColor : ILogger
    {
        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            Console.Write(txt);
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            Console.Write(txt);
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            Console.WriteLine(txt);
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            Console.WriteLine(txt);
        }
    }
}
