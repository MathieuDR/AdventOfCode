namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 12 from year 2021
/// </summary>
internal sealed class Day12 : BaseDay {
    private readonly List<Cave> _caves;

    public Day12() {
        var pathways = File.ReadAllLines(InputFilePath);

        // Create caves
        _caves = pathways.SelectMany(x => x.Split("-")).Distinct().Select(c => new Cave(c)).ToList();

        // Create pathways
        foreach (var pathway in pathways) {
            var caves = pathway.Split("-");
            var c1 = _caves.First(c => c.Id == caves[0]);
            var c2 = _caves.First(c => c.Id == caves[1]);

            c1.ConnectedCaves.Add(c2);
            c2.ConnectedCaves.Add(c1);
        }
    }

    private static List<List<Cave>> FindAllPathsToCave(Cave start, Cave end, bool revisitSmallCave = false) {
        var pathWays = new List<List<Cave>>();

        // Start a path for each connected cave
        foreach (var cave in start.ConnectedCaves) {
            var path = new List<Cave> { cave };
            // Add the results from our pathfinding
            // this is a range
            pathWays.AddRange(FindAllPathsToCave(cave, end, path, revisitSmallCave));
        }

        return pathWays;
    }

    private static List<List<Cave>> FindAllPathsToCave(Cave start, Cave end, List<Cave> currentPathway, bool revisitSmallCave) {
        var pathWays = new List<List<Cave>>();
        foreach (var cave in start.ConnectedCaves) {
            // When it's a start dont continue
            // if it's a small cave & we have it in our list. Check if we can revisit the small cave
            if (cave.IsStart || !cave.IsBig && currentPathway.Contains(cave) &&
                (!revisitSmallCave || CurrentPathwayVisitedSmallCaveTwice(currentPathway))) {
                continue;
            }

            // Create a new path
            var path = new List<Cave>(currentPathway);
            path.Add(cave);
            
            if (cave != end) {
                // If it's not the end, continue to iterate on this path
                pathWays.AddRange(FindAllPathsToCave(cave, end, path, revisitSmallCave));
            } else {
                // It's the end, so we can 'stop' this pathway and add it to our result
                pathWays.Add(path);
            }
        }

        return pathWays;
    }

    private static bool CurrentPathwayVisitedSmallCaveTwice(List<Cave> currentPathway) {
        // Get all the small caves
        var smalls = currentPathway.Where(c => !c.IsBig).ToArray();

        // Check how many we have
        var visitedSmallCaves = smalls.Count();

        // Check how many distincts
        var distinctSmallCaves = smalls.Distinct().Count();

        // If we don't have the same amount of distincts vs visited
        // means we have visited a small cave twice
        return distinctSmallCaves != visitedSmallCaves;
    }


    public override ValueTask<string> Solve_1() {
        var result = FindAllPathsToCave(_caves.First(x => x.IsStart), _caves.First(x => x.IsEnd)).Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = FindAllPathsToCave(_caves.First(x => x.IsStart), _caves.First(x => x.IsEnd), true).Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    internal sealed class Cave {
        public string Id { get; }
        public bool IsBig { get; }
        public bool IsStart => Id == "start";
        public List<Cave> ConnectedCaves { get; } = new();
        public bool IsEnd => Id == "end";

        public Cave(string id) {
            Id = id;
            if (id.ToUpper() == id) {
                IsBig = true;
            }
        }

        public override string ToString() {
            var prefix = IsBig ? "Big cave" : "Small cave";
            return $"{prefix} {Id}, with {ConnectedCaves.Count} connected caves";
        }
    }
}
