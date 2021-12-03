using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 23 from year 2021
/// </summary>
public class Day03 : BaseDay {
    private readonly bool[][] _data;

    public Day03() {
        _data = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => c == '1').ToArray()).ToArray();
    }

    public static bool MostCommonBit(bool[] bools) {
        var trues = bools.Count(x => x);
        return trues >= bools.Length / 2;
    }


    public override ValueTask<string> Solve_1() {
        var transposed = _data.Transpose().Select(x => x.ToArray()).ToArray();
        var gamma = new List<bool>();
        var epsilon = new List<bool>();

        for (var i = transposed.Length - 1; i >= 0; i--) {
            var common = MostCommonBit(transposed[i]);
            gamma.Add(common);
            epsilon.Add(!common);
        }

        var gammaResult = gamma.ToInt();
        var epsilonResult = epsilon.ToInt();
        var result = (long)gammaResult * epsilonResult;

        // 2521700
        return new ValueTask<string>($"Result is `{result}`. Gamma: {gammaResult}, epsilon: {epsilonResult}");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result is `{result}`");
    }
}
