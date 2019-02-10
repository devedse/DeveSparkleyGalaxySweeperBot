using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using DeveSparkleyGalaxySweeperBot.Stats;
using System;
using System.Diagnostics;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot
{
    public static class BommenBepaler
    {
        public static BommenBepalerStats BepaalBommenMulti(Vakje[,] deVakjesArray)
        {
            var stats = new BommenBepalerStats();

            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            bool doorGaan = true;
            int iteraties = 0;
            while (doorGaan)
            {
                var iteratie = BepaalBommen(deVakjesArray, width, height);
                iteratie.IteratieNummer = iteraties;
                stats.Iteraties.Add(iteratie);
                doorGaan = iteratie.Vondsten.Any();
                iteraties++;
            }
            Debug.WriteLine($"Totaal iteraties: {iteraties}");
            return stats;
        }

        public static BommenBepalerStatsIteratie BepaalBommen(Vakje[,] deVakjesArray, int width, int height)
        {
            var iteratie = new BommenBepalerStatsIteratie();

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
                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(unrevealed, VondstType.SimpleGuaranteedBomb));
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
                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(unrevealed, VondstType.SimpleGuaranteedNoBomb));
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


            var allSets = flatVakjes.SelectMany(t => t.VakjeBerekeningen.Sets).Distinct();
            //foreach (var vakje in alleUnrevealedDieMisschienEenBomZijn)
            //{

            foreach (var set in allSets)
            {
                if (set.CountVanBommenDieErMoetenZijn == 3)
                {

                }

                //we kijken nu per set
                foreach (var vakjeInSet in set.Vakjes)
                {

                    foreach (var setVanDeze in vakjeInSet.VakjeBerekeningen.Sets)
                    {
                        //if (set.Vakjes.Intersect(setVanDeze.Vakjes).Count() == setVanDeze.Vakjes.Count)
                        //{

                        //var vakjesNietInDezeSet = set.Vakjes.Except(setVanDeze.Vakjes).ToList();
                        //if (vakjesNietInDezeSet.Count == set.CountVanBommenDieErMoetenZijn - setVanDeze.CountVanBommenDieErMoetenZijn)
                        //{
                        //    foreach (var vakjeNietInDezeSet in vakjesNietInDezeSet)
                        //    {
                        //        if (vakjeNietInDezeSet.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                        //        {
                        //            vakjeNietInDezeSet.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                        //            erIsIetsBerekend = true;
                        //        }
                        //    }
                        //}



                        var vakjesInBeideSets = set.Vakjes.Intersect(setVanDeze.Vakjes).ToList();
                        //var bommenInVakjesInBeideNodigGezienVanuitSet = setVanDeze.CountVanBommenDieErMoetenZijn - (setVanDeze.Vakjes.Count - vakjesInBeideSets.Count);
                        var bommenInVakjesInBeideNodigGezienVanuitSet = set.CountVanBommenDieErMoetenZijn - (set.Vakjes.Count - vakjesInBeideSets.Count);

                        if (bommenInVakjesInBeideNodigGezienVanuitSet == setVanDeze.CountVanBommenDieErMoetenZijn)
                        {
                            var vakjesNietGedeeld = setVanDeze.Vakjes.Except(vakjesInBeideSets).ToList();
                            foreach (var vakjeNietGedeeld in vakjesNietGedeeld)
                            {
                                if (vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom)
                                {
                                    vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedNoBom;
                                    iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(vakjeNietGedeeld, VondstType.SetsBasedGuaranteedNoBomb));
                                }
                            }
                        }

                        //Alleen als we 2 vakjes hebben die overlappen is er de mogelijkheid dat maar 1 van de 2 een bom is
                        if (vakjesInBeideSets.Count > 1 && bommenInVakjesInBeideNodigGezienVanuitSet >= 0)
                        {
                            if (setVanDeze.Vakjes.Count - vakjesInBeideSets.Count == setVanDeze.CountVanBommenDieErMoetenZijn - bommenInVakjesInBeideNodigGezienVanuitSet)
                            {
                                var vakjesNietGedeeld = setVanDeze.Vakjes.Except(vakjesInBeideSets).ToList();
                                foreach (var vakjeNietGedeeld in vakjesNietGedeeld)
                                {
                                    if (vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                                    {
                                        vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                                        iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(vakjeNietGedeeld, VondstType.SetsBasedGuaranteedBomb));
                                    }
                                }
                            }
                        }


                        //if (0 == set.CountVanBommenDieErMoetenZijn - setVanDeze.CountVanBommenDieErMoetenZijn)
                        //{

                        //}
                        //}
                    }
                }
            }
            //}



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
            return iteratie;
        }
    }
}
