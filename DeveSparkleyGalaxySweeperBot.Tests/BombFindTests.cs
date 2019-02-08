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

            GalaxyVisualizator.RenderToConsole(deVakjesArray);
        }
    }
}
