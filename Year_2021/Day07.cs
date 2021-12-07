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
        var average = CalculateAverage(crabsPos);
        var averageFuelConsumption = CalculateFuel2(crabsPos, average);
        var result = Math.Min(LowestPoint(crabsPos, average, 1, averageFuelConsumption), LowestPoint(crabsPos, average, -1, averageFuelConsumption));
        
        return new ValueTask<string>($"Result: `{result}`");
    }


    public static int LowestPoint(int[] crabs, int position, int delta, int start) {
        var result = start;
        int temp;
        var i = 1;
        do {
            temp = CalculateFuel2(crabs, position + i * delta);
            i++;

            // Temp is lower, update result
            if (temp < result) {
                result = temp;
            }

            // Lets exit
            if (i > crabs.Length) {
                break;
            }
        } while (start >= temp);

        return result;
    }
}
