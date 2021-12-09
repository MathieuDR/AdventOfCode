namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 09 from year 2021
/// </summary>
public class Day09 : BaseDay {
    private readonly int[][] _map;

    public Day09() {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    public static int CalculateRiskHeight(int[][] map, IEnumerable<(int row, int column)> coordinates) {
        var result = 0;
        foreach (var coordinate in coordinates) {
            result += map[coordinate.row][coordinate.column] + 1;
        }

        return result;
    }

    private static bool IsLowestPoint(int[][] map, int row, int column) {
        var isLowest = true;
        var neighbours = GetNeighbours(map, row, column);
        foreach (var neighbour in neighbours) {
            if (neighbour <= map[row][column]) {
                isLowest = false;
                break;
            }
        }

        return isLowest;
    }

    private static IEnumerable<(int row, int column)> FindLowestPoints(int[][] map) {
        for (var row = 0; row < map.Length; row++) {
            var rowPoints = map[row];
            for (var column = 0; column < rowPoints.Length; column++) {
                if (IsLowestPoint(map, row, column)) {
                    yield return (row, column);
                }
            }
        }
    }

    private static IEnumerable<int> GetNeighbours(int[][] map, int row, int column) {
        if (row > 0) {
            yield return map[row - 1][column];
        }

        if (row < map.Length - 1) {
            yield return map[row + 1][column];
        }

        if (column > 0) {
            yield return map[row][column - 1];
        }

        if (column < map[row].Length - 1) {
            yield return map[row][column + 1];
        }
    }

    public override ValueTask<string> Solve_1() {
        var lowestPoints = FindLowestPoints(_map);
        var result = CalculateRiskHeight(_map, lowestPoints);

        return new ValueTask<string>($"Result: `{result}`");
    }


    public override ValueTask<string> Solve_2() {
        long result = 0;

        return new ValueTask<string>($"Result: `{result}`");
    }
}
