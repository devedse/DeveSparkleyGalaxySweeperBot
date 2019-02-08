using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using System;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot
{
    public static class BommenBepaler
    {
        public static void BepaalBommenMulti(Vakje[,] deVakjesArray)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            bool doorGaan = true;
            int hoeveelsteKeer = 0;
            while (doorGaan)
            {
                doorGaan = BepaalBommen(deVakjesArray, width, height);
                Console.WriteLine($"{hoeveelsteKeer}: Bommen gevonden: {TwoDimensionalArrayHelper.Flatten(deVakjesArray).Count(t => t != null && t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)}");
                hoeveelsteKeer++;
            }
        }

        public static bool BepaalBommen(Vakje[,] deVakjesArray, int width, int height)
        {
            //Clear de sets
            var flatVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null);
            foreach (var vakje in flatVakjes)
            {
                vakje.VakjeBerekeningen.Sets.Clear();
            }

            var alleUnrevealedDieMisschienEenBomZijn = flatVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
            var theBigUnrevealedSet = new VakjeSet(51 - flatVakjes.Count(t => t.IsBomb), alleUnrevealedDieMisschienEenBomZijn);
            foreach (var unrev in alleUnrevealedDieMisschienEenBomZijn)
            {
                unrev.VakjeBerekeningen.TheBigUnrevealedSet = theBigUnrevealedSet;
            }



            bool erIsIetsBerekend = false;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        // || t.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom
                        var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
                        var unrevealedTilesOmMeHeenZonderGuaranteedNoBom = vakje.SurroundingVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();



                        var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed).ToList();

                        if (vakje.IsNumber && vakje.Number - bommenOmMeHeen == unrevealedTilesOmMeHeenZonderGuaranteedNoBom.Count)
                        {
                            foreach (var unrevealed in unrevealedTilesOmMeHeenZonderGuaranteedNoBom)
                            {
                                if (unrevealed.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                                {
                                    unrevealed.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                                    erIsIetsBerekend = true;
                                }
                            }
                        }
                        else if (vakje.IsNumber && vakje.Number == bommenOmMeHeen)
                        {
                            foreach (var unrevealed in unrevealedTilesOmMeHeen)
                            {
                                if (unrevealed.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom)
                                {
                                    unrevealed.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedNoBom;
                                    erIsIetsBerekend = true;
                                }
                            }
                        }


                        if (vakje.IsNumber)
                        {
                            var set = new VakjeSet(vakje.Number - bommenOmMeHeen, unrevealedTilesOmMeHeenZonderGuaranteedNoBom);
                            foreach (var unrevealed in unrevealedTilesOmMeHeenZonderGuaranteedNoBom)
                            {
                                unrevealed.VakjeBerekeningen.Sets.Add(set);
                            }
                        }
                    }
                }
            }

            return erIsIetsBerekend;
        }
    }
}
