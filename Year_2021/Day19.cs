using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 19 from year 2021
/// </summary>
internal sealed class Day19 : BaseDay {
    // private ArrayAdjacencyGraph<Beacon, UndirectedEdge<Beacon>>[] _graphs;
    private Scanner[] _scanners;
    public Day19() {
        
        var input = File.ReadAllText(InputFilePath);
        _scanners = Reader.ReadScanners(input);

        var sw = Stopwatch.StartNew();
        
        var distances =
            new List<Dictionary<Beacon, Dictionary<Beacon, float>>>();
        
        
        CreateDistanceDictionaries(distances);
        
        sw.Stop();
        
        Console.WriteLine($"Done indexing in {sw.ElapsedMilliseconds}ms with");
        sw.Start();
        var beaconField = CreateCommonBeaconField(distances);
        sw.Stop();
        
        Console.WriteLine(beaconField.Count);
    }

    private static List<Beacon> CreateCommonBeaconField(List<Dictionary<Beacon, Dictionary<Beacon, float>>> distances) {
        const int neededToBeCommon = 12;
        const int neededDistances = 1;
        var beaconField = distances[0];
        var scannerList = distances.Skip(1).ToList();
        var result = beaconField.Select(x => x.Key).ToList();
        
        while (scannerList.Any()) {
            Dictionary<Beacon, Dictionary<Beacon, float>>? field = null;
            foreach (var toTest in scannerList) {
                var commonBeacons = new List<Beacon>();
                foreach (var (beacon, beaconDistances) in toTest) {
                    if (commonBeacons.Contains(beacon)) {
                        continue;
                    }
                    foreach (var (toBeacon, distance) in beaconDistances) {
                        if (commonBeacons.Contains(toBeacon)) {
                            continue;
                        }
                        
                        var equalDistances = 0;
                        foreach (var (_, consolidatedBeaconDistances) in beaconField) {
                            foreach (var (_, consolidatedDistance) in consolidatedBeaconDistances) {
                                if (Math.Abs(consolidatedDistance - distance) < 0.01f) {
                                    // Equals
                                    equalDistances++;
                                }

                                if (equalDistances >= neededDistances) {
                                    break;
                                }
                            }
                            
                            if (equalDistances >= neededDistances) {
                                break;
                            }
                        }
                        
                        if (equalDistances >= neededDistances) {
                            commonBeacons.Add(beacon);
                            
                            //if(!commonBeacons.Contains(toBeacon))
                                commonBeacons.Add(toBeacon);
                            break;
                        }
                    }
                }

                
                //commonBeacons = commonBeacons.Distinct().ToList();

                if (commonBeacons.Count >= neededToBeCommon) {
                    field = toTest;
                    break;
                }

                Console.WriteLine($"Scanner not OK only found {commonBeacons.Count}");
            }

            if (field is null) {
                throw new Exception("Couldn't find field");
            }

            // add to my beaconfield
            scannerList.Remove(field);
        }

        return result;
    }

    private void CreateDistanceDictionaries(List<Dictionary<Beacon, Dictionary<Beacon, float>>> distances) {
        foreach (var scanner in _scanners) {
            var dict = new Dictionary<Beacon, Dictionary<Beacon, float>>();
            for (var i = 0; i < scanner.Beacons.Length - 1; i++) {
                var fromBeacon = scanner.Beacons[i];
                var beaconDict = new Dictionary<Beacon, float>();
                for (var u = i + 1; u < scanner.Beacons.Length; u++) {
                    var toBeacon = scanner.Beacons[u];
                    beaconDict.Add(toBeacon, VectorHelper.DistanceFrom(fromBeacon.Location, toBeacon.Location));
                }

                dict.Add(fromBeacon, beaconDict);
            }

            distances.Add(dict);
        }
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
        private static Vector3 ToVector3(Vector vector) => new Vector3(vector.X, vector.Y, vector.Z);
        public static float DistanceFrom(Vector from, Vector to) {
            var from3 = ToVector3(from);
            var to3 = ToVector3(to);
            return Vector3.Distance(from3, to3);
        }
    }

}
