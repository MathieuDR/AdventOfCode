using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public sealed class Day18Tests {
    [Fact]
    public void Reader_ShouldParseNode_WhenGivenSingle() {
        //Arrange
        var input = "[1,2]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should().BeEquivalentTo(new Day18.Node(new Day18.Literal(1, 0), new Day18.Literal(2, 0), 0));
    }

    [Fact]
    public void Reader_ShouldParseNode_WhenGivenDoubleToTheLeft() {
        //Arrange
        var input = "[[1,2],3]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should()
            .BeEquivalentTo(new Day18.Node(new Day18.Node(new Day18.Literal(1, 1), new Day18.Literal(2, 1), 1), new Day18.Literal(3, 0), 0));
    }

    [Fact]
    public void Reader_ShouldParseNode_WhenGivenDoubleToTheRight() {
        //Arrange
        var input = "[1,[2,3]]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should()
            .BeEquivalentTo(new Day18.Node(new Day18.Literal(1, 0), new Day18.Node(new Day18.Literal(2, 1), new Day18.Literal(3, 1), 1), 0));
    }
    
    [Fact]
    public void Reader_ShouldParseNode_WhenGienComplexNode() {
        //Arrange
        var input = "[[1,9],[8,5]]";
        var expected = new Day18.Node(new Day18.Node(new Day18.Literal(1, 1), new Day18.Literal(9, 1), 1),
            new Day18.Node(new Day18.Literal(8, 1), new Day18.Literal(5, 1), 1), 0);

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should()
            .BeEquivalentTo(expected);
    }
}
