using System.Globalization;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 19 from year 2021
/// </summary>
internal sealed class Day19 : BaseDay {
    public Day19() {
        var input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    internal static class Reader {
        public static Scanner[] ReadScanners(string input) {
            var result = new List<Scanner>();
            var beacons = new List<Beacon>();
            
            foreach (var line in input.ReadLines()) {
                if (string.IsNullOrWhiteSpace(line)) {
                    continue;
                }
                
                if (line.StartsWith("--- scanner")) {
                    if(!beacons.Any()) // skip first
                        continue;
                    
                    result.Add(new Scanner(result.Count, beacons.ToArray()));
                    beacons = new List<Beacon>();
                    continue;
                }

                beacons.Add(ParseBeacon(line));
            }
            
            result.Add(new Scanner(result.Count, beacons.ToArray()));
            
            return result.ToArray();
        }

        private static Beacon ParseBeacon(string line) {
            var coords = line.Split(",").Select(int.Parse).ToArray();
            return new Beacon(new Vector(coords[0], coords[1], coords[2]));
        }
    }

    internal sealed record Scanner(int Index, Beacon[] Beacons) {
        public int XRotation { get; init; } = 0;
        public int YRotation { get; init; } = 0;
        public int ZRotation { get; init; } = 0;
    };

    internal sealed record Beacon(Vector Location);
    internal sealed record Vector(int X, int Y, int Z);

    internal static class VectorHelper {
        public static Vector DistanceFrom(Vector from, Vector to) {
            return new Vector(from.X - to.X, from.Y - to.Y, from.Z - to.Z);
        }
    }

}
