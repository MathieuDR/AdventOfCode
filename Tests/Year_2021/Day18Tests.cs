using System;
using System.Collections;
using AdventOfCode.Year_2021;
using FluentAssertions.Equivalency;

namespace Tests.Year_2021;

public sealed class Day18Tests {
    [Fact]
    public void Reader_ShouldParseNode_WhenGivenSingle() {
        //Arrange
        var input = "[1,2]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should().BeEquivalentTo(new Day18.Node(new Day18.Literal(1), new Day18.Literal(2)));
    }
    
    [Fact]
    public void Reader_ShouldParseNode_WhenGivenDoubleToTheLeft() {
        //Arrange
        var input = "[[1,2],3]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should().BeEquivalentTo(new Day18.Node( new Day18.Node(new Day18.Literal(1), new Day18.Literal(2)),new Day18.Literal(3)));
    }
    
    [Fact]
    public void Reader_ShouldParseNode_WhenGivenDoubleToTheRight() {
        //Arrange
        var input = "[1,[2,3]]";

        //Act
        var result = Day18.Reader.Read(input);

        //Assert
        result.Should().BeEquivalentTo(new Day18.Node(new Day18.Literal(1), new Day18.Node(new Day18.Literal(2), new Day18.Literal(3))));
    }
}
