namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 13 from year 2021
/// </summary>
public class Day13 : BaseDay {
    private bool[][] _page;
    private readonly (string axis, int value)[] _instructions;
    private List<(int x, int y)> _coords;

    public Day13() {
        var lines = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine).Select(x => x.Split(Environment.NewLine))
            .ToArray();

        //_coords = lines[0].Select(x => x.Split(",").Select(int.Parse).ToArray()).OrderBy(c=> c[1]*10000+c[0]).ToArray();
        _coords = lines[0]
            .Select(x => x.Split(",").Select(int.Parse).ToArray())
            .OrderBy(c => c[1] * 10000 + c[0]).Select(x => (x: x[0], y: x[1]))
            .ToList();

        _instructions = new (string axis, int value)[lines[1].Length];

        for (var i = 0; i < lines[1].Length; i++) {
            var instr = lines[1][i];
            var value = instr.Split(" ").Last().Split("=");
            _instructions[i] = (value[0], int.Parse(value[1]));
        }
    }

    private void Fold(string axis, int foldLine) {
        var newCoords = new List<(int x, int y)>();
        foreach (var coord in _coords) {
            var toAdd = (coord.x, coord.y);
            if (axis == "y") {
                toAdd.y = Fold(foldLine, toAdd.y);
            }

            if (axis == "x") {
                toAdd.x = Fold(foldLine, toAdd.x);
            }

            newCoords.Add(toAdd);
        }

        _coords = newCoords.Distinct().ToList();
    }

    private int Fold(int foldLine, int position) {
        if (position < foldLine) {
            return position;
        }

        return foldLine - (position - foldLine);
    }

    private void Fold((string axis, int value) instruction) {
        Fold(instruction.axis, instruction.value);
    }

    public override ValueTask<string> Solve_1() {
        Fold(_instructions[0]);
        var result = _coords.Count;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }
}
