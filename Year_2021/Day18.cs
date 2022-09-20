using System.Globalization;
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
        public static INode Read(string input) => Read(input.AsSpan(), 0, out _);
        private static INode Read(ReadOnlySpan<char> input, int level, out ReadOnlySpan<char> leftOver) {
            INode left = null, right = null;
            var firstChar = input.Slice(1, 1);
            if (firstChar[0] == '[') {
                left = Read(input.Slice(1), level + 1, out input);
            }else if (int.TryParse(firstChar, NumberStyles.Integer, CultureInfo.InvariantCulture, out int literal)) {
                left = new Literal((short)literal, level);
                input = input.Slice(2); // remove the number
            } else {
                throw new Exception("Unexpected char: " + firstChar[0]);
            }

            if (input[0] != ',') {
                throw new Exception("expecting, found: " + firstChar[0]);
            }
            
            var secondChar = input.Slice(1, 1);
            if (secondChar[0] == '[') {
                right = Read(input.Slice(1), level + 1, out input);
            }else if (int.TryParse(secondChar, NumberStyles.Integer, CultureInfo.InvariantCulture, out int literal)) {
                right = new Literal((short)literal, level);
                input = input.Slice(2); // remove the number
            } else {
                throw new Exception("Unexpected char: " + secondChar[0]);
            }

            leftOver = input.Slice(1); // remove the ]
            return new Node(left, right, level);
        }
    }

    internal interface INode {
        public INode? Left { get; }
        public INode? Right { get; }
        public int Level { get; }
        // public INode? Parent { get; }
    }

    internal sealed record Node(INode Left, INode Right, int Level) : INode { }

    internal sealed record Literal(short Value, int Level) : INode {
        public INode? Left => null;
        public INode? Right => null;
    }
}
