namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 15 from year 2021
/// </summary>
public class Day15 : BaseDay {
    public readonly int[][] _graph;

    // A utility function to find the
    // vertex with minimum distance
    // value, from the set of vertices
    // not yet included in shortest
    // path tree
    public readonly int[][] _map;

    public Day15() {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        _graph = ConvertToGraphs(_map);
    }

    private int MinDistance(int[] dist, bool[] sptSet) {
        // Initialize min value
        int min = int.MaxValue, minIndex = -1;

        for (var v = 0; v < dist.Length; v++) {
            if (sptSet[v] == false && dist[v] <= min) {
                min = dist[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    // Function that implements Dijkstra's
    // single source shortest path algorithm
    // for a graph represented using adjacency
    // matrix representation
    private int[] Dijkstra(int[][] graph, int source) {
        var graphLength = graph.Length;
        var distance = new int[graphLength];

        // The output array. dist[i]
        // will hold the shortest
        // distance from src to i

        // sptSet[i] will true if vertex
        // i is included in shortest path
        // tree or shortest distance from
        // src to i is finalized
        var shortestPathTreeSet = new bool[graphLength];

        // Initialize all distances as
        // INFINITE and stpSet[] as false
        for (var i = 0; i < graphLength; i++) {
            distance[i] = int.MaxValue;
            shortestPathTreeSet[i] = false;
        }

        // Distance of source vertex
        // from itself is always itself
        distance[source] = 0; //graph[source][source];

        // Find shortest path for all vertices
        for (var count = 0; count < graphLength; count++) {
            // Pick the minimum distance vertex
            // from the set of vertices not yet
            // processed. u is always equal to
            // src in first iteration.
            var u = MinDistance(distance, shortestPathTreeSet);

            // Mark the picked vertex as processed
            shortestPathTreeSet[u] = true;

            // Update dist value of the adjacent
            // vertices of the picked vertex.
            for (var v = 0; v < graphLength; v++) {
                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]

                if (!shortestPathTreeSet[v] && graph[u][v] != 0 &&
                    distance[u] != int.MaxValue && distance[u] + graph[u][v] < distance[v]) {
                    distance[v] = distance[u] + graph[u][v];
                }
            }
        }

        return distance;
    }

    private static int[][] ConvertToGraphs(int[][] map) {
        var length = map.Length * map.Length;
        var result = new int[length][];
        for (var i = 0; i < map.Length; i++) {
            for (var u = 0; u < map.Length; u++) {
                var nodeCounter = i * map.Length + u;
                result[nodeCounter] = new int[length];

                var currentCoord = new Coordinate(i, u);
                var neighbours = GetNeighbours(map, currentCoord).ToArray();

                for (var nodeI = 0; nodeI < result[nodeCounter].Length; nodeI++) {
                    var coord = new Coordinate(nodeI / map.Length, nodeI % map.Length);

                    var value = 0;
                    if (coord == currentCoord || neighbours.Contains(coord)) {
                        value = GetValue(coord, map);
                    }

                    result[nodeCounter][nodeI] = value;
                }
            }
        }

        return result;
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

    private static int GetValue(Coordinate coordinate, int[][] map) {
        return map[coordinate.Y][coordinate.X];
    }


    public override ValueTask<string> Solve_1() {
        var paths = Dijkstra(_graph, 0);
        var result = paths.Last();
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    private readonly record struct Coordinate(int Y, int X);
}
