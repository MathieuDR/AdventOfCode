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

    internal readonly Func<string, string, Day18.Node> AddFromString = (s1, s2) =>
        Day18.Add(Day18.Helper.Read(s1), Day18.Helper.Read(s2));
    internal readonly Func<string, Day18.Node> ReduceString = s => Day18.Reduce(Day18.Helper.Read(s));
    internal readonly Func< Day18.Node, Day18.Node> Reduce = Day18.Reduce;
    internal readonly Func<string, Day18.Node> Addlist = s=> Day18.AddList(Day18.Helper.ReadLines(s));
    internal readonly Func<string, long> Magnitude = s => Day18.Magnitude(Day18.Helper.Read(s));

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
        //Act
        var result = Addition(Reader(left), Reader(right));

        //Assert
        result.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void Addition_ShouldReturnLeftRightPair_WithComplexNode() {
        //Arrange
        var left = "[1,2]";
        var right = "[[3,4],5]";
        var expected = "[[1,2],[[3,4],5]]";

        //Act
        var result = Addition(Reader(left), Reader(right));

        //Assert
        result.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]","[[[[0,9],2],3],4]","No literal on the left")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]","[7,[6,[5,[7,0]]]]","No literal on the right")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]","[[6,[5,[7,0]]],3]","Simple node")]
    [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,2]]]]","[[3,[2,[8,0]]],[9,[5,[4,2]]]]","Can go to the right")]
    [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]","[[3,[2,[8,0]]],[9,[5,[7,0]]]]","Explodes to the left first")]
    public void Reduce_ShouldExplodeCorrectly_WhenGivenDifferentScenarios(string input, string expected, string scenario) {
        //Arrange
        //var expectedNode = Reader(expected);

        //Act
        var result = ReduceString(input);

        //Assert
        result.ToString().Should().Be(expected, scenario);
    }
    
    [Fact]
    public void Reduce_ShouldExplodeAndSplit_WhenMultipleActons() {
        // [7,[6,[1,[4,[8,2]]]]]
        // [7,[6,[1,[12,0]]]]
        // [7,[6,[1,[[6,6],0]]]]
        // [7,[6,[7,[0,6]]]]
        //Arrange
        var input = Reader("[7,[6,[1,[4,[8,2]]]]]"); 
            
        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[7,[6,[7,[0,6]]]]");
    }
    
    [Fact]
    public void Reduce_ShouldSplit_WhenNestedDoubleSplit() {
        //Arrange
        var input = Reader("[[3,9],[[1,3],3]]");
        input.Left!.Right = new Day18.Literal(14, input.Left!.Right!.Level);
        input.Right!.Left!.Left = new Day18.Literal(18,  input.Right!.Left!.Left!.Level);
        input = Day18.Helper.FixParents(input);
            
        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[[3,[7,7]],[[[9,9],3],3]]");
    }
    
    [Fact]
    public void Reduce_ShouldSplit_WhenNestedEvenSplit() {
        //Arrange
        var input = Reader("[[3,9],[[1,3],3]]");
        input.Right!.Left!.Left = new Day18.Literal(18,  input.Right!.Left!.Left!.Level);
        input = Day18.Helper.FixParents(input);
            
        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[[3,9],[[[9,9],3],3]]");
    }
    
    [Fact]
    public void Reduce_ShouldSplit_WhenNestedUnevenSplit() {
        //Arrange
        var input = Reader("[[3,9],[[1,3],3]]");
        input.Right!.Left!.Left = new Day18.Literal(17,  input.Right!.Left!.Left!.Level);
        input = Day18.Helper.FixParents(input);
            
        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[[3,9],[[[8,9],3],3]]");
    }

    [Fact]
    public void Reduce_ShouldSplit_WhenSimpleEvenSplit() {
        //Arrange
        var input = Reader("[1,3]");
        input.Left = new Day18.Literal(12, input.Left!.Level);
        input = Day18.Helper.FixParents(input);

        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[[6,6],3]");
    }
    
    [Fact]
    public void Reduce_ShouldSplit_WhenSimpleUnevenSplit() {
        //Arrange
        var input = Reader("[1,3]");
        input.Left = new Day18.Literal(13, input.Left!.Level);
        input = Day18.Helper.FixParents(input);

        //Act
        var result = Reduce(input);

        //Assert
        result.ToString().Should().Be("[[6,7],3]");
    }

    [Fact]
    public void Addition_ShouldReduce_WhenItExplodesAfterAddition() {
        //Arrange
        var left = Reader("[[[[4,3],4],4],[7,[[8,4],9]]]");
        var right = Reader("[1,1]");
        var expected = "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]";

        //Act
        var result = Addition(left, right);

        //Assert
        result.ToString().Should().Be(expected);
    }

    [Fact]
    public void AddList_ShouldReturnCorrect_WhenNoReduce() {
        //Arrange
        var list = @"[1,1]
[2,2]
[3,3]
[4,4]";
        var expected = "[[[[1,1],[2,2]],[3,3]],[4,4]]";

        //Act
        var result = Addlist(list);

        //Assert
        result.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void AddList_ShouldReturnCorrect_WhenReducing() {
        //Arrange
        var list = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]
[6,6]";
        var expected = "[[[[5,0],[7,4]],[5,5]],[6,6]]";

        //Act
        var result = Addlist(list);

        //Assert
        result.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void Testie() {
        //Arrange
        var input = "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]";
        var expected = "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]";

        //Act
        var result = ReduceString(input);

        //Assert
        result.ToString().Should().Be(expected);
    }

    [Fact]
    public void Reducing_ShouldBeCorrect_WithBigNumber() {
        //Arrange
        var input = "[[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]";
        var expected = "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]";

        //Act
        var result = ReduceString(input);

        //Assert
        result.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]")]
    [InlineData("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]", "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]")]
    [InlineData("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]", "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]")]
    public void Add_ShouldBeCorrect_FromTwoComplex(string s1, string s2, string expected) {
        //Arrange

        //Act
        var result = AddFromString(s1, s2);

        //Assert
        result.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void AddList_ShouldReturnCorrect_WhenComplex() {
        //Arrange
        var list = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";
        var expected = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";

        //Act
        var result = Addlist(list);

        //Assert
        result.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("[9,1]", 29)]
    [InlineData("[[9,1],[1,9]]", 129)]
    [InlineData("[[1,2],[[3,4],5]]", 143)]
    [InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
    [InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
    [InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
    [InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
    [InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
    public void Magnitude_ShouldBeCorrect_WithDifferentNumbers(string number, long expected) {
        var result = Magnitude(number);

        result.Should().Be(expected);
    }

    [Fact]
    public void Magnitude_ShouldReturnCorrectMagnitude_WhenUsingAllOperations() {
        //Arrange
        var input = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
        var resultNumber = "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]";
        var expected = (long)4140;

        //Act
        var added = Addlist(input);
        var magnitude = Magnitude(added.ToString());

        //Assert
        added.ToString().Should().Be(resultNumber);
        magnitude.Should().Be(expected);
    }
}
