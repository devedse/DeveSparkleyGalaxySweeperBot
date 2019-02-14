using DeveSparkleyGalaxySweeperBot.Config;
using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using DeveSparkleyGalaxySweeperBot.Stats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot
{
    public static class BommenBepaler
    {
        private static void AddSet(BommenBepalerStatsIteratie iteratie, int bommenErIn, List<Vakje> vakjes)
        {
            var set = new VakjeSet(bommenErIn, vakjes);
            foreach (var unrevealed in vakjes)
            {
                unrevealed.VakjeBerekeningen.Sets.Add(set);
            }
        }

        private static bool AddSetDeluxe(List<VakjeSetDeluxe> alleSets, BommenBepalerStatsIteratie iteratie, int minCountBommen, int minCountNietBommen, List<Vakje> vakjes)
        {
            var newSet = new VakjeSetDeluxe(minCountBommen, minCountNietBommen, vakjes);

            var matchingSets = alleSets.Where(t => SetComparer.SetsAreEqual(newSet, t)).ToList();

            if (matchingSets.Count > 1)
            {
                throw new InvalidOperationException("Should never happen");
            }
            else if (matchingSets.Count == 0)
            {
                alleSets.Add(newSet);
                foreach (var unrevealed in vakjes)
                {
                    unrevealed.VakjeBerekeningen.SetsDeluxe.Add(newSet);
                }
                return true;
            }
            else
            {
                var setExisting = matchingSets.First();
                bool changed = false;

                var newMinGuaranteed = Math.Max(setExisting.MinCountGuaranteedBombs, newSet.MinCountGuaranteedBombs);
                var newMinGuaranteedNot = Math.Max(setExisting.MinCountGuaranteedNotBombs, newSet.MinCountGuaranteedNotBombs);

                if (setExisting.MinCountGuaranteedBombs != newMinGuaranteed)
                {
                    changed = true;
                    setExisting.MinCountGuaranteedBombs = newMinGuaranteed;
                }

                if (setExisting.MinCountGuaranteedNotBombs != newMinGuaranteedNot)
                {
                    changed = true;
                    setExisting.MinCountGuaranteedNotBombs = newMinGuaranteedNot;
                }

                if (setExisting.MinCountGuaranteedBombs + setExisting.MinCountGuaranteedNotBombs > setExisting.Vakjes.Count)
                {
                    throw new InvalidOperationException("Dit kan ook niet");
                }
                return changed;
            }


        }

        public static BommenBepalerStats BepaalBommenMulti2(Vakje[,] deVakjesArray, BotConfig botConfig)
        {
            var stats = new BommenBepalerStats();

            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            var initialIteratie = new BommenBepalerStatsIteratie();
            stats.Iteraties.Add(initialIteratie);

            var flatVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null);
            foreach (var vakje in flatVakjes)
            {
                if (vakje.IsNumber)
                {
                    var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed).ToList();
                    var revealedBommenOmMeHeen = vakje.SurroundingVakjes.Where(t => t.IsBomb).ToList();
                    var revealedNietBommenOmMeHeen = vakje.SurroundingVakjes.Where(t => t.IsNumber).ToList();

                    var bommenInDitSet = vakje.Number - revealedBommenOmMeHeen.Count;
                    var nietBommenInDitSet = unrevealedTilesOmMeHeen.Count - bommenInDitSet;
                    AddSetDeluxe(new List<VakjeSetDeluxe>(), initialIteratie, bommenInDitSet, nietBommenInDitSet, unrevealedTilesOmMeHeen);
                }
            }

            bool doorGaan = true;
            int iteraties = 1;
            while (doorGaan)
            {
                var iteratie = BepaalBommen2(deVakjesArray, width, height, botConfig);
                iteratie.IteratieNummer = iteraties;
                stats.Iteraties.Add(iteratie);
                doorGaan = iteratie.Vondsten.Any();
                iteraties++;
            }



            var allSets = flatVakjes.SelectMany(t => t.VakjeBerekeningen.SetsDeluxe).Distinct().ToList();
            foreach (var set in allSets)
            {
                if (set.MinCountGuaranteedBombs == set.Vakjes.Count)
                {
                    foreach (var vakje in set.Vakjes)
                    {
                        if (vakje.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedNoBom)
                        {
                            throw new InvalidOperationException("Kan niet");
                        }
                        else if (vakje.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.Unknown)
                        {
                            vakje.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                        }
                        else
                        {
                            //Wisten we al
                        }
                    }
                }

                if (set.MinCountGuaranteedNotBombs == set.Vakjes.Count)
                {
                    foreach (var vakje in set.Vakjes)
                    {
                        if (vakje.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)
                        {
                            throw new InvalidOperationException("Kan niet");
                        }
                        else if (vakje.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.Unknown)
                        {
                            vakje.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedNoBom;
                        }
                        else
                        {
                            //Wisten we al
                        }
                    }
                }
            }


            Debug.WriteLine($"Totaal iteraties: {iteraties}");
            return stats;
        }



        public static BommenBepalerStatsIteratie BepaalBommen2(Vakje[,] deVakjesArray, int width, int height, BotConfig botConfig)
        {
            var iteratie = new BommenBepalerStatsIteratie();

            var flatVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null);
            var allSets = flatVakjes.SelectMany(t => t.VakjeBerekeningen.SetsDeluxe).Distinct().ToList();


            var setsToAdd = new List<VakjeSetDeluxe>();

            foreach (var set in allSets)
            {
                var alleOverlappendeSets = set.Vakjes.SelectMany(t => t.VakjeBerekeningen.SetsDeluxe).Distinct().ToList();

                var filledIntersections = SetBepaler.BepaalSets(set, alleOverlappendeSets);

                foreach (var filledIntersection in filledIntersections)
                {
                    var totGuaranteedNotBombs = filledIntersection.Sum(t => t.MinCountGuaranteedNotBombsInIntersection);
                    if (totGuaranteedNotBombs == set.Vakjes.Count - set.MinCountGuaranteedBombs)
                    {
                        //Shouldn't be required but we're now sure that the max count of bombs here is in fact this
                        //set.MinCountGuaranteedNotBombs = Math.Min(set.Vakjes.Count - set.MinCountGuaranteedBombs, set.MinCountGuaranteedNotBombs);

                        foreach (var intersectionSet in filledIntersection)
                        {
                            var guaranteedBommenHier = intersectionSet.Intersection.Count - intersectionSet.MinCountGuaranteedNotBombsInIntersection;
                            var newSet = new VakjeSetDeluxe(guaranteedBommenHier, intersectionSet.MinCountGuaranteedNotBombsInIntersection, intersectionSet.Intersection);
                            setsToAdd.Add(newSet);
                        }
                    }

                    foreach (var intersectionSet in filledIntersection)
                    {
                        var theRest = filledIntersection.Except(new List<IntersectionAndSet>() { intersectionSet }).ToList();
                        var bommenInTheRest = theRest.Sum(t => t.MinCountGuaranteedBombsInIntersection);
                        if (set.Vakjes.Count - set.MinCountGuaranteedNotBombs == bommenInTheRest)
                        {
                            var newSet = new VakjeSetDeluxe(0, intersectionSet.Intersection.Count, intersectionSet.Intersection);
                            setsToAdd.Add(newSet);
                        }
                    }

                }
            }


            foreach (var newSet in setsToAdd)
            {
                var changed = AddSetDeluxe(allSets, iteratie, newSet.MinCountGuaranteedBombs, newSet.MinCountGuaranteedNotBombs, newSet.Vakjes);
                if (changed)
                {
                    iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(iteratie, newSet.Vakjes.First(), VondstType.SetsBasedGuaranteedBomb));
                }
            }
            return iteratie;

        }



        public static BommenBepalerStats BepaalBommenMulti(Vakje[,] deVakjesArray, BotConfig botConfig)
        {
            var stats = new BommenBepalerStats();

            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            bool doorGaan = true;
            int iteraties = 0;
            while (doorGaan)
            {
                var iteratie = BepaalBommen(deVakjesArray, width, height, botConfig);
                iteratie.IteratieNummer = iteraties;
                stats.Iteraties.Add(iteratie);
                doorGaan = iteratie.Vondsten.Any();
                iteraties++;
            }
            Debug.WriteLine($"Totaal iteraties: {iteraties}");
            return stats;
        }

        public static BommenBepalerStatsIteratie BepaalBommen(Vakje[,] deVakjesArray, int width, int height, BotConfig botConfig)
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
                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(iteratie, unrevealed, VondstType.SimpleGuaranteedBomb));
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
                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(iteratie, unrevealed, VondstType.SimpleGuaranteedNoBomb));
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

                    AddSet(iteratie, vakje.Number - bommenOmMeHeen, unrevealedTilesOmMeHeenZonderGuaranteedNoBom);
                }
            }

            if (botConfig.UseSetDetection)
            {
                var allSets = flatVakjes.SelectMany(t => t.VakjeBerekeningen.Sets).Distinct();

                foreach (var set in allSets)
                {
                    //we kijken nu per set
                    foreach (var vakjeInSet in set.Vakjes)
                    {

                        foreach (var setVanDeze in vakjeInSet.VakjeBerekeningen.Sets)
                        {


                            if (set != setVanDeze)
                            {
                                var vakjesInBeideSets = set.Vakjes.Intersect(setVanDeze.Vakjes).ToList();
                                var bommenInVakjesInBeideNodigGezienVanuitSet = set.CountVanBommenDieErMoetenZijn - (set.Vakjes.Count - vakjesInBeideSets.Count);
                                var countGuaranteedNotBombsInIntersection = vakjesInBeideSets.Count - set.CountVanBommenDieErMoetenZijn;

                                if (bommenInVakjesInBeideNodigGezienVanuitSet == setVanDeze.CountVanBommenDieErMoetenZijn)
                                {
                                    var vakjesNietGedeeld = setVanDeze.Vakjes.Except(vakjesInBeideSets).ToList();
                                    foreach (var vakjeNietGedeeld in vakjesNietGedeeld)
                                    {
                                        if (vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedNoBom)
                                        {
                                            vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedNoBom;
                                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(iteratie, vakjeNietGedeeld, VondstType.SetsBasedGuaranteedNoBomb));
                                        }
                                    }
                                }

                                //Alleen als we 2 vakjes hebben die overlappen is er de mogelijkheid dat maar 1 van de 2 een bom is
                                //if (vakjesInBeideSets.Count > 1 && bommenInVakjesInBeideNodigGezienVanuitSet > 0)
                                //{
                                //if (setVanDeze.CountVanBommenDieErMoetenZijn - countGuaranteedNotBombsInIntersection == setVanDeze.Vakjes.Count - vakjesInBeideSets.Count)
                                if (setVanDeze.Vakjes.Count - countGuaranteedNotBombsInIntersection == setVanDeze.CountVanBommenDieErMoetenZijn)
                                {
                                    var vakjesNietGedeeld = setVanDeze.Vakjes.Except(vakjesInBeideSets).ToList();
                                    foreach (var vakjeNietGedeeld in vakjesNietGedeeld)
                                    {
                                        if (vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType != BerekendVakjeType.GuaranteedBom)
                                        {
                                            vakjeNietGedeeld.VakjeBerekeningen.BerekendVakjeType = BerekendVakjeType.GuaranteedBom;
                                            iteratie.Vondsten.Add(new BommenBepalerStatsIteratieVondst(iteratie, vakjeNietGedeeld, VondstType.SetsBasedGuaranteedBomb));
                                        }
                                    }
                                }
                                //}
                            }
                        }
                    }
                }
            }
            return iteratie;
        }
    }
}
