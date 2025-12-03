using System.Numerics;

namespace AOC2025.Days
{
    public class Day03
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input);
            List<int> joltage = new List<int>();
            foreach (var line in lines)
            {
                int number1 = 0;
                int number2 = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    var number = line.Substring(i, 1);
                    var numberAsInt = int.Parse(number);
                    if(numberAsInt > number1 && i < line.Length - 1)
                    {
                        number1 = numberAsInt;
                        number2 = 0;
                    }
                    else if (numberAsInt > number2)
                    {
                        number2 = numberAsInt;
                    }
                }

                joltage.Add(int.Parse(number1.ToString() + number2.ToString()));

            }

            return joltage.Sum().ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLines(input);
            List<BigInteger> joltage = new List<BigInteger>();
            foreach (var line in lines)
            {
                List<int> jNumbers = new List<int>();
                

                for (int i = 0; i < line.Length; i++)
                {
                    var number = line.Substring(i, 1);
                    var numberAsInt = int.Parse(number);
                    for (int j = 0; j < 12; j++)
                    {
                        if(j + 1 > jNumbers.Count)
                        {
                            jNumbers.Add(numberAsInt);
                            break;
                        }
                        if (numberAsInt > jNumbers[j] && line.Length - i >= 12 - j)
                        {
                            jNumbers.RemoveRange(j, jNumbers.Count - j);
                            jNumbers.Insert(j, numberAsInt);
                            break;
                        }
                    }
                }

                joltage.Add(BigInteger.Parse(string.Join("", jNumbers)));
            }

            return joltage.Aggregate(BigInteger.Add).ToString();
        }
    }
}
