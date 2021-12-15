namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 15 from year 2021
/// </summary>
public class Day15 : BaseDay {

    // A utility function to find the
    // vertex with minimum distance
    // value, from the set of vertices
    // not yet included in shortest
    // path tree
    public readonly int[][] _map;

    public Day15() {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
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
    private int[] Dijkstra(int[][] graph, int source, int loops = 1) {
        var graphLength = graph.Length * graph.Length * loops * loops;
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
        var printValue = (graph.Length + graph.Length) * loops;
        Console.WriteLine($"Looping over {graphLength} values");
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

            if (count % printValue == 0) {
                Console.WriteLine($"Looped over {count / printValue} values");
            }
            for (var v = 0; v < graphLength; v++) {
                
                
                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]

                //var value = GetValue(new Coordinate(u, v), graph);
                var value = GetGraphValue(u, v, graph, loops);

                if (!shortestPathTreeSet[v] && value != 0 &&
                    distance[u] != int.MaxValue && distance[u] + value < distance[v]) {
                    distance[v] = distance[u] + value;
                }
            }
        }

        return distance;
    }


    private static int GetGraphValue(int currentNode, int nodeToCheck, int[][] map, int loops) {
        var currentNodeCoords = NodeInListToCoordinates(currentNode, map.Length * loops);
        var nodeToCheckCoords = NodeInListToCoordinates(nodeToCheck, map.Length * loops);

        if (AreNeighbours(currentNodeCoords, nodeToCheckCoords)) {
            return GetValue(nodeToCheckCoords, map);
        }

        return 0;
    }

    private static bool AreNeighbours(Coordinate nodeCoords, Coordinate nodeToCheckCoords) {
        // They are the same, so no
        if (nodeCoords == nodeToCheckCoords) {
            return false;
        }

        var yDiff = Math.Abs(nodeCoords.Y - nodeToCheckCoords.Y);
        var xDiff = Math.Abs(nodeCoords.X - nodeToCheckCoords.X);
        return yDiff <= 1 && xDiff <= 1 && yDiff != xDiff;
    }


    private static Coordinate NodeInListToCoordinates(int node, int mapLenght) {
        // var listLoop = mapLenght * mapLenght;

        // var original = new Coordinate(node / mapLenght, node % mapLenght);
        return new Coordinate(node / mapLenght, node % mapLenght);
    }
    
    private static int GetValue(Coordinate coordinate, int[][] map) {
        var length = map.Length;
        var yLoops = coordinate.Y / length;
        var xLoops = coordinate.X / length;

        var originalCoord = new Coordinate(coordinate.Y % length, coordinate.X % length);
        var originalValue = map[originalCoord.Y][originalCoord.X];
        var result = originalValue + yLoops + xLoops;
        while (result > 9) {
            result -= 9;
        }

        return result;
    }


    public override ValueTask<string> Solve_1() {
        // var paths = Dijkstra(_map, 0);
        // var result = paths.Last();
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        // var paths = Dijkstra(_map, 0, 5);
        // var result = paths.Last();
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    private readonly record struct Coordinate(int Y, int X);
}
