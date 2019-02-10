using System;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class CacheAsLinesLogger : ILogger
    {
        public ILogger NextLogger { get; }
        private StringBuilder sb = new StringBuilder();

        public CacheAsLinesLogger(ILogger nextLogger)
        {
            NextLogger = nextLogger;
        }

        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            sb.Append(txt);
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            sb.Append(txt);
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            sb.Append(txt);
            NextLogger.WriteLine(sb.ToString());
            sb.Clear();
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            sb.Append(txt);
            NextLogger.WriteLine(sb.ToString());
            sb.Clear();
        }
    }
}
