using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class MultiLogger : ILogger
    {
        public List<ILogger> Loggers { get; }

        public MultiLogger(params ILogger[] loggers)
        {
            Loggers = loggers.ToList();
        }

        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            foreach (var logger in Loggers)
            {
                logger.Write(txt, color);
            }
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            foreach (var logger in Loggers)
            {
                logger.Write(txt, color);
            }
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            foreach (var logger in Loggers)
            {
                logger.WriteLine(txt, color);
            }
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            foreach (var logger in Loggers)
            {
                logger.WriteLine(txt, color);
            }
        }
    }
}
