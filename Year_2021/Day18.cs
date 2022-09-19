using System.Text.RegularExpressions;
using Spectre.Console.Rendering;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 18 from year 2021
/// </summary>
internal sealed class Day18 : BaseDay {

    public Day18() {
        var input = File.ReadAllText(InputFilePath);
    }
   
    public override ValueTask<string> Solve_1() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }
    
    internal static class Reader {
        public static INode Read(string input) {
            return new Node(new Literal(1), new Literal(1));
        }
    }

    internal interface INode {
        public INode? Left { get; }
        public INode? Right { get; }
        public int Level { get; }
        public INode? Parent { get; }
    }

    internal sealed record Node(INode Left, INode Right, int Level, INode Parent) : INode { }

    internal sealed record Literal(long Value, int Level, INode Parent) : INode {
        public INode? Left => null;
        public INode? Right => null;
    }
}
