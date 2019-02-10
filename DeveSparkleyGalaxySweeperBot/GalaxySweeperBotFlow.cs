using DeveSparkleyGalaxySweeperBot.Logging;
using System;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperBotFlow
    {
        private readonly GalaxySweeperApiHelper apiHandler;
        private readonly GalaxySweeperSignalRHandler signalHandler;
        private readonly ILogger logger;

        public GalaxySweeperBotFlow(string bearerToken, ILogger logger)
        {
            apiHandler = new GalaxySweeperApiHelper(bearerToken, logger);
            signalHandler = new GalaxySweeperSignalRHandler(bearerToken, apiHandler, logger);
            this.logger = logger;
        }

        public void StartBot()
        {
            Console.WriteLine("The bot has started");
            signalHandler.StartConnection();
        }
    }
}
