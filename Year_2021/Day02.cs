namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 2 from year 2021
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
        switch (dir) {
            case "forward":
                horizontal += amount;
                break;
            case "down":
                depth += amount;
                break;
            case "up":
                depth -= amount;
                break;
        }
    }

    private static void CalculatePosition(string dir, int amount, ref int horizontal, ref int depth, ref int aim) {
        switch (dir) {
            case "forward":
                horizontal += amount;
                depth += aim * amount;
                break;
            case "down":
                aim += amount;
                break;
            case "up":
                aim -= amount;
                break;
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
