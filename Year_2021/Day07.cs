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

    public static int CalculateAverage(int[] positions) {
        var average = positions.Average();
        return (int)Math.Round(average);
    }

    public static int CalculateFuel(int[] positions, int position) {
        return positions.Select(x => Math.Abs(x - position)).Sum();
    }

    public static int CalculateFuel2(int[] positions, int position) {
        return positions.Select(x => {
            var steps = Math.Abs(x - position);
            var result = 0;
            for (var i = steps; i > 0; i--) {
                result += i;
            }

            return result;
        }).Sum();
    }

    public override ValueTask<string> Solve_1() {
        var median = CalculateMedian(crabsPos);
        var result = CalculateFuel(crabsPos, median);

        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        // Solve from average, even though it doesn't necessary mean it's correct
        var average = CalculateAverage(crabsPos);
        var lowest = CalculateFuel2(crabsPos, average);
        int result;
        var i = 1;
        do {
            // With some fancy algorithm we can do a binary search somehow
            result = lowest;
            lowest = CalculateFuel2(crabsPos, average + i);

            var t2 = CalculateFuel2(crabsPos, average - i);
            if (t2 < lowest) {
                lowest = t2;
            }

            i++;
        } while (result >= lowest);
        
        return new ValueTask<string>($"Result: `{result}`");
    }

}
