using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

public sealed record RunResult(bool Succeeded, TimeSpan Elapsed);

/// <summary>Runs both parts of a solution, timing each and reporting failures.</summary>
public static class PuzzleRunner
{
    public static async Task<RunResult> RunAsync(SolutionInfo solution, bool example)
    {
        Console.WriteLine($"Advent of Code {solution.Year} - Day {solution.Day:00}{(example ? " (example input)" : "")}");

        string input;
        try
        {
            input = await InputProvider.LoadAsync(solution.Year, solution.Day, example);
        }
        catch (Exception ex) when (ex is InputException or HttpRequestException)
        {
            WriteError(ex.Message);
            return new RunResult(false, TimeSpan.Zero);
        }

        var part1 = RunPart(1, solution.Part1, input);
        var part2 = RunPart(2, solution.Part2, input);

        return new RunResult(part1.Succeeded && part2.Succeeded, part1.Elapsed + part2.Elapsed);
    }

    private static RunResult RunPart(int part, MethodInfo? solve, string input)
    {
        if (solve is null)
        {
            Console.WriteLine($"  Part {part}: not implemented");
            return new RunResult(true, TimeSpan.Zero);
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            var answer = solve.Invoke(null, BindingFlags.DoNotWrapExceptions, binder: null, [input], culture: null);
            stopwatch.Stop();
            Console.WriteLine($"  Part {part}: {answer,-24} ({Format(stopwatch.Elapsed)})");
            return new RunResult(true, stopwatch.Elapsed);
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"  Part {part}: not implemented");
            return new RunResult(true, TimeSpan.Zero);
        }
        // With a debugger attached the exception is left unhandled, so the debugger stops at the failing line.
        catch (Exception ex) when (!Debugger.IsAttached)
        {
            WriteError($"  Part {part} failed: {ex}");
            return new RunResult(false, stopwatch.Elapsed);
        }
    }

    public static string Format(TimeSpan elapsed) =>
        elapsed.TotalSeconds >= 1 ? $"{elapsed.TotalSeconds:F2} s" : $"{elapsed.TotalMilliseconds:F1} ms";

    public static void WriteError(string message)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(message);
        Console.ForegroundColor = previous;
    }
}
