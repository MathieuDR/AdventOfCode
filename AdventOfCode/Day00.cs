
namespace AdventOfCode;

/// <summary>
/// Day 1 from year 2020
/// </summary>
public class Day00 : BaseDay {
    private int[] _numbers;

    public Day00() {
        _numbers = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }
   
    private long MultiplyNumbers(IEnumerable<int> numbers) {
        // return numbers multiplied
        return numbers.Aggregate((current, number) => current * number);
    }

    private long GetMax<T>(IEnumerable<IEnumerable<T>> items, Func<IEnumerable<T>, long> func) {
        return items.Select(func).Max();
    }

    public override ValueTask<string> Solve_1() {
        var numberPairs = _numbers.GetNumbersThatSumTo(2,2020);
        return new ValueTask<string>(GetMax(numberPairs, MultiplyNumbers).ToString());
    }

    public override ValueTask<string> Solve_2() {
        var numberPairs = _numbers.GetNumbersThatSumTo(3,2020);
        return new ValueTask<string>(GetMax(numberPairs, MultiplyNumbers).ToString());
    }
}
