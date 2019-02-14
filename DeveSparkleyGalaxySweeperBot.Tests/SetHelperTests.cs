using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeveSparkleyGalaxySweeperBot.Tests
{
    public class SetHelperTests
    {
      

        [Fact]
        public void FindsSetsWithGuaranteedBombs()
        {
            var logger = DefaultLoggerFactory.CreateLoggerForTests();




            var vakjeLinksBoven = new Vakje('.', 0, 0);
            var vakjeRechtsBoven = new Vakje('.', 1, 0);
            var vakjeLinks = new Vakje('.', 0, 1);
            var vakjeRechts= new Vakje('.', 1, 1);
            var vakjeLinksOnder= new Vakje('.', 0, 2);
            var vakjeRechtsOnder = new Vakje('.', 1, 2);

            var alleVakjes = new List<Vakje>() { vakjeLinksBoven, vakjeRechtsBoven, vakjeRechts, vakjeRechtsOnder, vakjeLinksOnder, vakjeLinks };
            var overallSet = new VakjeSetDeluxe(0, 0, alleVakjes);

            //Set1 groen linksboven
            var set1 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeLinks, vakjeLinksBoven });

            //Set2 rood top
            var set2 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeLinksBoven, vakjeRechtsBoven });

            //Set3 Rood rechts (3 set)
            var set3 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeRechtsBoven, vakjeRechts, vakjeRechtsOnder });

            //Set4 Blauw rechtsboven
            var set4 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeRechtsBoven, vakjeRechts });

            //Set5 Blauw onder
            var set5 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeLinksOnder, vakjeRechtsOnder });

            //Set6 Rood linksonder (Enkel)
            var set6 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeLinksOnder });

            //Set7 Blauw links (Enkel)
            var set7 = new VakjeSetDeluxe(0, 0, new List<Vakje>() { vakjeLinks });

            var alleSets = new List<VakjeSetDeluxe>()
            {
                set1,
                set2,
                set3,
                set4,
                set5,
                set6,
                set7
            };

            var result = SetBepaler.BepaalSetsThatFillMeCompletely(overallSet, alleSets);

            Assert.Equal(2, result.Count);
        }
    }
}
