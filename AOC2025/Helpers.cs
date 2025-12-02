public static class Helpers
{
    public static string[] ReadLines(string input, char delimiter = '\n') =>
        input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}