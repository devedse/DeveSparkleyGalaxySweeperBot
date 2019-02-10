using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using DeveSparkleyGalaxySweeperBot.Stats;
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

        private static void AddSetDeluxe(BommenBepalerStatsIteratie iteratie, int minCountBommen, int minCountNietBommen, List<Vakje> vakjes)
        {
            var set = new VakjeSetDeluxe(minCountBommen, minCountNietBommen, vakjes);
            foreach (var unrevealed in vakjes)
            {
                unrevealed.VakjeBerekeningen.SetsDeluxe.Add(set);
            }
        }

        //public static BommenBepalerStats BepaalBommenMulti2(Vakje[,] deVakjesArray)
        //{
        //    var stats = new BommenBepalerStats();

        //    int width = deVakjesArray.GetLength(0);
        //    int height = deVakjesArray.GetLength(1);

        //    var initialIteratie = new BommenBepalerStatsIteratie();
        //    stats.Iteraties.Add(initialIteratie);

        //    var flatVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null);
        //    foreach (var vakje in flatVakjes)
        //    {
        //        if (vakje.IsNumber)
        //        {
        //            var unrevealedTilesOmMeHeen = vakje.SurroundingVakjes.Where(t => !t.Revealed).ToList();

        //            AddSet(initialIteratie, vakje.Number, unrevealedTilesOmMeHeen);
        //        }
        //    }

        //    bool doorGaan = true;
        //    int iteraties = 1;
        //    while (doorGaan)
        //    {
        //        var iteratie = BepaalBommen2(deVakjesArray, width, height);
        //        iteratie.IteratieNummer = iteraties;
        //        stats.Iteraties.Add(iteratie);
        //        doorGaan = iteratie.Vondsten.Any();
        //        iteraties++;
        //    }
        //    Debug.WriteLine($"Totaal iteraties: {iteraties}");
        //    return stats;
        //}



        //public static BommenBepalerStatsIteratie BepaalBommen2(Vakje[,] deVakjesArray, int width, int height)
        //{
        //    var iteratie = new BommenBepalerStatsIteratie();

        //    var flatVakjes = TwoDimensionalArrayHelper.Flatten(deVakjesArray).Where(t => t != null);
        //    var allSets = flatVakjes.SelectMany(t => t.VakjeBerekeningen.SetsDeluxe).Distinct();

        //    foreach (var set in allSets)
        //    {
        //        foreach (var setVanDeze in allSets)
        //        {
        //            //var vakjesOver = n

        //            if (set != setVanDeze)
        //            {

        //                var vakjesInBeideSets = set.Vakjes.Intersect(setVanDeze.Vakjes).ToList();

        //                if (vakjesInBeideSets.Count > 0)
        //                {
        //                    var vakjesInSetDieNietInSetVanDezeZitten = set.Vakjes.Except(setVanDeze.Vakjes).ToList();

        //                    var minCountGuaranteedBombsInIntersection = set.MinCountGuaranteedBombs - vakjesInSetDieNietInSetVanDezeZitten.Count;
        //                    var minCountGuaranteedNotBombsInIntersection = vakjesInBeideSets.Count - set.MinCountGuaranteedBombs;

        //                    //if (vakjesInSetDieNietInSetVanDezeZitten.Count == 0)
        //                    if (minCountGuaranteedBombsInIntersection > 0)
        //                    {
        //                        AddSetDeluxe(iteratie, setVanDeze.MinCountGuaranteedBombs - minCountGuaranteedBombsInIntersection, 0, vakjesInSetDieNietInSetVanDezeZitten);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    foreach (var set in allSets)
        //    {
        //        //we kijken nu per set
        //        foreach (var vakjeInSet in set.Vakjes)
        //        {
        //            foreach (var setVanDeze in vakjeInSet.VakjeBerekeningen.Sets)
        //            {
        //                if (set != setVanDeze)
        //                {
        //                    var vakjesInBeideSets = set.Vakjes.Intersect(setVanDeze.Vakjes).ToList();
        //                    var countGuaranteedBombsInIntersection = set.CountVanBommenDieErMoetenZijn - (set.Vakjes.Count - vakjesInBeideSets.Count);
        //                    var countGuaranteedNotBombsInIntersection = vakjesInBeideSets.Count - set.CountVanBommenDieErMoetenZijn;


        //                }
        //            }
        //        }
        //    }

        //    return iteratie;
        //}





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
            return iteratie;
        }
    }
}
