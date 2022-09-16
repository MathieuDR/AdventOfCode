namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 06 from year 2021
/// </summary>
internal sealed class Day06 : BaseDay {
    private readonly long[] groups = new long[9];
    private const int _reproduceTime = 6;

    public Day06() {
        var input = File.ReadAllText(InputFilePath).Split(",").Select(int.Parse).ToArray();

        for (var i = 0; i < groups.Length; i++) {
            groups[i] = input.Count(x => x == i);
        }
    }

    public override ValueTask<string> Solve_1() {
        var children = PassDays(80);
        return new ValueTask<string>($"Result: `{children}`");
    }

    public override ValueTask<string> Solve_2() {
        var children = PassDays(256 - 80);
        return new ValueTask<string>($"Result: `{children}`");
    }

    private long PassDays(int days) {
        for (var i = 0; i < days; i++) {
            var parents = groups[0];
            Array.Copy(groups, 1, groups, 0, groups.Length - 1);
            groups[^1] = parents;
            groups[_reproduceTime] += parents;
        }

        return groups.Sum();
    }
}
