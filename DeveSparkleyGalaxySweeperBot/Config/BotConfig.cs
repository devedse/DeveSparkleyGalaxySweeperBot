namespace DeveSparkleyGalaxySweeperBot.Config
{
    public class BotConfig
    {
        public bool UseSetDetection { get; set; }
        public bool AlwaysAvoidClickingOpenFields { get; set; }

        public BotConfig()
        {
        }



        public static BotConfig Level9 { get; } = new BotConfig()
        {
            AlwaysAvoidClickingOpenFields = true,
            UseSetDetection = true,
        };

        public static BotConfig Level8 { get; } = new BotConfig()
        {
            AlwaysAvoidClickingOpenFields = false,
            UseSetDetection = true,
        };
    }
}
