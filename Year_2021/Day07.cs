using MathNet.Numerics.Statistics;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 06 from year 2021
/// </summary>
public class Day07 : BaseDay {
    private readonly int[] crabsPos;

    public Day07(string input) {
        crabsPos = input.Split(",").Select(int.Parse).ToArray();
    }

    public Day07() {
        crabsPos = File.ReadAllText(InputFilePath).Split(",").Select(int.Parse).ToArray();
    }

    public static int CalculateMedian(int[] positions) {
        var median = positions.Select(x => (double)x).Median();
        return (int)Math.Round(median);
    }

    public static int CalculateFuel(int[] positions, int position) {
        return positions.Select(x => Math.Abs(x - position)).Sum();
    }

    public override ValueTask<string> Solve_1() {
        var median = CalculateMedian(crabsPos);
        var result = CalculateFuel(crabsPos, median);

        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }
}
