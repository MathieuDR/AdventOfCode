namespace AdventOfCode.Year_2020;

/// <summary>
///     Day 2 from year 2020
/// </summary>
public class Day03 : BaseDay {
    private readonly string[] map;

    public Day03() {
        map = File.ReadAllLines(InputFilePath);
    }

    public Day03(string input) {
        map = input.Split("\n");
    }

    private int CalculateCollisions(int rightStep, int downStep) {
        var collisions = 0;
        for (var i = 0; i < map.Length; i += downStep) {
            var index = rightStep * i % map[i].Length;
            var mapObject = map[i][index];
            if (mapObject == '#') {
                collisions++;
            }
        }

        return collisions;
    }

    public override ValueTask<string> Solve_1() {
        var collisions = CalculateCollisions(3, 1);
        return new ValueTask<string>(
            $"We hit {collisions} trees");
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>(
            "");
    }
}
