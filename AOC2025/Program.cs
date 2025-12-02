using AOC2025.Days;


int day = args.Length > 0 && int.TryParse(args[0], out var d) ? d : 2;

Console.WriteLine($"Advent of Code 2025 - Day {day:00}");

string inputPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Inputs", $"day{day:00}.txt");
if (!File.Exists(inputPath))
{
    Console.WriteLine($"Input file not found: {inputPath}");
    return;
}

string input = File.ReadAllText(inputPath);

string part1 = string.Empty;
string part2 = string.Empty;

switch (day)
{
    case 1:
        part1 = Day01.SolvePart1(input);
        part2 = Day01.SolvePart2(input);
        break;
    case 2:
        part1 = Day02.SolvePart1(input);
        part2 = Day02.SolvePart2(input);
        break;

    default:
        Console.WriteLine($"Day {day} is not implemented yet.");
        return;
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");


