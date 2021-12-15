namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 14 from year 2021
/// </summary>
public class Day14 : BaseDay {
    //private string _currentTemplate;

    private readonly char _firstLetter;
    private readonly (char LeftBound, char RightBount, char ToInsert)[] _instructions;

    //private (char left, char right, long amount)[] groups;

    private Dictionary<(char left, char right), long> groupDictionary;

    public Day14() {
        var lines = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine);

        _firstLetter = lines[0][0];
        var groups = new (char left, char right)[lines[0].Length - 1];
        for (var i = 1; i < lines[0].Length; i++) {
            groups[i - 1] = (lines[0][i - 1], lines[0][i]);
        }

        groupDictionary = groups.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());


        _instructions = lines[1]
            .Split(Environment.NewLine)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(parts => (parts[0][0], parts[0][1], parts[1][0]))
            .ToArray();
    }

    private static Dictionary<(char left, char right), long> Step(Dictionary<(char left, char right), long> template,
        (char LeftBound, char RightBount, char ToInsert)[] instructions, int steps) {
        for (var i = 0; i < steps; i++) {
            template = Step(template, instructions);
        }

        return template;
    }

    private static Dictionary<(char left, char right), long> Step(Dictionary<(char left, char right), long> template,
        (char LeftBound, char RightBount, char ToInsert)[] instructions) {
        var result = new Dictionary<(char left, char right), long>();
        foreach (var kvp in template) {
            var instruction = instructions.First(x => x.LeftBound == kvp.Key.left && x.RightBount == kvp.Key.right);

            var pairs = new[] { (kvp.Key.left, instruction.ToInsert), (instruction.ToInsert, kvp.Key.right) };
            foreach (var pair in pairs) {
                if (!result.TryGetValue(pair, out var amount)) {
                    amount = 0;
                    result.Add(pair, amount);
                }

                result[pair] += kvp.Value;
            }
        }

        return result;
    }

    private long CalculateResult(Dictionary<(char left, char right), long> template) {
        var counts = template
            .GroupBy(x => x.Key.right)
            .Select(x => (x.Key, Count: x.Key == _firstLetter ? x.Sum(t => t.Value) + 1 : x.Sum(t => t.Value)))
            .OrderByDescending(x => x.Count).ToArray();

        return counts[0].Count - counts[^1].Count;
    }

    public override ValueTask<string> Solve_1() {
        groupDictionary = Step(groupDictionary, _instructions, 10);
        var result = CalculateResult(groupDictionary);

        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        groupDictionary = Step(groupDictionary, _instructions, 30);
        var result = CalculateResult(groupDictionary);
        return new ValueTask<string>($"Result: `{result}`");
    }
}
