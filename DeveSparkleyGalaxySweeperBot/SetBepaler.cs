﻿using DeveSparkleyGalaxySweeperBot.Helpers;
using DeveSparkleyGalaxySweeperBot.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DeveSparkleyGalaxySweeperBot
{
    public static class SetBepaler
    {
        public static List<List<IntersectionAndSet>> BepaalSets(VakjeSetDeluxe deOverallSet, List<VakjeSetDeluxe> lijstMetAlleSets)
        {
            var foundItems = BepaalSets(deOverallSet, lijstMetAlleSets, new List<IntersectionAndSet>());
            var result = EnsureNoDuplicates(foundItems);
            return result;
        }

        private static List<List<IntersectionAndSet>> BepaalSets(VakjeSetDeluxe deOverallSet, List<VakjeSetDeluxe> lijstMetAlleSets, List<IntersectionAndSet> current)
        {
            Debug.WriteLine("Iteratie");

            if (current.SelectMany(t => t.VakjeSetDeluxe.Vakjes).Count() == deOverallSet.Vakjes.Count)
            {
                return new List<List<IntersectionAndSet>>() { current };
            }

            var allItems = new List<List<IntersectionAndSet>>();
            foreach (var set in lijstMetAlleSets)
            {
                var intersection = deOverallSet.Vakjes.Intersect(set.Vakjes).ToList();
                var vrijeVakjesOver = deOverallSet.Vakjes.Except(current.SelectMany(t => t.VakjeSetDeluxe.Vakjes)).ToList();

                if (vrijeVakjesOver.Intersect(intersection).Count() == intersection.Count)
                {
                    //Hij past er nog in
                    var clone = current.ToList();
                    clone.Add(new IntersectionAndSet(intersection, set));
                    var foundItems = BepaalSets(deOverallSet, lijstMetAlleSets, clone);
                    allItems.AddRange(foundItems);
                }
            }
            return allItems;
        }

        private static List<List<IntersectionAndSet>> EnsureNoDuplicates(List<List<IntersectionAndSet>> setGroupsToAdd)
        {
            var setGroups = new List<List<IntersectionAndSet>>();
            foreach (var itemToAdd in setGroupsToAdd)
            {
                var matchingSets = setGroups.Where(t => SetComparer.MultipleSetsAreEqual(t.Select(z => z.VakjeSetDeluxe).ToList(), itemToAdd.Select(g => g.VakjeSetDeluxe).ToList())).ToList();
                if (matchingSets.Count == 0)
                {
                    setGroups.Add(itemToAdd);
                }
                else
                {
                    Debug.WriteLine("Item already exists");
                }
            }
            return setGroups;
        }
    }
}
