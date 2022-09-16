namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 01 from year 2021
/// </summary>
internal sealed class Day01 : BaseDay {
    public enum Direction {
        Up,
        Down,
        Stagnant
    }

    private readonly int[] _numbers;

    public Day01() {
        _numbers = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }

    public Day01(string input) {
        _numbers = input.Split("\n").Select(int.Parse).ToArray();
    }

    public IEnumerable<Direction> GetDirectionsWithSlidingView(int amountToCompare) {
        var result = new List<Direction>();
        for (var i = 1; i < _numbers.Length; i++) {
            var current = _numbers.Skip(i).Take(amountToCompare).Sum();
            var previous = _numbers.Skip(i - 1).Take(amountToCompare).Sum();

            if (current > previous) {
                result.Add(Direction.Up);
            } else if (current < previous) {
                result.Add(Direction.Down);
            } else if (current == previous) {
                result.Add(Direction.Stagnant);
            }
        }

        return result;
    }

    public override ValueTask<string> Solve_1() {
        var directions = GetDirectionsWithSlidingView(1);
        var result = directions.Sum(d => d == Direction.Down ? 1 : 0);
        return new ValueTask<string>($"There are {result} steps up with a sliding view of 1");
    }

    public override ValueTask<string> Solve_2() {
        var directions = GetDirectionsWithSlidingView(3);
        var result = directions.Sum(d => d == Direction.Up ? 1 : 0);
        return new ValueTask<string>($"There are {result} steps up wit a sliding view of 3");
    }
}
