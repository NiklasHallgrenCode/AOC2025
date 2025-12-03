using System.Numerics;

namespace AOC2025.Days
{
    public class Day02
    {
        public static string SolvePart1(string input)
        {
            var productRanges = Helpers.ReadLines(input, ',');
            List<BigInteger> invalidIds = new List<BigInteger>();

            foreach (var range in productRanges)
            {
                BigInteger minId = BigInteger.Parse(range.Split('-')[0]);
                BigInteger maxId = BigInteger.Parse(range.Split('-')[1]);

                for (BigInteger i = minId; i <= maxId; i++)
                {
                    var iString = i.ToString();
                    if (iString.Length % 2 > 0)
                    {
                        continue;
                    }
                    var halfLength = iString.Length / 2;
                    var firstHalf = iString.Substring(0, halfLength);
                    var secondHalf = iString.Substring(halfLength, halfLength);
                    if (firstHalf == secondHalf)
                    {
                        invalidIds.Add(i);
                    }
                }
            }
            return invalidIds.Aggregate(BigInteger.Add).ToString();
        }

        public static string SolvePart2(string input)
        {
            var productRanges = Helpers.ReadLines(input, ',');
            List<BigInteger> invalidIds = new List<BigInteger>();

            foreach (var range in productRanges)
            {
                BigInteger minId = BigInteger.Parse(range.Split('-')[0]);
                BigInteger maxId = BigInteger.Parse(range.Split('-')[1]);

                for (BigInteger i = minId; i <= maxId; i++)
                {
                    var iString = i.ToString();
                    for (int j = iString.Length; j > 1; j--)
                    {
                        List<string> numbersToCheck = new List<string>();
                        if (iString.Length % j > 0)
                        {
                            continue;
                        }

                        int currentLength = iString.Length / j;

                        for (int k = 0; k < iString.Length; k += currentLength)
                        {
                            numbersToCheck.Add(iString.Substring(k, currentLength));
                        }
                        if (numbersToCheck.TrueForAll(n => n == numbersToCheck.First()))
                        {
                            invalidIds.Add(i);
                            break;
                        }

                    }

                }
            }
            return invalidIds.Aggregate(BigInteger.Add).ToString();
        }
    }
}
