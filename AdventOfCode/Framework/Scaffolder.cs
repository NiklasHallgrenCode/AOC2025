using System.Text;

namespace AdventOfCode;

/// <summary>Creates empty day files in the same layout as the existing solutions.</summary>
public static class Scaffolder
{
    /// <summary>Writes <c>AOC{year}/Days/Day{dd}.cs</c> unless it already exists. Returns the file path.</summary>
    public static string CreateDay(int year, int day, out bool created)
    {
        var path = Path.Combine(InputProvider.ProjectDirectory, $"AOC{year}", "Days", $"Day{day:00}.cs");

        created = !File.Exists(path);
        if (created)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            // UTF-8 with BOM and OS line endings, like files created in Visual Studio.
            File.WriteAllText(path, Template(year, day).ReplaceLineEndings(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        return path;
    }

    private static string Template(int year, int day) => $$"""
        namespace AOC{{year}}.Days
        {
            public class Day{{day:00}}
            {
                public static string SolvePart1(string input)
                {
                    throw new NotImplementedException();
                }

                public static string SolvePart2(string input)
                {
                    throw new NotImplementedException();
                }
            }
        }

        """;
}
