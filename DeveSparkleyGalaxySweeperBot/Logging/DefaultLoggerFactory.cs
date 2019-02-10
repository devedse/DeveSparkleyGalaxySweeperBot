namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public static class DefaultLoggerFactory
    {
        public static ILogger CreateLoggerForTests()
        {
            var consoleLogger = new ConsoleLoggerWithColor();
            var debugLogger = new DebugLogger();

            var multiLogger = new MultiLogger(consoleLogger, debugLogger);

            var cacheAsLinesLogger = new CacheAsLinesLogger(multiLogger);

            return cacheAsLinesLogger;
        }

        public static ILogger CreateLoggerForConsoleApp()
        {
            var consoleLogger = new ConsoleLoggerWithColor();

            return consoleLogger;
        }

        public static ILogger CreateLoggerForConsoleAppFast()
        {
            var consoleLogger = new ConsoleLoggerFastNoColor();
            var cacheAsLinesLogger = new CacheAsLinesLogger(consoleLogger);
            return cacheAsLinesLogger;
        }
    }
}
