namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 06 from year 2021
/// </summary>
public class Day06 : BaseDay {
    private readonly List<(int ageGroup, long amount)> groups = new();

    public Day06() {
        var input = File.ReadAllText(InputFilePath).Split(",").Select(int.Parse).ToArray();

        for (var i = 0; i < 10; i++) {
            groups.Add((i, input.Count(x => x == i)));
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
            for (var j = 0; j < groups.Count; j++) {
                if (groups[j].ageGroup == 0) {
                    // handle offspring
                    groups[9] = (9, groups[j].amount);
                    groups[7] = (7, groups[7].amount + groups[j].amount);
                } else {
                    // switch groups
                    groups[j - 1] = (groups[j].ageGroup - 1, groups[j].amount);
                    groups[j] = (j, 0);
                }
            }
        }

        return groups.Sum(x => x.amount);
    }
}
