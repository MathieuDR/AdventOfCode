using AdventOfCode.Year_2020;

namespace Tests.Year_2020;

public sealed class Day02Tests {
    [Theory]
    [InlineData("1-3 a: abcde", true)]
    [InlineData("1-3 b: cdefg", false)]
    [InlineData("2-9 c: ccccccccc", false)]
    [InlineData("1-9 c: carst", true)]
    [InlineData("1-9 a: carst", false)]
    [InlineData("9-19 s: carst", false)]
    public void IsValidTobogganPassword(string line, bool isValid) {
        var password = new Day02.PasswordRecord(line);

        password.IsValidForToboggan.Should().Be(isValid);
    }
}
