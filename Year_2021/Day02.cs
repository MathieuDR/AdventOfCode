namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 02 from year 2021
/// </summary>
public class Day02 : BaseDay {
    private readonly string[] _commands;

    public Day02() {
        _commands = File.ReadAllLines(InputFilePath).ToArray();
    }

    /// <summary>
    ///     Problem 1
    /// </summary>
    /// <returns></returns>
    public static (int depth, int horizontal) CalculatePosition(string[] commands) {
        int depth = 0, horizontal = 0;
        foreach (var command in commands) {
            var (dir, amount) = ParseCommand(command);
            CalculatePosition(dir, amount, ref horizontal, ref depth);
        }

        return (depth, horizontal);
    }

    /// <summary>
    ///     Problem 2
    /// </summary>
    /// <returns></returns>
    public static (int depth, int horizontal) CalculatePositionWithAim(string[] commands) {
        int depth = 0, horizontal = 0, aim = 0;
        foreach (var command in commands) {
            var (dir, amount) = ParseCommand(command);
            CalculatePosition(dir, amount, ref horizontal, ref depth, ref aim);
        }

        return (depth, horizontal);
    }

    private static void CalculatePosition(string dir, int amount, ref int horizontal, ref int depth) {
        if (dir == "forward") {
            horizontal += amount;
        } else if (dir == "down") {
            depth += amount;
        } else if (dir == "up") {
            depth -= amount;
        }
    }

    private static void CalculatePosition(string dir, int amount, ref int horizontal, ref int depth, ref int aim) {
        if (dir == "forward") {
            horizontal += amount;
            depth += aim * amount;
        } else if (dir == "down") {
            depth += amount;
        } else if (dir == "up") {
            depth -= amount;
        }
    }

    /// <summary>
    ///     Parses a line
    /// </summary>
    /// <param name="command"></param>
    /// <returns>direction & amount</returns>
    private static (string direction, int amount) ParseCommand(string command) {
        var split = command.Split(" ");
        return (split[0], int.Parse(split[1]));
    }

    public override ValueTask<string> Solve_1() {
        var (depth, horizontal) = CalculatePosition(_commands);
        return new ValueTask<string>($"The multiplication is {depth * horizontal}");
    }

    public override ValueTask<string> Solve_2() {
        var (depth, horizontal) = CalculatePositionWithAim(_commands);
        return new ValueTask<string>($"The multiplication is {depth * horizontal}");
    }
}
