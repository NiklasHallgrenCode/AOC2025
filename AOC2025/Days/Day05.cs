using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2025.Days
{
    public class Day05
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input, "\r\n\r\n");
            var ingredientRanges = Helpers.ReadLines(lines[0]); 
            var ingredients = Helpers.ReadLines(lines[1]);

            var numberOfFreshIngredients = 
                ingredients.Where(i => ingredientRanges
                .Any(ir => BigInteger.Parse(i) >= BigInteger.Parse(ir.Split('-')[0]) 
                    && BigInteger.Parse(i) <= BigInteger.Parse(ir.Split('-')[1])))
                .Count();

            return numberOfFreshIngredients.ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLines(input, "\r\n\r\n");
            var ingredientRanges = Helpers.ReadLines(lines[0]);

            var idRanges = ingredientRanges.Select(i => (BigInteger.Parse(i.Split("-")[0]), BigInteger.Parse(i.Split("-")[1]))).ToList();

            bool noAdds = false;
            while (true)
            {
                noAdds = true;
                for (int i = 0; i < idRanges.Count(); i++)
                {
                    if (idRanges[i].Item1 == 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < idRanges.Count(); j++)
                    {
                        if(i == j)
                        {
                            continue;
                        }
                        if (idRanges[j].Item1 == 0)
                        {
                            continue;
                        }
                        if (IsOverlapping(idRanges[i].Item1, idRanges[i].Item2, idRanges[j].Item1, idRanges[j].Item2))
                        {
                            idRanges.Add((BigInteger.Min(idRanges[i].Item1, idRanges[j].Item1), BigInteger.Max(idRanges[i].Item2, idRanges[j].Item2)));
                            idRanges[i] = (0, 0);
                            idRanges[j] = (0, 0);
                            noAdds = false;
                            break;
                        }
                        if (!noAdds)
                        {
                            break;
                        }
                    }
                }
                idRanges.RemoveAll(i => i == (0, 0));
                if (noAdds)
                {
                    break;
                }
            }

            BigInteger sum = 0;
            foreach (var range in idRanges)
            {
                sum += range.Item2 - range.Item1 + 1;
                
            }

            return sum.ToString();
        }

        private static bool IsOverlapping(BigInteger x1, BigInteger x2, BigInteger y1, BigInteger y2)
        {
            return BigInteger.Max(x1, y1) <= BigInteger.Min(x2, y2);
        }
    }
}
