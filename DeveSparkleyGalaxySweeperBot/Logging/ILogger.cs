using System;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public interface ILogger
    {
        void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black);
        void Write(string txt, ConsoleColor color = ConsoleColor.Black);

        void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black);
        void Write(char txt, ConsoleColor color = ConsoleColor.Black);
    }
}
