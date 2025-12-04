using System.Numerics;

namespace AOC2025.Days
{
    public class Day04
    {
        public static string SolvePart1(string input)
        {
            var lines = Helpers.ReadLines(input);
            int accessRolls = 0;
            for (int i = 0; i < lines.Length; i++)
            {

                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i].Substring(j, 1) != "@")
                    {
                        continue;
                    }
                    var adjRolls = 0;
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            //Topleft
                            AddIfRoll(lines[i - 1].Substring(j - 1, 1), ref adjRolls);
                        }
                        if (lines[i].Length != j + 1)
                        {
                            //TopRight
                            AddIfRoll(lines[i - 1].Substring(j + 1, 1), ref adjRolls);
                        }
                        //TopMid
                        AddIfRoll(lines[i - 1].Substring(j, 1), ref adjRolls);
                    }

                    if (lines.Length > i + 1)
                    {
                        if (j > 0)
                        {
                            //BottomLeft
                            AddIfRoll(lines[i + 1].Substring(j - 1, 1), ref adjRolls);
                        }
                        if (lines[i].Length != j + 1)
                        {
                            //BottomRight
                            AddIfRoll(lines[i + 1].Substring(j + 1, 1), ref adjRolls);
                        }
                        //BottomMid
                        AddIfRoll(lines[i + 1].Substring(j, 1), ref adjRolls);
                    }

                    if (j > 0)
                    {
                        //MidLeft
                        AddIfRoll(lines[i].Substring(j - 1, 1), ref adjRolls);
                    }
                    if (lines[i].Length != j + 1)
                    {
                        //BottomRight
                        AddIfRoll(lines[i].Substring(j + 1, 1), ref adjRolls);
                    }

                    if (adjRolls < 4)
                    {
                        accessRolls++;
                    }
                }

            }

            return accessRolls.ToString();
        }

        public static string SolvePart2(string input)
        {
            var lines = Helpers.ReadLines(input).ToList();
            int accessRolls = 0;
            int currentAccessRolls = -1;
            while (accessRolls != currentAccessRolls)
            {
                currentAccessRolls = accessRolls;
                for (int i = 0; i < lines.Count; i++)
                {

                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i].Substring(j, 1) != "@")
                        {
                            continue;
                        }
                        var adjRolls = 0;
                        if (i > 0)
                        {
                            if (j > 0)
                            {
                                //Topleft
                                AddIfRoll(lines[i - 1].Substring(j - 1, 1), ref adjRolls);
                            }
                            if (lines[i].Length != j + 1)
                            {
                                //TopRight
                                AddIfRoll(lines[i - 1].Substring(j + 1, 1), ref adjRolls);
                            }
                            //TopMid
                            AddIfRoll(lines[i - 1].Substring(j, 1), ref adjRolls);
                        }

                        if (lines.Count > i + 1)
                        {
                            if (j > 0)
                            {
                                //BottomLeft
                                AddIfRoll(lines[i + 1].Substring(j - 1, 1), ref adjRolls);
                            }
                            if (lines[i].Length != j + 1)
                            {
                                //BottomRight
                                AddIfRoll(lines[i + 1].Substring(j + 1, 1), ref adjRolls);
                            }
                            //BottomMid
                            AddIfRoll(lines[i + 1].Substring(j, 1), ref adjRolls);
                        }

                        if (j > 0)
                        {
                            //MidLeft
                            AddIfRoll(lines[i].Substring(j - 1, 1), ref adjRolls);
                        }
                        if (lines[i].Length != j + 1)
                        {
                            //BottomRight
                            AddIfRoll(lines[i].Substring(j + 1, 1), ref adjRolls);
                        }

                        if (adjRolls < 4)
                        {
                            accessRolls++;
                            lines[i] = lines[i].Remove(j, 1);
                            lines[i] = lines[i].Insert(j, ".");
                        }
                    }

                }
            }

            return accessRolls.ToString();
        }

        private static void AddIfRoll(string symbol, ref int adjRolls)
        {
            if (symbol == "@")
            {
                adjRolls++;
            }
        }

    }
}
