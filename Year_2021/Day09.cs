namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 09 from year 2021
/// </summary>
public class Day09 : BaseDay {
    private readonly int[][] _map;
    private readonly (int row, int column)[] _lowestPoints;

    public Day09() {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        _lowestPoints = FindLowestPoints(_map).ToArray();
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
            if (neighbour.height <= map[row][column]) {
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

    private static IEnumerable<(int row, int column, int height)> GetNeighbours(int[][] map, (int row, int column) coordinate) {
        return GetNeighbours(map, coordinate.row, coordinate.column);
    }

    private static IEnumerable<(int row, int column, int height)> GetNeighbours(int[][] map, int row, int column) {
        // Top
        if (row > 0) {
            yield return (row - 1, column, map[row - 1][column]);
        }

        //Left
        if (column > 0) {
            yield return (row, column - 1, map[row][column - 1]);
        }

        //Right
        if (column < map[row].Length - 1) {
            yield return (row, column + 1, map[row][column + 1]);
        }

        //Bottom
        if (row < map.Length - 1) {
            yield return (row + 1, column, map[row + 1][column]);
        }
    }

    public override ValueTask<string> Solve_1() {
        var result = CalculateRiskHeight(_map, _lowestPoints);

        return new ValueTask<string>($"Result: `{result}`");
    }

    private static IEnumerable<ulong> GetBasinSizes(int[][] map, IEnumerable<(int row, int column)> coordinates) {
        foreach (var coordinate in coordinates) {
            yield return GetBasinSize(map, coordinate, 0, new HashSet<(int row, int column)>());
        }
    }

    private static ulong GetBasinSize(int[][] map, (int row, int column) coordinate, ulong currentSize, HashSet<(int row, int column)> visited) {
        var neighbours = GetNeighbours(map, coordinate);
        visited.Add(coordinate);
        currentSize += 1;
        // Replace instead of add
        currentSize = CheckNeighboursForBasin(map, coordinate, currentSize, visited, neighbours);

        return currentSize;
    }

    private static ulong CheckNeighboursForBasin(int[][] map, (int row, int column) coordinate, ulong currentSize,
        HashSet<(int row, int column)> visited,
        IEnumerable<(int row, int column, int height)> neighbours) {
        foreach (var neighbour in neighbours) {
            // Already added
            if (visited.Contains((neighbour.row, neighbour.column))) {
                continue;
            }

            // We don't add stuff with 9
            if (neighbour.height != 9) {
                continue;
            }

            // We check for the height now
            if (neighbour.height > map[coordinate.row][coordinate.column]) {
                // Replace the current size with our recursive function
                // We already have 'visited' so we don't miss anything 
                currentSize = GetBasinSize(map, (neighbour.row, neighbour.column), currentSize, visited);
            }
        }

        return currentSize;
    }

    public override ValueTask<string> Solve_2() {
        var basinSizes = GetBasinSizes(_map, _lowestPoints).ToArray();
        var result = basinSizes.OrderByDescending(x => x).Take(3).Aggregate((arg1, arg2) => arg1 * arg2);

        //`1038240`
        return new ValueTask<string>($"Result: `{result}`");
    }
}
