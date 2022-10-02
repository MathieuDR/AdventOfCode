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
        var distances = CreateDistanceDictionaries();
        sw.Stop();
        
        Console.WriteLine($"Done indexing in {sw.ElapsedMilliseconds}ms with");
        sw.Start();
        var beaconField = CreateCommonBeaconField(distances);
        sw.Stop();
        
        Console.WriteLine(beaconField.Count);
    }

    
    // create scanner dictionaries with inner dictionaries of distance to each beacon
    // Check each scanner field with another one to see if we find 12 beacons with the same distances to other ones
    // if we find it, we know which ones are the common beacons
    // use the first and second one from both fields (a and b) to create a 'minus' vectors a3 = a0 - a1, b3 = b0 - b1
    // a3 and b3 should have the same values, but mixed axisses/inverted
    // Create different 'getters' for each axis part
    // Create an aligned version of the rotated b0 vector
    // minus a0 with aligned b0
    // create function to use the getters and the 'to add' values
    // Add all non common beacons to one big 'field' using the created function
    // fill up new distances with each-other
    // repeat until we only have one field
    
    private static List<Beacon> CreateCommonBeaconField(List<Dictionary<Beacon, Dictionary<Beacon, float>>> distances) {
        const int neededToBeCommon = 12;
        // var beaconField = distances[0];
        // var scannerList = distances.Skip(1).ToList();
        // var result = beaconField.Select(x => x.Key).ToList();
        var loops = 0;
        while (distances.Count() > 1) {
            var foundSomething = false;
            for (var scannerIndexReference = 0; scannerIndexReference < distances.Count; scannerIndexReference++) {
                var beaconField = distances[scannerIndexReference];
                var toAdd = new List<Beacon>();
                int scannerIndex;
                for (scannerIndex = 0; scannerIndex < distances.Count; scannerIndex++) {
                    var toTest = distances[scannerIndex];

                    if (toTest == beaconField) {
                        continue;
                    }

                    var sw = Stopwatch.StartNew();
                    var commonBeacons = GetCommonBeacons(toTest, beaconField, neededToBeCommon);
                    sw.Stop();
                    // Console.WriteLine($"Done finding common beacons in field {scannerIndex}/{scannerList.Count()} in {sw.ElapsedMilliseconds}ms with");

                   
                    if (commonBeacons.Count < neededToBeCommon) {
                        if (commonBeacons.Count > 8) {
                            Console.WriteLine("Nani");
                        }
                        // Console.WriteLine($"Which is not enough");
                        continue;
                    }
                    
                    Console.WriteLine(
                        $"Found {commonBeacons.Count()}/{toTest.Count} common beacons in scanner {scannerIndex}/{distances.Count()} in {sw.ElapsedMilliseconds}ms");


                    var beaconFunction = GetAlignBeaconFunction(commonBeacons);
                    var newCommonBeacons = commonBeacons.Select(x => x.scannerBeacon).ToList();
                    toAdd = toTest.Select(x => x.Key)
                        .Where(x => !newCommonBeacons.Contains(x))
                        .Select(x => beaconFunction(x))
                        .ToList();
                    foundSomething = true;
                    break;
                }

                if (!toAdd.Any()) {
                    continue;
                }

                AddToDistanceDictionary(toAdd, beaconField);
                Console.WriteLine($"Added {toAdd.Count()} to our field which is now {beaconField.Count()} beacons total.");
                distances.RemoveAt(scannerIndex);
            }

            loops++;
            Console.WriteLine($"Looped {loops} times");
            if (!foundSomething) {
                throw new Exception("boo-hoo");
            }
        }

        return distances[0].Keys.ToList();
    }

    private static Func<Beacon, Beacon> GetAlignBeaconFunction(List<(Beacon reference, Beacon toAlign)> beacons) {
        var getters = CreateGetters(beacons);

        if (getters.Any(func => func is null)) {
            throw new NullReferenceException("One of the funcs is null");
        }

        var toAddVector = GetToAddVector(beacons, getters);

        return b => {
            var location = new Vector(getters[0]!(b) + toAddVector.X, getters[1]!(b) + toAddVector.Y, getters[2]!(b) + toAddVector.Z);
            return new Beacon(location);
        };
    }

    private static Vector GetToAddVector(List<(Beacon reference, Beacon toAlign)> beacons, Func<Beacon, int>?[] getters) {
        var alignedB1 = new Vector(getters[0]!(beacons[0].toAlign), getters[1]!(beacons[0].toAlign), getters[2]!(beacons[0].toAlign));
        var toAddVector = VectorHelper.Minus(beacons[0].reference.Location, alignedB1);
        return toAddVector;
    }

    private static Func<Beacon, int>?[] CreateGetters(List<(Beacon reference, Beacon toAlign)> beacons) {
        var a1 = beacons[0].reference;
        var a2 = beacons[1].reference;
        var a3 = VectorHelper.Minus(a1.Location, a2.Location);

        var b1 = beacons[0].toAlign;
        var b2 = beacons[1].toAlign;
        var b3 = VectorHelper.Minus(b1.Location, b2.Location);

        var x = FindAxis(a3, b3, 0);
        var y = FindAxis(a3, b3, 1);
        var z = FindAxis(a3, b3, 2);

        var getters = new Func<Beacon, int>?[] { null, null, null };
        getters = GetFunctions(x.axis, x.inverted, getters);
        getters = GetFunctions(y.axis, y.inverted, getters);
        getters = GetFunctions(z.axis, z.inverted, getters);
        return getters;
    }

    private static Func<Beacon, int>?[] GetFunctions(int axis, bool inverted, Func<Beacon, int>?[] functions) {
        functions[axis] = b => inverted ?b.Location.GetValue(axis) * -1 : b.Location.GetValue(axis);
        return functions;
    }

    
    /// <summary>
    /// Axis
    /// 0 = X
    /// 1 = Y
    /// 2 = Z
    /// </summary>
    /// <param name="original"></param>
    /// <param name="rotated"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static (int axis, bool inverted) FindAxis(Vector original, Vector rotated, int axis) {
        const float tolerance = 0.01f;
        var value = axis switch {
            0 => rotated.X,
            1 => rotated.Y,
            2 => rotated.Z,
            _ => throw new ArgumentOutOfRangeException()
        };

        var abs = Math.Abs(value);

        if (Math.Abs(Math.Abs(original.X) - abs) < tolerance) {
            return (0, Math.Abs(original.X - value) > tolerance);
        }
        
        if (Math.Abs(Math.Abs(original.Y) - abs) < tolerance) {
            return (1, Math.Abs(original.Y - value) > tolerance);
        }
        
        if (Math.Abs(Math.Abs(original.Z) - abs) < tolerance) {
            return (2, Math.Abs(original.Z - value) > tolerance);
        }

        throw new ArgumentOutOfRangeException();
    }
    
    private static List<(Beacon original, Beacon scannerBeacon)> GetCommonBeacons(Dictionary<Beacon, Dictionary<Beacon, float>> toTest, Dictionary<Beacon, Dictionary<Beacon, float>> beaconField, int neededToBeCommon) {
        var commonBeacons = new List<(Beacon original, Beacon scannerBeacon)>(); // list of common beacons in scanner field

        // foreach beacon and distance in the reference list
        foreach (var (beacon, beaconDistances) in beaconField) {
            var commonDistanceDict = new Dictionary<Beacon, int>();
            // foreach beacon and distance from the reference beacon
            foreach (var (_, distance) in beaconDistances) {
                foreach (var (newBeacon, consolidatedBeaconDistances) in toTest) {
                    foreach (var (_, consolidatedDistance) in consolidatedBeaconDistances) {
                        if (Math.Abs(consolidatedDistance - distance) < 0.01f) {
                            // Equals
                            if (!commonDistanceDict.TryGetValue(newBeacon, out var @int)) {
                                commonDistanceDict.Add(newBeacon, 0);
                                @int = 0;
                            }

                            commonDistanceDict[newBeacon] = @int + 1;
                            break;
                        }
                    }
                }
            }

            if (!commonDistanceDict.Any()) {
                continue;
            }

            var max = commonDistanceDict.Max(x=> x.Value);
            if (max < neededToBeCommon - 1) {
                continue;
            }

            var (nb, _) = commonDistanceDict.FirstOrDefault(x=> x.Value == max);
            commonBeacons.Add((beacon, nb));
            // if I know one, I should have all normally.
        }

        return commonBeacons;
    }

    private static Dictionary<Beacon, Dictionary<Beacon, float>> AddToDistanceDictionary(IEnumerable<Beacon> beacons,
        Dictionary<Beacon, Dictionary<Beacon, float>> dictionary) {
        var beaconArr = beacons as Beacon[] ?? beacons.ToArray();
        var existingBeacons = dictionary.Keys.ToList();

        var compound = existingBeacons.ToList();
        compound.AddRange(beaconArr);

        foreach (var beacon in beaconArr) {
            foreach (var (fromBeacon, distances) in dictionary) {
                if(!distances.ContainsKey(beacon))
                    distances.Add(beacon, VectorHelper.DistanceFrom(fromBeacon.Location, beacon.Location));
            }
            
            var beaconDict = compound
                .Where(toBeacon => toBeacon != beacon)
                .ToDictionary(toBeacon => toBeacon, 
                    toBeacon => VectorHelper.DistanceFrom(beacon.Location, toBeacon.Location));
        
            dictionary.Add(beacon, beaconDict);
        }

        return dictionary;
    }
    
    private List<Dictionary<Beacon, Dictionary<Beacon, float>>> CreateDistanceDictionaries() {
        var result = new List<Dictionary<Beacon, Dictionary<Beacon, float>>>();
        foreach (var scanner in _scanners) {
            result.Add(AddToDistanceDictionary(scanner.Beacons, 
                new Dictionary<Beacon, Dictionary<Beacon, float>>()));
        }

        return result;
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

    internal sealed record Vector(int X, int Y, int Z) {
        public int GetValue(int axis) =>
            axis switch {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException()
            };

        public Vector WithNewValue(int axis, int value) =>
            axis switch {
                0 => this with {X = value},
                1 => this with {Y = value},
                2 => this with {Z = value},
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    internal static class VectorHelper {
        private static Vector3 ToVector3(Vector vector) => new Vector3(vector.X, vector.Y, vector.Z);
        public static float DistanceFrom(Vector from, Vector to) {
            var from3 = ToVector3(from);
            var to3 = ToVector3(to);
            return Vector3.Distance(from3, to3);
        }

        public static Vector Minus(Vector from, Vector to) => new Vector(from.X - to.X, from.Y - to.Y, from.Z - to.Z);
    }

}
