using AdventOfCode.Year_2020;

namespace Tests.Year_2020;

public sealed class Day05Tests {
    [Theory]
    [InlineData("FBFBBFFRLR", 44, 5, 357)]
    [InlineData("BFFFBBFRRR", 70, 7, 567)]
    [InlineData("FFFBBBFRRR", 14, 7, 119)]
    [InlineData("BBFFBBFRLL", 102, 4, 820)]
    public void HeightValidationIsCorrect(string pass, int row, int col, int seat) {
        var result = new Day05.Seat(pass);

        result.Should().NotBeNull();
        result.Row.Should().Be(row);
        result.Column.Should().Be(col);
        result.SeatId.Should().Be(seat);
    }
}
