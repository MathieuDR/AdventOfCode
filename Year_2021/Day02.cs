using System.Text.RegularExpressions;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 1 from year 2021
/// </summary>
public class Day02 : BaseDay {
    private readonly string[] _commands;
    private static readonly Regex Regex = new("(\\w+) (\\d+)", RegexOptions.Compiled);

    public Day02() {
        _commands = File.ReadAllLines(InputFilePath).ToArray();
    }

    public static (int depth, int horizontal) CalculatePosition(string[] commands) {
        int depth = 0, horizontal = 0;
        foreach (var command in commands) {
            var matches = Regex.Matches(command);
            var dir = matches[0].Groups[1].Value;
            var amount = int.Parse(matches[0].Groups[2].Value);

            switch (dir) {
                case "forward":
                    horizontal += amount;
                    break;
                case "back":
                    horizontal -= amount;
                    break;
                case "down":
                    depth += amount;
                    break;
                case "up":
                    depth -= amount;
                    break;
            }
        }

        return (depth, horizontal);
    }

    public override ValueTask<string> Solve_1() {
        var (depth, horizontal) = CalculatePosition(_commands);

        return new ValueTask<string>($"The multiplication is {depth * horizontal}");
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>("");
    }
}
