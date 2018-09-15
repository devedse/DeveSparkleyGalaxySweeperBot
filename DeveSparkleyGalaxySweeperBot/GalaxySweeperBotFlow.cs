using System;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperBotFlow
    {
        private GalaxySweperSignalRHandler signalHandler;
        public GalaxySweeperBotFlow()
        {
            signalHandler = new GalaxySweperSignalRHandler();
        }

        public void StartBot()
        {
            Console.WriteLine("The bot has started");
            signalHandler.StartConnection();
        }
    }
}
