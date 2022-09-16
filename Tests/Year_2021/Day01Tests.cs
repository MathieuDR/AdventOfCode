using System.Linq;
using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public sealed class Day01Tests {
    [Fact]
    public void CanSolveFirstExample() {
        var input = "199\n200\n208\n210\n200\n207\n240\n269\n260\n263";

        var day = new Day01(input);
        var output = day.GetDirectionsWithSlidingView(1);
        var ups = output.Count(x => x == Day01.Direction.Up);

        ups.Should().Be(7);
    }

    [Fact]
    public void CanSolveSecondExample() {
        var input = "199\n200\n208\n210\n200\n207\n240\n269\n260\n263";

        var day = new Day01(input);
        var output = day.GetDirectionsWithSlidingView(3);
        var ups = output.Count(x => x == Day01.Direction.Up);

        ups.Should().Be(5);
    }
}
