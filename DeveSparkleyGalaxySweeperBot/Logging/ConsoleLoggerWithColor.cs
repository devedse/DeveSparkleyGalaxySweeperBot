using System;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class ConsoleLoggerWithColor : ILogger
    {
        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            if (Console.BackgroundColor != color)
            {
                Console.BackgroundColor = color;
            }
            Console.Write(txt);
            if (Console.BackgroundColor != ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            if (Console.BackgroundColor != color)
            {
                Console.BackgroundColor = color;
            }
            Console.Write(txt);
            if (Console.BackgroundColor != ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            if (Console.BackgroundColor != color)
            {
                Console.BackgroundColor = color;
            }
            Console.WriteLine(txt);
            if (Console.BackgroundColor != ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            if (Console.BackgroundColor != color)
            {
                Console.BackgroundColor = color;
            }
            Console.WriteLine(txt);
            if (Console.BackgroundColor != ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
