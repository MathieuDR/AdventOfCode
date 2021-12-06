namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 06 from year 2021
/// </summary>
public class Day06 : BaseDay {
    private readonly long[] groups = new long[10];

    public Day06() {
        var input = File.ReadAllText(InputFilePath).Split(",").Select(int.Parse).ToArray();

        for (var i = 0; i < 10; i++) {
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
            for (var j = 0; j < groups.Length; j++) {
                if (j == 0) {
                    // handle offspring
                    groups[9] = groups[j];
                    groups[7] += groups[j];
                } else {
                    // switch groups
                    groups[j - 1] = groups[j];
                    groups[j] = 0;
                }
            }
        }

        return groups.Sum();
    }
}
