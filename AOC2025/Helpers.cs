public static class Helpers
{
    public static string[] ReadLines(string input) =>
        input.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}