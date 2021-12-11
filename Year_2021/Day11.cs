namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 11 from year 2021
/// </summary>
public class Day11 : BaseDay {
    private readonly int[][] _octopuses;

    public Day11() {
        _octopuses = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    private long Steps(int steps) {
        long result = 0;
        for (var i = 0; i < steps; i++) {
            result += Step();
        }

        return result;
    }

    private (int Row, int Column) ToCoordinateTupple(int row, int column) {
        return (row, column);
    }

    private int Step() {
        var _flashed = new HashSet<(int row, int col)>();
        for (var i = 0; i < _octopuses.Length; i++) {
            var row = _octopuses[i];
            for (var j = 0; j < row.Length; j++) {
                var coordinate = ToCoordinateTupple(i, j);
                row[j] += 1;
                if (row[j] > 9) {
                    _flashed = FlashOctopus(coordinate, _flashed);
                }
            }
        }

        // reset
        foreach (var coordinate in _flashed) {
            _octopuses[coordinate.row][coordinate.col] = 0;
        }

        return _flashed.Count;
    }

    private HashSet<(int row, int col)> FlashOctopus((int Row, int Column) coordinate, HashSet<(int row, int col)> flashed) {
        if (_octopuses[coordinate.Row][coordinate.Column] <= 9 || flashed.Contains(coordinate)) {
            return flashed;
        }

        _octopuses[coordinate.Row][coordinate.Column] = 0;
        flashed.Add(coordinate);
        var neighbours = GetNeighbours(_octopuses, coordinate);
        foreach (var neighbour in neighbours) {
            _octopuses[neighbour.Row][neighbour.Column] += 1;
            FlashOctopus(neighbour, flashed);
        }

        return flashed;
    }

    private static IEnumerable<(int Row, int Column)> GetNeighbours(int[][] map, (int row, int column) coordinate) {
        return GetNeighbours(map, coordinate.row, coordinate.column);
    }

    private static IEnumerable<(int Row, int Column)> GetNeighbours(int[][] map, int row, int column) {
        // Top
        if (row > 0) {
            yield return (row - 1, column);
        }

        //Left
        if (column > 0) {
            yield return (row, column - 1);
        }

        //Top left
        if (row > 0 && column > 0) {
            yield return (row - 1, column - 1);
        }

        //Right
        if (column < map[row].Length - 1) {
            yield return (row, column + 1);
        }

        // Top right
        if (row > 0 && column < map[row].Length - 1) {
            yield return (row - 1, column + 1);
        }

        //Bottom
        if (row < map.Length - 1) {
            yield return (row + 1, column);
        }

        //Bottom left
        if (row < map.Length - 1 && column > 0) {
            yield return (row + 1, column - 1);
        }

        // Bottom right
        if (row < map.Length - 1 && column < map[row].Length - 1) {
            yield return (row + 1, column + 1);
        }
    }


    public override ValueTask<string> Solve_1() {
        var result = Steps(100);

        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var counter = 100; // From solve 1;
        while (_octopuses.SelectMany(x => x).Sum() > 0) {
            Step();
            counter++;
        }

        return new ValueTask<string>($"Result: `{counter}`");
    }
}
