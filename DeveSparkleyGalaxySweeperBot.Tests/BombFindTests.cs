using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace DeveSparkleyGalaxySweeperBot.Tests
{
    public class BombFindTests
    {
        [Fact]
        public void FindsSetsWithGuaranteedBombs()
        {
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
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            foreach (var maybeBom in ordered)
            {
                Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }

            Assert.Equal(2, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs2()
        {
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
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            foreach (var maybeBom in ordered)
            {
                Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }

            Assert.Equal(1, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);
        }


        [Fact]
        public void EnsureDoesntFindFalsePositives()
        {
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
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            foreach (var maybeBom in ordered)
            {
                Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);
        }


        [Fact]
        public void EnsureDoesntFindFalsePositives2()
        {
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
            var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            foreach (var maybeBom in ordered)
            {
                Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            }

            Assert.Equal(0, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);
        }



        [Fact]
        public void FindsNonGuaranteedBombsAndThenRecursivelyFindsGuaranteedBombs()
        {
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

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);


            BommenBepaler.BepaalBommenMulti(deVakjesArray);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();
            //var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            //foreach (var maybeBom in ordered)
            //{
            //    Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            //}

            Assert.Equal(5, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
            Assert.Equal(4, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedNoBom));

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);
        }

        [Fact]
        public void FindsSetsWithGuaranteedBombs3()
        {
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

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);


            BommenBepaler.BepaalBommenMulti(deVakjesArray);

            var flattened = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null).ToList();
            //var ordered = flattened.OrderBy(t => t.VakjeBerekeningen.BerekendeVakjeKans);
            //foreach (var maybeBom in ordered)
            //{
            //    Debug.WriteLine($"Bom: ({maybeBom.VakjeBerekeningen.BerekendeVakjeKans}) ({maybeBom.X},{maybeBom.Y})");
            //}

            GalaxyVisualizator.RenderToConsole(deVakjesArray, true);

            Assert.Equal(5, flattened.Count(t => t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom));
        }
    }
}
