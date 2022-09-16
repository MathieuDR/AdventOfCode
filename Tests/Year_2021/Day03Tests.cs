using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public sealed class Day03Tests {
    [Fact]
    public void MostCommonBitReturnsTrueForOddNumbersWithMostCommonTrue() {
        var input = new[] { false, true, true, false, true, true, true };
        var result = Day03.MostCommonBit(input);

        result.Should().BeTrue();
    }

    [Fact]
    public void MostCommonBitReturnsTrueForEvenNumbersWithMostCommonTrue() {
        var input = new[] { false, true, true, false, true, true };
        var result = Day03.MostCommonBit(input);

        result.Should().BeTrue();
    }

    [Fact]
    public void MostCommonBitReturnsTrueForEvenNumbersWithevenDistribution() {
        var input = new[] { false, true, true, false, true, false };
        var result = Day03.MostCommonBit(input);

        result.Should().BeTrue();
    }


    [Fact]
    public void MostCommonBitReturnsFalseForOddNumbersWithMostCommonFalse() {
        var input = new[] { true, false, false, true, false, false, false };
        var result = Day03.MostCommonBit(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void MostCommonBitReturnsFalseForEvenNumbersWithMostCommonFalse() {
        var input = new[] { true, false, false, true, false, false };
        var result = Day03.MostCommonBit(input);

        result.Should().BeFalse();
    }
}
