using System;
using System.Collections.Generic;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class IntersectionAndSet
    {
        public List<Vakje> Intersection { get; }
        public VakjeSetDeluxe VakjeSetDeluxe { get; }

        public IntersectionAndSet(List<Vakje> intersection, VakjeSetDeluxe vakjeSetDeluxe)
        {
            Intersection = intersection;
            VakjeSetDeluxe = vakjeSetDeluxe;
        }

        public int MinCountGuaranteedBombsInIntersection => Math.Max(0, Intersection.Count - (VakjeSetDeluxe.Vakjes.Count - VakjeSetDeluxe.MinCountGuaranteedBombs));
        public int MinCountGuaranteedNotBombsInIntersection => Math.Max(0, Intersection.Count - (VakjeSetDeluxe.Vakjes.Count - VakjeSetDeluxe.MinCountGuaranteedNotBombs));
    }
}
