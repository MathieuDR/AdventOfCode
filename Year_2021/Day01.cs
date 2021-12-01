
namespace AdventOfCode.Year_2021;

/// <summary>
/// Day 1 from year 2021
/// </summary>
public class Day01 : BaseDay {
    private int[] _numbers;

    public Day01() {
        _numbers = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }
    
    private IEnumerable<Direction> GetDirectionsWithSlidingView(int amountToCompare) {
        var result = new List<Direction>();
        for (int i = 1; i < _numbers.Length; i++) {
            var currentNumbers = _numbers.Skip(i).Take(amountToCompare);
            var previousNumbers = _numbers.Skip(i - 1).Take(amountToCompare);
            
            var current = currentNumbers.Sum();
            var previous = previousNumbers.Sum();
            
            if (current > previous) {
                result.Add(Direction.Up);
            } else if (current < previous) {
                result.Add(Direction.Down);
            } else if(current == previous){
                result.Add(Direction.Stagnant);
            } else {
                result.Add(Direction.None);
            }
        }

        return result;
    }
    
    public override ValueTask<string> Solve_1() {
        var directions = GetDirectionsWithSlidingView(1);
        var result = directions.Sum(d => d == Direction.Up ? 1 : 0);
        return new ValueTask<string>($"There are {result} steps up with a sliding view of 1");
    }

    public override ValueTask<string> Solve_2() {
        var directions = GetDirectionsWithSlidingView(3);
        var result = directions.Sum(d => d == Direction.Up ? 1 : 0);
        return new ValueTask<string>($"There are {result} steps up wit a sliding view of 3");
    }
    
    public enum Direction {
        Up,
        Down,
        Stagnant,
        None
    }
}
