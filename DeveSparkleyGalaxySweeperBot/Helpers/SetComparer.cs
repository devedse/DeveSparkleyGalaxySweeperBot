using DeveSparkleyGalaxySweeperBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Helpers
{
    public static class SetComparer
    {
        public static bool SetsAreEqual(VakjeSetDeluxe set1, VakjeSetDeluxe set2)
        {
            if (set1.Vakjes.Count != set2.Vakjes.Count)
            {
                return false;
            }

            //if (set1.MinCountGuaranteedBombs != set2.MinCountGuaranteedBombs)
            //{
            //    return false;
            //}

            //if (set1.MinCountGuaranteedNotBombs != set2.MinCountGuaranteedNotBombs)
            //{
            //    return false;
            //}

            var intersection = set1.Vakjes.Intersect(set2.Vakjes);
            if (intersection.Count() != set1.Vakjes.Count)
            {
                return false;
            }
            return true;
        }

        public static bool MultipleSetsAreEqual(List<VakjeSetDeluxe> setsList1, List<VakjeSetDeluxe> setsList2)
        {
            if (setsList1.Count != setsList2.Count)
            {
                return false;
            }

            foreach(var set in setsList1)
            {
                var timesFound = setsList2.Count(t => SetsAreEqual(t, set));
                if (timesFound != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
