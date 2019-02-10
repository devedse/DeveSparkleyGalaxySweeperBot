using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System.Linq;
using Xunit;

namespace DeveSparkleyGalaxySweeperBot.Tests
{
    public class BombFindTests
    {
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

            BommenBepaler.BepaalBommenMulti(deVakjesArray);

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

            BommenBepaler.BepaalBommenMulti(deVakjesArray);

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

            BommenBepaler.BepaalBommenMulti(deVakjesArray);

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

            BommenBepaler.BepaalBommenMulti(deVakjesArray);

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

            BommenBepaler.BepaalBommenMulti(deVakjesArray);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(5, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
            Assert.Equal(4, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedNoBom));
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs3()
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


            BommenBepaler.BepaalBommenMulti(deVakjesArray);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();

            LogTopBombs(flattened, logger);
            GalaxyVisualizator.RenderToConsole(deVakjesArray, logger);

            Assert.Equal(5, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }

        private static void LogTopBombs(System.Collections.Generic.List<Vakje> flattened, ILogger logger)
        {
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans).Take(10);
            foreach (var maybeBom in ordered)
            {
                logger.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }
        }
    }
}
