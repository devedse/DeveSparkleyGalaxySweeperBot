namespace DeveSparkleyGalaxySweeperBot.Logging
{
    public static class DefaultLoggerFactory
    {
        public static ILogger CreateLoggerForTests()
        {
            var consoleLogger = new ConsoleLoggerWithColor();
            var debugLogger = new DebugLogger();

            var multiLogger = new MultiLogger(consoleLogger, debugLogger);

            var dateTimeLogger = new DateTimeLogger(multiLogger);

            var cacheAsLinesLogger = new CacheAsLinesLogger(dateTimeLogger);

            return cacheAsLinesLogger;
        }

        public static ILogger CreateLoggerForConsoleApp()
        {
            var consoleLogger = new ConsoleLoggerWithColor();

            return consoleLogger;
        }
    }
}
