using System.Linq;
using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public class Day07Tests {
    [Theory]
    [InlineData("16,1,2,0,4,2,7,1,2,14", 2)]
    [InlineData("2,2,0,4,2,7,1,2,14", 2)]
    [InlineData("5,7,8,2,4", 5)]
    [InlineData("23,27,16,31", 25)]
    public void MiddlePointShouldBe2(string input, int expected) {
        // 0 1 1 2 2 4 7 14 
        var numbers = input.Split(",").Select(int.Parse).ToArray();
        var middlePoint = Day07.CalculateMedian(numbers);

        middlePoint.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 37)]
    [InlineData(1, 41)]
    [InlineData(3, 39)]
    [InlineData(10, 71)]
    public void CalculateFuelShouldBeCorrect(int position, int expected) {
        // 0 1 1 2 2 4 7 14 16
        var input = "16,1,2,0,4,2,7,1,2,14".Split(",").Select(int.Parse).ToArray();
        var fuel = Day07.CalculateFuel(input, position);

        fuel.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 206)]
    [InlineData(5, 168)]
    public void CalculateFuel2ShouldBeCorrect(int position, int expected) {
        // 0 1 1 2 2 4 7 14 16
        var input = "16,1,2,0,4,2,7,1,2,14".Split(",").Select(int.Parse).ToArray();
        var fuel = Day07.CalculateFuel2(input, position);

        fuel.Should().Be(expected);
    }
}
