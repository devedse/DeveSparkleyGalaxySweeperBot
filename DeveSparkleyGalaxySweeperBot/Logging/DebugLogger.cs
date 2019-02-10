using System;
using System.Diagnostics;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class DebugLogger : ILogger
    {
        public void Write(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            Debug.Write(txt);
        }

        public void Write(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            Debug.Write(txt);
        }

        public void WriteLine(string txt, ConsoleColor color = ConsoleColor.Black)
        {
            Debug.WriteLine(txt);
        }

        public void WriteLine(char txt, ConsoleColor color = ConsoleColor.Black)
        {
            Debug.WriteLine(txt);
        }
    }
}
