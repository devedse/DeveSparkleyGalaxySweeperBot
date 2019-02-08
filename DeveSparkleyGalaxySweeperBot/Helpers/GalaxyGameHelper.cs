using DeveSparkleyGalaxySweeperBot.Models;

namespace DeveSparkleyGalaxySweeperBot.Helpers
{
    public static class GalaxyGameHelper
    {
        public static Vakje[,] CreateVakjesArray(GalaxySweeperGame game)
        {
            Vakje[,] deVakjesArray = new Vakje[game.field.Count, game.field[0].Length];

            for (int x = 0; x < game.field.Count; x++)
            {
                var line = game.field[x];
                for (int y = 0; y < line.Length; y++)
                {
                    var character = line[y];
                    if (character != '#')
                    {
                        var vakje = new Vakje(character, x, y);
                        deVakjesArray[x, y] = vakje;
                    }
                }
            }

            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x, y - 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x + 1, y - 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x - 1, y);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x + 1, y);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x - 1, y + 1);
                        AddVakjeToNeightboursIfNotNull(vakje, deVakjesArray, x, y + 1);
                    }
                }
            }

            return deVakjesArray;
        }

        public static void AddVakjeToNeightboursIfNotNull(Vakje hetVakje, Vakje[,] deVakjesArray, int x, int y)
        {
            var hetPotentieleNeighbourVakje = GetVakjeFromArray(deVakjesArray, x, y);
            if (hetPotentieleNeighbourVakje != null)
            {
                hetVakje.SurroundingVakjes.Add(hetPotentieleNeighbourVakje);
            }
        }

        public static Vakje GetVakjeFromArray(Vakje[,] deVakjesArray, int x, int y)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            if (y >= 0 && y < height && x < width && x >= 0)
            {
                var vakje = deVakjesArray[x, y];
                if (vakje != null && vakje.Value != '#')
                {
                    return vakje;
                }
            }
            return null;
        }
    }
}
