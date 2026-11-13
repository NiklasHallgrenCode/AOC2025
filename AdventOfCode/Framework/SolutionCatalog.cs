using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode;

/// <summary>
/// One day's solution: a class named <c>Day{dd}</c> in an <c>AOC{year}.Days</c> namespace with
/// <c>public static SolvePart1(string input)</c> and <c>SolvePart2(string input)</c> methods.
/// A missing part method counts as not implemented.
/// </summary>
public sealed record SolutionInfo(int Year, int Day, MethodInfo? Part1, MethodInfo? Part2);

/// <summary>Finds every solution in this assembly by its namespace and class name.</summary>
public static partial class SolutionCatalog
{
    public static IReadOnlyList<SolutionInfo> All { get; } = Discover();

    public static SolutionInfo? Find(int year, int day) =>
        All.FirstOrDefault(s => s.Year == year && s.Day == day);

    public static IReadOnlyList<SolutionInfo> ForYear(int year) =>
        All.Where(s => s.Year == year).ToList();

    [GeneratedRegex(@"(^|\.)AOC(?<year>\d{4})\.Days\.Day(?<day>\d{2})$")]
    private static partial Regex SolutionName();

    private static List<SolutionInfo> Discover() =>
        typeof(SolutionCatalog).Assembly.GetTypes()
            .Select(type => (Type: type, Match: SolutionName().Match(type.FullName ?? "")))
            .Where(t => t.Match.Success)
            .Select(t => new SolutionInfo(
                int.Parse(t.Match.Groups["year"].Value),
                int.Parse(t.Match.Groups["day"].Value),
                FindPart(t.Type, "SolvePart1"),
                FindPart(t.Type, "SolvePart2")))
            .OrderBy(s => s.Year)
            .ThenBy(s => s.Day)
            .ToList();

    private static MethodInfo? FindPart(Type type, string name) =>
        type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, [typeof(string)]);
}
