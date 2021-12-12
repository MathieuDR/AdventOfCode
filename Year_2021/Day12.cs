namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 12 from year 2021
/// </summary>
public class Day12 : BaseDay {
    private readonly List<Cave> _caves;

    public Day12() {
        var pathways = File.ReadAllLines(InputFilePath);
        _caves = pathways.SelectMany(x => x.Split("-")).Distinct().Select(c => new Cave(c)).ToList();
        foreach (var pathway in pathways) {
            var caves = pathway.Split("-");
            var c1 = _caves.First(c => c.Id == caves[0]);
            var c2 = _caves.First(c => c.Id == caves[1]);

            c1.ConnectedCaves.Add(c2);
            c2.ConnectedCaves.Add(c1);
        }
    }

    private static List<List<Cave>> FindAllPathsToCave(Cave start, Cave end) {
        var pathWays = new List<List<Cave>>();

        foreach (var cave in start.ConnectedCaves) {
            var path = new List<Cave> { cave };
            pathWays.AddRange(FindAllPathsToCave(cave, end, path));
        }

        return pathWays;
    }

    private static List<List<Cave>> FindAllPathsToCave(Cave start, Cave end, List<Cave> currentPathway) {
        var pathWays = new List<List<Cave>>();
        foreach (var cave in start.ConnectedCaves) {
            if (!cave.IsBig && currentPathway.Contains(cave) || cave.IsStart) {
                continue;
            }

            var path = new List<Cave>(currentPathway);
            path.Add(cave);

            if (cave != end) {
                pathWays.AddRange(FindAllPathsToCave(cave, end, path));
            } else {
                pathWays.Add(path);
            }
        }

        return pathWays;
    }


    private static List<List<Cave>> FindAllPathsToCaveP2(Cave start, Cave end) {
        var pathWays = new List<List<Cave>>();

        foreach (var cave in start.ConnectedCaves) {
            var path = new List<Cave> { cave };
            pathWays.AddRange(FindAllPathsToCaveP2(cave, end, path));
        }

        return pathWays;
    }

    private static List<List<Cave>> FindAllPathsToCaveP2(Cave start, Cave end, List<Cave> currentPathway) {
        var pathWays = new List<List<Cave>>();
        foreach (var cave in start.ConnectedCaves) {
            if (cave.IsStart) {
                continue;
            }

            if (!cave.IsBig && currentPathway.Contains(cave)) {
                var smalls = currentPathway.Where(c => !c.IsBig).ToArray();
                var visitedSmallCaves = smalls.Count();
                var distinctSmallCaves = smalls.Distinct().Count();

                if (distinctSmallCaves != visitedSmallCaves) {
                    continue;
                }
            }

            var path = new List<Cave>(currentPathway);
            path.Add(cave);

            if (cave != end) {
                pathWays.AddRange(FindAllPathsToCaveP2(cave, end, path));
            } else {
                pathWays.Add(path);
            }
        }

        return pathWays;
    }


    public override ValueTask<string> Solve_1() {
        var result = FindAllPathsToCave(_caves.First(x => x.IsStart), _caves.First(x => x.IsEnd)).Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = FindAllPathsToCaveP2(_caves.First(x => x.IsStart), _caves.First(x => x.IsEnd)).Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public class Cave {
        public string Id { get; set; }
        public bool IsBig { get; set; }
        public bool IsStart => Id == "start";
        public List<Cave> ConnectedCaves { get; set; } = new();
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
