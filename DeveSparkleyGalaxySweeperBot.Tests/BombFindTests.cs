using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System.Linq;
using Xunit;

namespace DeveSparkleyGalaxySweeperBot.Tests
{
    public class BombFindTests
    {
        private static void LogTopBombs(System.Collections.Generic.List<Vakje> flattened, ILogger logger)
        {
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans).Take(10);
            foreach (var maybeBom in ordered)
            {
                logger.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }
        }

        public BotConfig TestBotConfig()
        {
            return BotConfig.Level9;
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#........232.....",
                                    ".................",
                                    "........B3B.....#",
                                    "........11.....##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(2, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs2()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".................",
                                    "........121.....#",
                                    "........1......##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(1, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }


        [Fact]
        public void EnsureDoesntFindFalsePositives()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".................",
                                    "........122.....#",
                                    "........1......##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }


        [Fact]
        public void EnsureDoesntFindFalsePositives2()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".................",
                                    "........122.....#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void FindsNonGuaranteedBombsAndThenRecursivelyFindsGuaranteedBombs()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    "...........1.....",
                                    "........12..4...#",
                                    "........11.....##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(5, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
            Assert.Equal(7, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedNoBom));
        }

        [Fact]
        public void EnsureDoesntFindFalsePositives3()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    "...........5.....",
                                    "........12......#",
                                    "........11.....##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs4()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    "...........1.....",
                                    "........12......#",
                                    "........11.....##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(1, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void DoesntFindAllTheseFalsePositives()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            //Hier vond hij random bommen
            string[] data = new[] { "########B3..1.B10",
                                    "#######BRR2.B3310",
                                    "######1233BR3BR10",
                                    "#####001B.2..B200",
                                    "####0002B..R21000",
                                    "###210013B2100001",
                                    "##BB101.R3000001R",
                                    "#.3201B3R1000002.",
                                    "..R102..1111112R.",
                                    ".1.12B2..R.R.R.1#",
                                    "...R.3RB21.2RB.##",
                                    "...1.R3.....R2###",
                                    "....B3..B.12.####",
                                    "...B3R..3..R#####",
                                    ".....1.2..2######",
                                    "..........#######",
                                    ".........########" };


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void DoesntFindAllTheseFalsePositives2()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".........23......",
                                    ".......R........#",
                                    ".......1.......##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void DoesntFindAllTheseFalsePositives3()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".........33......",
                                    "................#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void DoesntFindAllTheseFalsePositives4()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".........44......",
                                    "................#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};


            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void DoesntFindAllTheseFalsePositives5()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###..............",
                                    "##...............",
                                    "#................",
                                    ".........55......",
                                    "................#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};

            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(8, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void FindsTheseThroughSetsRecursively()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####........11...",
                                    "###........111...",
                                    "##...............",
                                    "#..........3.1...",
                                    ".........1..11...",
                                    ".......4...11...#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};

            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(4, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        [Fact]
        public void FindsTheseThroughSetsRecursively2()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();

            string[] data = new[] { "########.........",
                                    "#######..........",
                                    "######...........",
                                    "#####............",
                                    "####.............",
                                    "###.........1....",
                                    "##...............",
                                    "#..........3.....",
                                    ".........1..1....",
                                    ".......4........#",
                                    "...............##",
                                    "..............###",
                                    ".............####",
                                    "............#####",
                                    "...........######",
                                    "..........#######",
                                    ".........########"};

            var game = new GalaxySweeperGame
            {
                field = data.ToList()
            };

            var deVakjesArray = GalaxyGameHelper.CreateVakjesArray(game);

            var stats = BommenBepaler.BepaalBommenMulti2(deVakjesArray, TestBotConfig());
            stats.Log(logger);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(4, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }
    }
}
