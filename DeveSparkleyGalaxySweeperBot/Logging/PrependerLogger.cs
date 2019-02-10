using System;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class PrependerLogger : ILogger
    {
        public ILogger NextLogger { get; }
        private readonly Func<string> toAppend;

        public PrependerLogger(ILogger nextLogger, Func<string> toAppend)
        {
            NextLogger = nextLogger;
            this.toAppend = toAppend;
        }


        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            NextLogger.Write($"{toAppend()}{txt}", color);
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            NextLogger.Write($"{toAppend()}{txt}", color);
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            NextLogger.WriteLine($"{toAppend()}{txt}", color);
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            NextLogger.WriteLine($"{toAppend()}{txt}", color);
        }
    }
}
