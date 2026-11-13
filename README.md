# Advent of Code

My [Advent of Code](https://adventofcode.com/) solutions in C# (.NET 8), one folder per year.

```
AdventOfCode.sln
AdventOfCode/
├── AOC2025/Days/Day01.cs …    2025 solutions
├── AOC2026/Days/Day01.cs …    2026 solutions
├── Inputs/2025/day01.txt …    puzzle inputs (git-ignored)
├── Framework/                 runner: finds solutions, loads/downloads inputs, times each part
├── Helpers.cs
└── Program.cs                 command line
```

## Running

From the `AdventOfCode` folder:

| Command | Runs |
| --- | --- |
| `dotnet run` | the most recent day |
| `dotnet run -- 7` | day 7 of the most recent year |
| `dotnet run -- 2025 7` | 2025 day 7 |
| `dotnet run -- 2025` | every day of 2025, with a total time |
| `dotnet run -- 2025 7 --example` | 2025 day 7 on `Inputs/2025/day07.example.txt` |
| `dotnet run -- new 2026 2` | creates `AOC2026/Days/Day02.cs` and downloads its input |

In Visual Studio, F5 runs the most recent day. Pick the **Latest day (example input)** launch profile to run it on the example input instead.

Each part's answer is printed with its run time. A part that throws `NotImplementedException` is reported as "not implemented". When a debugger is attached, other exceptions are left unhandled, so the debugger stops on the failing line.

## Adding a day

Run `dotnet run -- new <year> <day>`, or add the file yourself. The runner finds any class named `DayNN` in an `AOC<year>.Days` namespace with these methods:

```csharp
namespace AOC2026.Days
{
    public class Day01
    {
        public static string SolvePart1(string input) { ... }
        public static string SolvePart2(string input) { ... }
    }
}
```

`input` is the input file's contents, unchanged.

## Inputs

Inputs are read from `AdventOfCode/Inputs/<year>/dayNN.txt`. The folder is git-ignored because Advent of Code asks that puzzle inputs aren't shared.

If an input file is missing, the runner downloads it from adventofcode.com, as long as it can find your session cookie:

1. Log in on adventofcode.com and copy the value of the `session` cookie from your browser's developer tools.
2. Put it in the `AOC_SESSION` environment variable, or in the file `AdventOfCode/Inputs/.session`, which is also git-ignored.

Each input is downloaded once and then read from disk. Downloaded files are saved with your OS's line endings and no trailing newline, the same as an input pasted into Visual Studio.
