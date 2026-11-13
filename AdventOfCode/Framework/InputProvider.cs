using System.Net;
using System.Reflection;

namespace AdventOfCode;

/// <summary>An input file is missing or could not be downloaded.</summary>
public sealed class InputException(string message) : Exception(message);

/// <summary>
/// Reads puzzle inputs from <c>Inputs/{year}/day{dd}.txt</c>, downloading them from adventofcode.com
/// on first use when a session cookie is configured.
/// </summary>
public static class InputProvider
{
    // Advent of Code asks automated tools to identify themselves with a link to their source.
    private const string UserAgent = "github.com/NiklasHallgrenCode/AOC2025 (.NET Advent of Code runner)";

    public static string ProjectDirectory { get; } =
        typeof(InputProvider).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "ProjectDirectory")?.Value
        ?? AppContext.BaseDirectory;

    public static string InputsDirectory => Path.Combine(ProjectDirectory, "Inputs");

    private static string SessionFile => Path.Combine(InputsDirectory, ".session");

    public static string GetInputPath(int year, int day, bool example) =>
        Path.Combine(InputsDirectory, year.ToString(), example ? $"day{day:00}.example.txt" : $"day{day:00}.txt");

    public static async Task<string> LoadAsync(int year, int day, bool example)
    {
        var path = GetInputPath(year, day, example);

        if (!File.Exists(path))
        {
            if (example)
            {
                throw new InputException($"Example input not found. Paste the example from the puzzle into:\n  {path}");
            }

            await DownloadAsync(year, day, path);
        }

        // Passed through unchanged: solutions may rely on the file's line endings.
        return await File.ReadAllTextAsync(path);
    }

    public static async Task DownloadAsync(int year, int day, string path)
    {
        // Puzzles unlock at midnight US Eastern time (UTC-5 in December).
        var unlocksAt = new DateTimeOffset(year, 12, day, 0, 0, 0, TimeSpan.FromHours(-5));
        if (DateTimeOffset.UtcNow < unlocksAt)
        {
            throw new InputException(
                $"Puzzle {year} day {day} unlocks at {unlocksAt.ToLocalTime():yyyy-MM-dd HH:mm} (local time).");
        }

        var session = GetSessionCookie() ?? throw new InputException(
            $"Input not found: {path}\n" +
            "Save your puzzle input there, or put your adventofcode.com session cookie in the AOC_SESSION\n" +
            $"environment variable or in {SessionFile} to download inputs automatically.");

        using var client = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        client.DefaultRequestHeaders.Add("Cookie", $"session={session}");

        Console.WriteLine($"Downloading input for {year} day {day}...");
        using var response = await client.GetAsync($"{year}/day/{day}/input");

        if (!response.IsSuccessStatusCode)
        {
            throw new InputException(response.StatusCode switch
            {
                HttpStatusCode.NotFound => $"adventofcode.com has no input for {year} day {day}.",
                HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden or
                    HttpStatusCode.InternalServerError =>
                    $"Downloading the input failed ({(int)response.StatusCode}). Your session cookie is probably invalid or expired.",
                _ => $"Downloading the input failed: {(int)response.StatusCode} {response.ReasonPhrase}",
            });
        }

        // Save it the way an input pasted into Visual Studio looks: OS line endings, no trailing newline.
        var content = (await response.Content.ReadAsStringAsync()).TrimEnd('\n').ReplaceLineEndings();

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await File.WriteAllTextAsync(path, content);
    }

    private static string? GetSessionCookie()
    {
        var session = Environment.GetEnvironmentVariable("AOC_SESSION");

        if (string.IsNullOrWhiteSpace(session) && File.Exists(SessionFile))
        {
            session = File.ReadAllText(SessionFile);
        }

        session = session?.Trim();

        // Accept the cookie with or without its "session=" prefix.
        if (session is not null && session.StartsWith("session=", StringComparison.Ordinal))
        {
            session = session["session=".Length..];
        }

        return string.IsNullOrEmpty(session) ? null : session;
    }
}
