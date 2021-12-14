using System.Text;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 14 from year 2021
/// </summary>
public class Day14 : BaseDay {
    private readonly (char LeftBound, char RightBount, string Bounds, char ToInsert)[] _instructions;
    private readonly string _currentTemplate;

    public Day14() {
        var lines = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine);


        _currentTemplate = lines[0];

        // var instructionLines = lines[1].Split(Environment.NewLine);
        // _instructions = new (char LeftBound, char RightBount, string Bounds, char ToInsert)[instructionLines.Length];
        _instructions = lines[1]
            .Split(Environment.NewLine)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(parts => (parts[0][0], parts[0][1], parts[0], parts[1][0]))
            .ToArray();
        // for (var i = 0; i < instructionLines.Length; i++) {
        //     var instructionLine = instructionLines[i];
        //     var parts = instructionLine.Split("->", StringSplitOptions.TrimEntries);
        //     _instructions[i] = (parts[0][0], parts[0][1], parts[0], parts[1][0]);
        // }
    }

    private static string Step(string template, (char LeftBound, char RightBount, string Bounds, char ToInsert)[] instructions, int steps) {
        for (var i = 0; i < steps; i++) {
            template = Step(template, instructions);
        }

        return template;
    }

    private static string Step(string template, (char LeftBound, char RightBount, string Bounds, char ToInsert)[] instructions) {
        var builder = new StringBuilder();
        builder.Append(template[0]);
        for (var i = 1; i < template.Length; i++) {
            var right = template[i];

            var newChar = instructions.First(x => x.LeftBound == builder[^1] && x.RightBount == right).ToInsert;
            builder.Append(newChar);
            builder.Append(right);
        }

        return builder.ToString();
    }

    public override ValueTask<string> Solve_1() {
        var template = Step(_currentTemplate, _instructions, 10);
        var counts = template.GroupBy(x => x).Select(x => (x.Key, Count: x.Count())).OrderByDescending(x => x.Count).ToArray();
        var result = counts[0].Count - counts[^1].Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = "HGAJBEHC";
        return new ValueTask<string>($"Result: `{result}`");
    }
}
