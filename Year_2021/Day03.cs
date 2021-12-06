using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 03 from year 2021
/// </summary>
public class Day03 : BaseDay {
    private readonly bool[][] _data;

    public Day03() {
        _data = File.ReadAllLines(InputFilePath).Select(x => x.Select(c => c == '1').ToArray()).ToArray();
    }

    public static bool MostCommonBit(bool[] bools) {
        var trues = bools.Count(x => x);
        return trues >= (int)Math.Ceiling(bools.Length / 2.0);
    }

    public static bool[] FilterByCommonBit(bool[][] bits, int pos = 0, bool mostCommon = true) {
        var transposed = bits.Transpose().Select(x => x.ToArray()).ToArray();
        var bitToFilterWith = MostCommonBit(transposed[pos]);

        if (!mostCommon) {
            bitToFilterWith = !bitToFilterWith;
        }

        var filtered = bits.Where(x => x[pos] == bitToFilterWith).ToArray();

        pos++;
        if (pos > transposed.Length || filtered.Length == 1) {
            return filtered.First();
        }

        return FilterByCommonBit(filtered, pos, mostCommon);
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
        return new ValueTask<string>($"Result is `{result}`.\r\n Gamma: {gammaResult}, epsilon: {epsilonResult}");
    }

    public override ValueTask<string> Solve_2() {
        var oxygen = FilterByCommonBit(_data).Reverse().ToInt();
        var scrubbing = FilterByCommonBit(_data, mostCommon: false).Reverse().ToInt();
        var result = oxygen * scrubbing;
        return new ValueTask<string>($"Result is `{result}`\r\n O2: {oxygen}, O2 Scrubbing: {scrubbing}");

        // 11510100
    }
}
