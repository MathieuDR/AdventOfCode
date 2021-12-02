using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public class Day02Tests {
    [Fact]
    public void CalculateDepthIsCorrect() {
        var input = "forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2";


        var (depth, _) = Day02.CalculatePosition(input.Split("\n"));
        depth.Should().Be(10);
    }

    [Fact]
    public void CalculateHorizontalIsCorrect() {
        var input = "forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2";


        var (_, horizontal) = Day02.CalculatePosition(input.Split("\n"));
        horizontal.Should().Be(15);
    }

    [Fact]
    public void CalculateDepthIsCorrectWithAim() {
        var input = "forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2";


        var (depth, _) = Day02.CalculatePositionWithAim(input.Split("\n"));
        depth.Should().Be(60);
    }

    [Fact]
    public void CalculateHorizontalIsCorrectWithAim() {
        var input = "forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2";


        var (_, horizontal) = Day02.CalculatePositionWithAim(input.Split("\n"));
        horizontal.Should().Be(15);
    }
}
