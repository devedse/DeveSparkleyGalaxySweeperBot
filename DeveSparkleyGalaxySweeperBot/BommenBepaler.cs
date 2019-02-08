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

            foreach (var vakje in flatVakjes)
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
            }


            foreach (var vakje in flatVakjes)
            {
                if (vakje.IsNumber)
                {
                    var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
                    var unrevealedTilesOmMeHeenZonderGuaranteedNoBom = vakje.SurroundingVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
                    var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed).ToList();

                    var set = new VakjeSet(vakje.Number - bommenOmMeHeen, unrevealedTilesOmMeHeenZonderGuaranteedNoBom);
                    foreach (var unrevealed in unrevealedTilesOmMeHeenZonderGuaranteedNoBom)
                    {
                        unrevealed.VakjeBerekeningen.Sets.Add(set);
                    }
                }
            }







            alleUnrevealedDieMisschienEenBomZijn = flatVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();


            foreach (var vakje in alleUnrevealedDieMisschienEenBomZijn)
            {
                //var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
                //var unrevealedTilesOmMeHeenZonderGuaranteedNoBom = vakje.SurroundingVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
                //var hoeveelBommenNogOmMeHeenTeZoeken = vakje.Number - bommenOmMeHeen;


                foreach (var set in vakje.VakjeBerekeningen.Sets)
                {
                    if (set.CountVanBommenDieErMoetenZijn == 3)
                    {

                    }

                    //we kijken nu per set
                    foreach (var vakjeInSet in set.Vakjes)
                    {

                        foreach (var setVanDeze in vakjeInSet.VakjeBerekeningen.Sets)
                        {
                            if (set.Vakjes.Intersect(setVanDeze.Vakjes).Count() == setVanDeze.Vakjes.Count)
                            {
                                var vakjesNietInDezeSet = set.Vakjes.Except(setVanDeze.Vakjes).ToList();
                                if (vakjesNietInDezeSet.Count == set.CountVanBommenDieErMoetenZijn - setVanDeze.CountVanBommenDieErMoetenZijn)
                                {
                                    foreach (var vakjeNietInDezeSet in vakjesNietInDezeSet)
                                    {
                                        if (vakjeNietInDezeSet.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                                        {
                                            vakjeNietInDezeSet.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                                            erIsIetsBerekend = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }



            //foreach (var vakje in flatVakjes)
            //{
            //    if (vakje.IsNumber)
            //    {


            //        var bommenOmMeHeen = vakje.SurroundingVakjes.Count(t => t.IsBomb);
            //        var unrevealedTilesOmMeHeenZonderGuaranteedNoBom = vakje.SurroundingVakjes.Where(t => !t.Revealed && t.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom).ToList();
            //        var hoeveelBommenNogOmMeHeenTeZoeken = vakje.Number - bommenOmMeHeen;


            //        var setsOmMijHeen = vakje.SurroundingVakjes.SelectMany(t => t.VakjeBerekeningen.Sets).ToList();

            //        foreach (var vakjeErOmheen in unrevealedTilesOmMeHeenZonderGuaranteedNoBom)
            //        {
            //            if (vakje.Number == 3 && vakjeErOmheen.SurroundingVakjes.Any(t => t.Number == 3) && vakjeErOmheen.SurroundingVakjes.Count(t => !t.Revealed) == 4)
            //            {

            //            }

            //            foreach (var set in setsOmMijHeen)
            //            {
            //                var tilesDieNietInSetZittenMaarWelInEromheen = unrevealedTilesOmMeHeenZonderGuaranteedNoBom.Except(set.Vakjes).ToList();


            //                //Vind set waar deze tile niet in zit
            //                if (tilesDieNietInSetZittenMaarWelInEromheen.Count > 0)
            //                {
            //                    //Zoek een set waar het aantal bommen to find hetzelfde is als de vakjes niet in die set
            //                    if (hoeveelBommenNogOmMeHeenTeZoeken - set.CountVanBommenDieErMoetenZijn == tilesDieNietInSetZittenMaarWelInEromheen.Count)
            //                    {
            //                        foreach (var tileEromheen in tilesDieNietInSetZittenMaarWelInEromheen)
            //                        {
            //                            //tileEromheen.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}
            return erIsIetsBerekend;
        }
    }
}
