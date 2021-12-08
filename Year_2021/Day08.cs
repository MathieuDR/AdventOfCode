namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 06 from year 2021
/// </summary>
public class Day08 : BaseDay {
    private readonly string[][] _patterns;
    private readonly string[][] _readings;
    private readonly string[][] _keys;

    public Day08() {
        var input = File.ReadAllLines(InputFilePath).Select(x => x.Split("|", StringSplitOptions.TrimEntries).Select(y => y.Split(" ")).ToArray())
            .ToArray();
        _patterns = new string[input.Length][];
        _readings = new string[input.Length][];
        _keys = new string[input.Length][];
        for (var index = 0; index < input.Length; index++) {
            var row = input[index];
            _patterns[index] = row[0].Select(x => new string(x.OrderBy(c => c).ToArray())).OrderBy(x => x.Length).ToArray();
            _readings[index] = row[1].Select(x => new string(x.OrderBy(c => c).ToArray())).ToArray();
            _keys[index] = SegmentSolver.SolveInputs(_patterns[index]);
        }
    }


    public override ValueTask<string> Solve_1() {
        var result = 0;
        for (var i = 0; i < _readings.Length; i++) {
            var segments = _keys[i];
            foreach (var reading in _readings[i]) {
                if (segments[1] == reading || segments[4] == reading || segments[7] == reading || segments[8] == reading) {
                    result++;
                }
            }
        }

        return new ValueTask<string>($"Result: `{result}`");
    }

    // Very optimal
    private long ReadingsToConcattedInt(string[] inputs, string[] keys) {
        var result = "";
        foreach (var input in inputs) {
            result += ReadingToInt(input, keys).ToString();
        }

        return int.Parse(result);
    }

    private int ReadingToInt(string input, string[] keys) {
        return Array.IndexOf(keys, input);
    }

    public override ValueTask<string> Solve_2() {
        long result = 0;
        for (var i = 0; i < _readings.Length; i++) {
            result += ReadingsToConcattedInt(_readings[i], _keys[i]);
        }

        return new ValueTask<string>($"Result: `{result}`");
    }

    public static class SegmentSolver {
        public static string[] SolveInputs(string[] inputs) {
            var segments = new string[10];
            segments[1] = inputs[0]; // 1 Only has 2 segments, its the lowest
            segments[7] = inputs[1]; // 7 Only has 3 segments, it's the second lowest
            segments[4] = inputs[2]; // 4 Only has 4 segments, it's the third in the list
            segments[8] = inputs[^1]; // 8 Has all segments, it's the last 

            var segmentToCharDict = new Dictionary<SegmentDetails, char>();
            segmentToCharDict.Add(SegmentDetails.a, RemoveCharacters(segments[7], segments[1])[0]);

            var _6Segments = inputs.Where(x => x.Length == 6).ToArray();

            // Figure out segment 'g' through removing 4 on all 6 canditates
            var leftovers = _6Segments.Select(x => RemoveCharacters(x, segments[4])).ToArray();

            var indexOf9 = leftovers.Select((value, index) => new { value, index = index + 1 }) // Get the index where there is only 2 chars
                .Where(pair => pair.value.Length == 2)
                .Select(pair => pair.index)
                .FirstOrDefault() - 1;

            segments[9] = _6Segments[indexOf9]; // Put 9 correct

            // Remove 7 from the leftovers, so we remove most of 9, which will only have 1 letter left
            segmentToCharDict.Add(SegmentDetails.g, RemoveCharacters(leftovers.OrderBy(x => x.Length).First(), segments[7])[0]);

            segments[6] = _6Segments.First(x => !segments[7].Select(x.Contains).All(b => b)); // 6 is where we don't have one segment of 7
            segments[0] = _6Segments.First(x => x != segments[9] && x != segments[6]); // 0 is leftover of the numbers with 6 segments


            // Figure out segment e by removing 4 from 6
            var segment_e = RemoveCharacters(segments[6], segments[4]);

            // Remove segment a and g
            segment_e = RemoveCharacters(segment_e, new[] { segmentToCharDict[SegmentDetails.a], segmentToCharDict[SegmentDetails.g] });

            segmentToCharDict.Add(SegmentDetails.e, segment_e[0]); // Add segment e
            segmentToCharDict.Add(SegmentDetails.c, RemoveCharacters(segments[1], segments[6])[0]); // add segment e by removing 6 from 1
            segmentToCharDict.Add(SegmentDetails.f,
                RemoveCharacters(segments[1], new[] { segmentToCharDict[SegmentDetails.c] })[0]); // add segment f by removing the upper segment
            segmentToCharDict.Add(SegmentDetails.d,
                RemoveCharacters(segments[8], segments[0])[0]); // get the middle segment by removing the outer walls using 0
            segmentToCharDict.Add(SegmentDetails.b,
                RemoveCharacters(segments[8],
                    segmentToCharDict.Select(x => x.Value).ToArray())[0]); // add the leftover segment by removing all we have

            // Solve 5 segments
            var _5Segments = inputs.Where(x => x.Length == 5).ToArray(); // Get all 5 segments, the leftovers
            segments[2] = _5Segments.First(x => x.Contains(segmentToCharDict[SegmentDetails.e])); // Only 2 has segment e
            segments[5] = _5Segments.First(x => x.Contains(segmentToCharDict[SegmentDetails.b])); // Only 5 has segment b
            segments[3] = _5Segments.First(x => x != segments[2] && x != segments[5]);

            var distincts = segments.Distinct().Count();
            if (distincts != 10) {
                throw new InvalidOperationException("u dun goofed");
            }

            return segments;
        }

        public static string RemoveCharacters(string input, string toRemove) {
            return RemoveCharacters(input, toRemove.ToArray());
        }

        public static string RemoveCharacters(string input, char[] toRemove) {
            foreach (var c in toRemove) {
                input = input.Replace(c.ToString(), string.Empty);
            }

            return input;
        }
    }

    public enum SegmentDetails {
        a,
        b,
        c,
        d,
        e,
        f,
        g
    }
}
