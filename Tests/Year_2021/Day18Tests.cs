using System;
using System.Threading.Tasks;
using AdventOfCode.Year_2021;
using VerifyXunit;
using static VerifyXunit.Verifier;

namespace Tests.Year_2021;

[UsesVerify]
public sealed class Day18Tests {

    internal readonly Func<string, Day18.Node> Reader = Day18.Helper.Read;
    internal readonly Func<Day18.Node, Day18.Node, Day18.Node> Addition = Day18.Add;
    internal readonly Func<string, Day18.Node> Reduce = s => Day18.Reduce(Day18.Helper.Read(s));

    [Fact]
    public Task Reader_ShouldParseNode_WhenGivenSingle() {
        //Arrange
        var input = "[1,2]";

        //Act
        var result = Reader(input);

        //Assert
        return Verify(result);
    }

    [Fact]
    public Task Reader_ShouldParseNode_WhenGivenDoubleToTheLeft() {
        //Arrange
        var input = "[[1,2],3]";

        //Act
        var result = Reader(input);

        //Assert
        return Verify(result);
    }

    [Fact]
    public Task Reader_ShouldParseNode_WhenGivenDoubleToTheRight() {
        //Arrange
        var input = "[1,[2,3]]";

        //Act
        var result = Reader(input);

        //Assert
        return Verify(result);
    }
    
    [Fact]
    public Task Reader_ShouldParseNode_WhenGivenComplexNode() {
        //Arrange
        var input = "[[1,9],[8,5]]";
      
        //Act
        var result = Reader(input);

        //Assert
        return Verify(result);
    }

    [Fact]
    public void Reader_ShouldNotError_OnVeryComplexNode() {
        //Arrange
        var input = "[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]"; 

        //Act
        Action result = () => Reader(input);;
        
        //Assert
        result.Should().NotThrow();
    }

    [Fact]
    public void Addition_ShouldReturnLeftRightPair_WithSimpleNode() {
        //Arrange
        var left = "[1,2]";
        var right = "[2,3]";
        var expected = "[[1,2],[2,3]]";
        var expectedNode = Reader(expected);

        //Act
        var result = Addition(Reader(left), Reader(right));

        //Assert
        result.Should().BeEquivalentTo(expectedNode, options => options.IgnoringCyclicReferences());
    }
    
    [Fact]
    public void Addition_ShouldReturnLeftRightPair_WithComplexNode() {
        //Arrange
        var left = "[1,2]";
        var right = "[[3,4],5]";
        var expected = "[[1,2],[[3,4],5]]";
        var expectedNode = Reader(expected);

        //Act
        var result = Addition(Reader(left), Reader(right));

        //Assert
        result.Should().BeEquivalentTo(expectedNode, options => options.IgnoringCyclicReferences());
    }

    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]","[[[[0,9],2],3],4]","No literal on the left")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]","[7,[6,[5,[7,0]]]]","No literal on the right")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]","[[6,[5,[7,0]]],3]","Simple node")]
    [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]","[[3,[2,[8,0]]],[9,[5,[7,0]]]]","Explodes to the left first")]
    public void Reduce_ShouldExplodeCorrectly_WhenGivenDifferentScenarios(string input, string expected, string scenario) {
        //Arrange
        //var expectedNode = Reader(expected);

        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be(expected, scenario);
    }
}
