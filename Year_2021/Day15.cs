namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 15 from year 2021
/// </summary>
public class Day15 : BaseDay {
    public readonly int[][] _map;

    public Day15() {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    private static List<List<Coordinate>> FindAllPathsToEnd(Coordinate start, Coordinate end, int[][] map) {
        var pathWays = new List<List<Coordinate>>();
        var neighbours = GetNeighbours(map, start);

        // Start a path for each connected cave
        foreach (var coordinate in neighbours) {
            var path = new List<Coordinate> { start, coordinate };
            // Add the results from our pathfinding
            // this is a range
            pathWays.AddRange(FindAllPathsToEnd(coordinate, end, path, map));
        }

        return pathWays;
    }

    private static List<List<Coordinate>> FindAllPathsToEnd(Coordinate start, Coordinate end, List<Coordinate> currentPathway, int[][] map) {
        var pathWays = new List<List<Coordinate>>();
        var neighbours = GetNeighbours(map, start);
        foreach (var coordinate in neighbours) {
            // When it's a start dont continue
            // if it's a small cave & we have it in our list. Check if we can revisit the small cave
            if (currentPathway.Contains(coordinate)) {
                continue;
            }

            // Create a new path
            var path = new List<Coordinate>(currentPathway);
            path.Add(coordinate);

            if (coordinate != end) {
                // If it's not the end, continue to iterate on this path
                pathWays.AddRange(FindAllPathsToEnd(coordinate, end, path, map));
            } else {
                // It's the end, so we can 'stop' this pathway and add it to our result
                pathWays.Add(path);
            }
        }

        return pathWays;
    }

    private static IEnumerable<Coordinate> GetNeighbours(int[][] map, Coordinate coordinate, bool diagonally = false) {
        // Top
        if (coordinate.Y > 0) {
            yield return new Coordinate(coordinate.Y - 1, coordinate.X);
        }

        //Left
        if (coordinate.X > 0) {
            yield return new Coordinate(coordinate.Y, coordinate.X - 1);
        }

        //Top left
        if (diagonally && coordinate.Y > 0 && coordinate.X > 0) {
            yield return new Coordinate(coordinate.Y - 1, coordinate.X - 1);
        }

        //Right
        if (coordinate.X < map[coordinate.Y].Length - 1) {
            yield return new Coordinate(coordinate.Y, coordinate.X + 1);
        }

        // Top right
        if (diagonally && coordinate.Y > 0 && coordinate.X < map[coordinate.Y].Length - 1) {
            yield return new Coordinate(coordinate.Y - 1, coordinate.X + 1);
        }

        //Bottom
        if (coordinate.Y < map.Length - 1) {
            yield return new Coordinate(coordinate.Y + 1, coordinate.X);
        }

        //Bottom left
        if (diagonally && coordinate.Y < map.Length - 1 && coordinate.X > 0) {
            yield return new Coordinate(coordinate.Y + 1, coordinate.X - 1);
        }

        // Bottom right
        if (diagonally && coordinate.Y < map.Length - 1 && coordinate.X < map[coordinate.Y].Length - 1) {
            yield return new Coordinate(coordinate.Y + 1, coordinate.X + 1);
        }
    }

    private int GetValue(Coordinate coordinate) {
        return _map[coordinate.Y][coordinate.X];
    }


    public override ValueTask<string> Solve_1() {
        var paths = FindAllPathsToEnd(new Coordinate(0, 0), new Coordinate(_map.Length, _map.Length), _map);
        var result = paths.Select(x => x.Select(GetValue).Sum()).Min();
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    private readonly record struct Coordinate(int X, int Y);
}
