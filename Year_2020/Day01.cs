namespace AdventOfCode.Year_2020;

/// <summary>
///     Day 1 from year 2020
/// </summary>
public class Day01 : BaseDay {
    private readonly int[] _numbers;

    public Day01() {
        _numbers = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }

    private long MultiplyNumbers(IEnumerable<int> numbers) {
        return numbers.Aggregate((current, number) => current * number);
    }

    private long GetMax<T>(IEnumerable<IEnumerable<T>> items, Func<IEnumerable<T>, long> func) {
        return items.Select(func).Max();
    }

    public override ValueTask<string> Solve_1() {
        var numberPairs = _numbers.GetNumbersThatSumTo(2, 2020).First().ToArray();
        return new ValueTask<string>(
            $"The numbers are {numberPairs.First()} and {numberPairs.Last()}, resulting in the sum of {MultiplyNumbers(numberPairs)}");
    }

    public override ValueTask<string> Solve_2() {
        var numberPairs = _numbers.GetNumbersThatSumTo(3, 2020).First().ToArray();
        return new ValueTask<string>(
            $"The numbers are {numberPairs.First()}, {numberPairs[1]} and {numberPairs.Last()}, resulting in the sum of {MultiplyNumbers(numberPairs)}");
    }
}
