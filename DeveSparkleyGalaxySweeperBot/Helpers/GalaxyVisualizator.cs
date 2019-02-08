using DeveSparkleyGalaxySweeperBot.Models;
using System;

namespace DeveSparkleyGalaxySweeperBot.Helpers
{
    public static class GalaxyVisualizator
    {
        private const int XSpacing = 5;
        private const int YSpacing = 2;

        private const int WidthOfTxt = 88;
        private const int HeightOfTxt = 69;

        public static char[][] GenerateEmptyGalaxyString(Vakje[,] deVakjesArray)
        {
            var fackString = new char[HeightOfTxt][];
            for (int i = 0; i < HeightOfTxt; i++)
            {
                fackString[i] = new string(' ', WidthOfTxt).ToCharArray();
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
                        int xResult = (x * XSpacing) + (XSpacing / 2) + 1;
                        int yResult = (x - 8 + (2 * y)) * YSpacing + (YSpacing / 1);

                        FillCellAtThisPoint(fackString, xResult, yResult);
                    }
                }

            }

            return fackString;
        }

        public static void RenderToConsoleOud(Vakje[,] deVakjesArray)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        Console.Write(vakje.Value + " ");
                    }
                    else
                    {
                        Console.Write("# ");
                    }
                }

                Console.WriteLine();
            }
        }


        public static void RenderToConsole(Vakje[,] deVakjesArray)
        {
            int width = deVakjesArray.GetLength(0);
            int height = deVakjesArray.GetLength(1);

            var fackString = GenerateEmptyGalaxyString(deVakjesArray);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vakje = deVakjesArray[x, y];

                    if (vakje != null)
                    {
                        int xResult = (x * XSpacing) + (XSpacing / 2) + 1;
                        int yResult = (x - 8 + (2 * y)) * YSpacing + (YSpacing / 1);

                        if (vakje.VakjeBerekeningen.BerekendVakjeType == BerekendVakjeType.GuaranteedBom)
                        {
                            fackString[yResult][xResult] = 'G';
                        }
                        else
                        {
                            fackString[yResult][xResult] = vakje.Value;
                        }
                    }
                }
            }

            for (int i = 0; i < HeightOfTxt; i++)
            {
                var curString = fackString[i];
                for (int x = 0; x < curString.Length; x++)
                {
                    var curChar = curString[x];
                    if (curChar == 'G')
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write(curString[x]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (curChar == '.')
                    {
                        Console.Write(curString[x]);
                    }
                    else if (char.IsDigit(curChar))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(curString[x]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (curChar == 'R' || curChar == 'B')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(curString[x]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.Write(curString[x]);
                    }
                }
                Console.WriteLine();
            }
        }

        public static void FillCellAtThisPoint(char[][] fackString, int xResult, int yResult)
        {
            fackString[yResult][xResult - 3] = '/';
            fackString[yResult - 1][xResult - 2] = '/';
            fackString[yResult - 2][xResult - 1] = '_';
            fackString[yResult - 2][xResult] = '_';
            fackString[yResult - 2][xResult + 1] = '_';
            fackString[yResult - 1][xResult + 2] = '\\';
            fackString[yResult][xResult + 3] = '\\';
            fackString[yResult + 1][xResult + 3] = '/';
            fackString[yResult + 2][xResult + 2] = '/';
            fackString[yResult + 2][xResult + 1] = '_';
            fackString[yResult + 2][xResult] = '_';
            fackString[yResult + 2][xResult - 1] = '_';
            fackString[yResult + 2][xResult - 2] = '\\';
            fackString[yResult + 1][xResult - 3] = '\\';
        }
    }
}
