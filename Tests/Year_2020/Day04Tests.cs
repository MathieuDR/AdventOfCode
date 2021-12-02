using AdventOfCode.Year_2020;

namespace Tests.Year_2020;

public class Day04Tests {
    [Fact]
    public void NumberBetweenReturnsFalseForOverTheLimit() {
        var result = Day04.Passport.NumberBetween("2919", 1920, 2020);
        result.Should().BeFalse();
    }

    [Fact]
    public void NumberBetweenReturnsFalseForUnderTheLimit() {
        var result = Day04.Passport.NumberBetween("1900", 1920, 2020);
        result.Should().BeFalse();
    }

    [Fact]
    public void NumberBetweenReturnsTrueForOnTheBottomLimit() {
        var result = Day04.Passport.NumberBetween("1920", 1920, 2020);
        result.Should().BeTrue();
    }

    [Fact]
    public void NumberBetweenReturnsTrueForOnTheUpperLimit() {
        var result = Day04.Passport.NumberBetween("2020", 1920, 2020);
        result.Should().BeTrue();
    }

    [Fact]
    public void NumberBetweenReturnsTrueForInLimit() {
        var result = Day04.Passport.NumberBetween("2000", 1920, 2020);
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("190", false)]
    [InlineData("astst", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("arstascm", false)]
    [InlineData("70bs", false)]
    [InlineData("140cm", false)]
    [InlineData("200cm", false)]
    [InlineData("150cm", true)]
    [InlineData("193cm", true)]
    [InlineData("183cm", true)]
    [InlineData("50in", false)]
    [InlineData("100in", false)]
    [InlineData("59in", true)]
    [InlineData("76in", true)]
    [InlineData("70in", true)]
    public void HeightValidationIsCorrect(string height, bool valid) {
        var result = Day04.Passport.ValidateHeight(height);

        result.Should().Be(valid);
    }

    [Theory]
    [InlineData("190", false)]
    [InlineData("astst", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("arstascm", false)]
    [InlineData("70bs", false)]
    [InlineData("#rstrst", false)]
    [InlineData("#0000", false)]
    [InlineData("#000p00", false)]
    [InlineData("#000000", true)]
    [InlineData("#00a09f", true)]
    [InlineData("000000", false)]
    public void HairColorValidationIsCorrect(string hair, bool valid) {
        var result = Day04.Passport.ValidateHair(hair);

        result.Should().Be(valid);
    }

    [Theory]
    [InlineData("brown", false)]
    [InlineData("astst", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("arstascm", false)]
    [InlineData("70bs", false)]
    [InlineData("#rstrst", false)]
    [InlineData("#0000", false)]
    [InlineData("#000p00", false)]
    [InlineData("amb", true)]
    [InlineData("blu", true)]
    [InlineData("brn", true)]
    [InlineData("gry", true)]
    [InlineData("grn", true)]
    [InlineData("hzl", true)]
    [InlineData("oth", true)]
    public void EyeColorValidationIsCorrect(string eyeColor, bool valid) {
        var result = Day04.Passport.ValidateEyes(eyeColor);

        result.Should().Be(valid);
    }

    [Theory]
    [InlineData("000000001", true)]
    [InlineData("0123456789", false)]
    public void PassportIdValidationIsCorrect(string eyeColor, bool valid) {
        var result = Day04.Passport.ValidatePid(eyeColor);

        result.Should().Be(valid);
    }
}
