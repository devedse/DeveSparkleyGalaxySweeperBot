using System;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperBotFlow
    {
        private readonly GalaxySweeperApiHelper apiHandler;
        private readonly GalaxySweeperSignalRHandler signalHandler;
        public GalaxySweeperBotFlow(string bearerToken)
        {
            apiHandler = new GalaxySweeperApiHelper(bearerToken);
            signalHandler = new GalaxySweeperSignalRHandler(bearerToken, apiHandler);
        }

        public void StartBot()
        {
            Console.WriteLine("The bot has started");
            signalHandler.StartConnection();
        }
    }
}
