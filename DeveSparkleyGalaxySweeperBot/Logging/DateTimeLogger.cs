using System;

namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public class DateTimeLogger : PrependerLogger
    {
        public DateTimeLogger(ILogger nextLogger) : base(nextLogger, GetDateTimePrepentionString)
        {
        }

        private static string GetDateTimePrepentionString()
        {
            return $"{DateTime.Now}: ";
        }
    }
}
