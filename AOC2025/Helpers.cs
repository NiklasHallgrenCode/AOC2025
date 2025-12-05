public static class Helpers
{
    public static string[] ReadLines(string input, string delimiter = "\n") =>
        input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

}