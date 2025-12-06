using System.Numerics;

namespace AOC2025.Days
{
    public class Day06
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input);
            var operators = Helpers.ReadLines(lines[lines.Length - 1], " ");

            BigInteger finalSum = 0;
            for (int i = 0; i < operators.Length; i++)
            {
                List<BigInteger> numbers = new List<BigInteger>();

                for (int j = 0; j < lines.Length - 1; j++)
                {
                    var number = BigInteger.Parse(Helpers.ReadLines(lines[j], " ")[i]);
                    numbers.Add(number);
                }

                if (operators[i] == "+")
                {
                    finalSum += numbers.Aggregate(BigInteger.Add);
                }
                else if (operators[i] == "*")
                {
                    finalSum += numbers.Aggregate(BigInteger.Multiply);
                }
            }

            return finalSum.ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLinesNoTrim(input, "\r\n");

            var operators = GetOperatorWithColumns(lines[lines.Length - 1]);

            BigInteger finalSum = 0;
            int startIndex = 0;
            for (int i = 0; i < operators.Count(); i++)
            {
                List<string> numbers = new List<string>();
                for (int j = 0; j < lines.Length - 1; j++)
                {
                    var number = lines[j].Substring(startIndex, operators[i].Count());
                    numbers.Add(number);
                }
                startIndex += operators[i].Count();

                var maxLength = numbers.Max(n => n.Length);
                List<BigInteger> finalNumbers = new List<BigInteger>();

                for (int k = maxLength - 1; k > -1; k--)
                {
                    var buildNumber = "";
                    for (int l = 0; l < lines.Length - 1; l++)
                    {
                        var numberReversed = Reverse(numbers[l]);
                        if (numberReversed[k] != ' ')
                        {
                            buildNumber += numberReversed[k];
                        }
                    }
                    if(buildNumber != "")
                    {
                        finalNumbers.Add(BigInteger.Parse(buildNumber));

                    }
                }

                if (operators[i].StartsWith("+"))
                {
                    Console.WriteLine($"Add: {string.Join(" + ", finalNumbers)} = {finalNumbers.Aggregate(BigInteger.Add)}");
                    finalSum += finalNumbers.Aggregate(BigInteger.Add);
                }
                else if (operators[i].StartsWith("*"))
                {
                    Console.WriteLine($"Mul: {string.Join(" * ", finalNumbers)} = {finalNumbers.Aggregate(BigInteger.Multiply)}");
                    finalSum += finalNumbers.Aggregate(BigInteger.Multiply);
                }
            }

            return finalSum.ToString();
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static List<string> GetOperatorWithColumns(string input)
        {
            var inputList = new List<string>();
            var buildInput = input[0].ToString();
            
            for (int i = 1; i < input.Length; i++)
            {
                if(input[i] == '+' || input[i] == '*')
                {
                    inputList.Add(buildInput);
                    buildInput = "";
                    buildInput += input[i];
                }
                else
                {
                    buildInput += input[i];
                }
            }
            inputList.Add(buildInput);
            return inputList;
        }
    }
}
