using System.Diagnostics;
using System.Numerics;

namespace AOC2025.Days
{
    public class Day07
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input);

            var lineLength = lines[0].Length;

            List<int> currentBeams = new List<int>();

            int splits = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        currentBeams.Add(j);
                    }
                    if (currentBeams.Contains(j))
                    {
                        if (lines[i][j] == '^')
                        {
                            splits++;
                            currentBeams.RemoveAll(cb => cb == j);
                            currentBeams.Add(j-1);
                            currentBeams.Add(j+1);

                        }
                    }
                }
            }

            return splits.ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLines(input);

            var lineLength = lines[0].Length;
            Dictionary<int, BigInteger> currentBeamsDict = new Dictionary<int, BigInteger>();
            double result = 1;

            for (int i = 0; i < lines.Length; i++)
            {
                BigInteger currentLineHits = 0;
                for (int j = 0; j < lineLength; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        currentBeamsDict.Add(j, 1);
                    }
                    if (currentBeamsDict.TryGetValue(j, out BigInteger currentValue) && currentValue > 0)
                    {
                        if (lines[i][j] == '^')
                        {
                            currentLineHits += currentValue;
                            if (currentBeamsDict.TryGetValue(j - 1, out BigInteger valueMinus))
                            {
                                currentBeamsDict[j - 1] = valueMinus + currentValue;
                            }
                            else
                            {
                                currentBeamsDict.Add(j - 1, currentValue);
                            }

                            if (currentBeamsDict.TryGetValue(j + 1, out BigInteger valuePlus))
                            {
                                currentBeamsDict[j + 1] = valuePlus + currentValue;
                            }
                            else
                            {
                                currentBeamsDict.Add(j + 1, currentValue);

                            }

                            currentBeamsDict[j] = 0;

                        }
                    }
                }
                if (currentLineHits > 0)
                {
                    double increment = 1 + (double)currentLineHits / result;
                    result = increment * result;
                }
                bool hasNoDecimals = result % 1 == 0;
                if (!hasNoDecimals)
                {
                    result = Math.Round(result);
                }
                currentLineHits = 0;
            }

            return result.ToString();
        }
    }

}
