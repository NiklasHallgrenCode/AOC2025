namespace AOC2025.Days
{
    public class Day01
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input);

            int position = 50;
            int password = 0;

            foreach (var line in lines)
            {
                var currentInt = int.Parse(line.Substring(1));
                if (line.StartsWith("R"))
                {
                    position += currentInt;

                    while (position > 99)
                    {
                        position -= 100;
                    }
                }
                else if (line.StartsWith("L"))
                {
                    position -= currentInt;
                    while (position < 0)
                    {
                        position += 100;
                    }
                }

                if (position == 0)
                {
                    password++;
                }
            }

            return password.ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLines(input);
            int position = 50;
            int password = 0;

            foreach (var line in lines)
            {
                var currentInt = int.Parse(line.Substring(1));

                if (line.StartsWith("R"))
                {
                    position += currentInt;

                    while (position > 99)
                    {
                        position -= 100;
                        password++;
                    }
                }
                else if (line.StartsWith("L"))
                {
                    bool posWasZero = position == 0;
                    bool hasBeenIterated = false;
                    position -= currentInt;

                    while (position < 0)
                    {
                        position += 100;
                        password++;
                        hasBeenIterated = true;
                    }

                    if (position == 0)
                    {
                        password++;
                    }
                    if (posWasZero && hasBeenIterated)
                    {
                        password--;
                    }
                }
            }

            return password.ToString();
        }
    }
}
