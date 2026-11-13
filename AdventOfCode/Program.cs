using AdventOfCode;

const string Usage = """
    Usage:
      dotnet run                        Run the most recent solution
      dotnet run -- <day>               Run a day from the most recent year
      dotnet run -- <year> <day>        Run one day
      dotnet run -- <year> [all]        Run every solution for a year
      dotnet run -- new <year> <day>    Create AOC<year>/Days/Day<dd>.cs and download its input

    Options:
      -e, --example    Use Inputs/<year>/day<dd>.example.txt instead of your puzzle input
      -h, --help       Show this help

    Inputs are read from Inputs/<year>/day<dd>.txt. Missing inputs are downloaded automatically when
    your adventofcode.com session cookie is in the AOC_SESSION environment variable or Inputs/.session.
    """;

var options = args.Where(a => a.StartsWith('-')).ToHashSet();
var positional = args.Where(a => !a.StartsWith('-')).ToArray();

if (options.Remove("-h") | options.Remove("--help"))
{
    Console.WriteLine(Usage);
    return 0;
}

bool useExample = options.Remove("-e") | options.Remove("--example");

try
{
    if (options.Count > 0)
    {
        throw new UsageException($"Unknown option: {string.Join(", ", options)}");
    }

    return positional switch
    {
        [] => await RunLatestAsync(),
        ["new", var year, var day] => await CreateDayAsync(ParseYear(year), ParseDay(day)),
        [var year] when IsYear(year) => await RunYearAsync(ParseYear(year)),
        [var day] => await RunDayAsync(LatestYear(), ParseDay(day)),
        [var year, "all"] => await RunYearAsync(ParseYear(year)),
        [var year, var day] => await RunDayAsync(ParseYear(year), ParseDay(day)),
        _ => throw new UsageException($"Unexpected arguments: {string.Join(' ', args)}"),
    };
}
catch (UsageException ex)
{
    PuzzleRunner.WriteError(ex.Message);
    Console.Error.WriteLine();
    Console.Error.WriteLine(Usage);
    return 1;
}

async Task<int> RunLatestAsync()
{
    if (SolutionCatalog.All.Count == 0)
    {
        throw new UsageException("No solutions found yet.");
    }

    var latest = SolutionCatalog.All[^1];
    return await RunDayAsync(latest.Year, latest.Day);
}

async Task<int> RunDayAsync(int year, int day)
{
    var solution = SolutionCatalog.Find(year, day);
    if (solution is null)
    {
        PuzzleRunner.WriteError($"There is no solution for {year} day {day} yet. Create one with: dotnet run -- new {year} {day}");
        return 1;
    }

    var result = await PuzzleRunner.RunAsync(solution, useExample);
    return result.Succeeded ? 0 : 1;
}

async Task<int> RunYearAsync(int year)
{
    var solutions = SolutionCatalog.ForYear(year);
    if (solutions.Count == 0)
    {
        PuzzleRunner.WriteError($"There are no solutions for {year} yet. Start with: dotnet run -- new {year} 1");
        return 1;
    }

    var total = TimeSpan.Zero;
    var failures = 0;
    foreach (var solution in solutions)
    {
        var result = await PuzzleRunner.RunAsync(solution, useExample);
        total += result.Elapsed;
        failures += result.Succeeded ? 0 : 1;
        Console.WriteLine();
    }

    Console.WriteLine($"{solutions.Count} days, {failures} failed, total {PuzzleRunner.Format(total)}");
    return failures == 0 ? 0 : 1;
}

async Task<int> CreateDayAsync(int year, int day)
{
    var path = Scaffolder.CreateDay(year, day, out var created);
    Console.WriteLine(created ? $"Created {path}" : $"{path} already exists.");

    if (!File.Exists(InputProvider.GetInputPath(year, day, example: false)))
    {
        try
        {
            await InputProvider.DownloadAsync(year, day, InputProvider.GetInputPath(year, day, example: false));
            Console.WriteLine("Input downloaded.");
        }
        catch (Exception ex) when (ex is InputException or HttpRequestException)
        {
            Console.WriteLine(ex.Message);
        }
    }

    Console.WriteLine($"Run it with: dotnet run -- {year} {day}");
    return 0;
}

int LatestYear() =>
    SolutionCatalog.All.Count > 0 ? SolutionCatalog.All[^1].Year : throw new UsageException("No solutions found yet.");

static bool IsYear(string value) => int.TryParse(value, out var year) && year >= 2015;

static int ParseYear(string value) =>
    int.TryParse(value, out var year) && year >= 2015 && year <= 9999
        ? year
        : throw new UsageException($"'{value}' is not a valid Advent of Code year.");

static int ParseDay(string value) =>
    int.TryParse(value, out var day) && day is >= 1 and <= 25
        ? day
        : throw new UsageException($"'{value}' is not a valid day (1-25).");

sealed class UsageException(string message) : Exception(message);
