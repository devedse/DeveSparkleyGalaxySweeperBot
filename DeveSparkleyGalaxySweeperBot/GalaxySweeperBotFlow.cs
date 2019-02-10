using DeveSparkleyGalaxySweeperBot.Logging;
using System;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperBotFlow
    {
        private readonly GalaxySweeperApiHelper galaxySweeperApiHelper;
        private readonly GalaxySweeperBot galaxySweeperBot;
        private readonly GalaxySweeperSignalRHandler signalHandler;
        private readonly ILogger logger;

        public GalaxySweeperBotFlow(string bearerToken, ILogger logger)
        {
            galaxySweeperApiHelper = new GalaxySweeperApiHelper(bearerToken, logger);
            galaxySweeperBot = new GalaxySweeperBot(galaxySweeperApiHelper, logger);
            signalHandler = new GalaxySweeperSignalRHandler(bearerToken, galaxySweeperBot, galaxySweeperApiHelper, logger);
            this.logger = logger;
        }

        public void StartBot()
        {
            Console.WriteLine("The bot has started");
            signalHandler.StartConnection();
        }
    }
}
