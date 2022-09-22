using System.Diagnostics;
using System.Globalization;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 18 from year 2021
/// </summary>
internal sealed class Day18 : BaseDay {
    public Day18() {
        var input = File.ReadAllText(InputFilePath);
    }

    public static Node Add(Node left, Node right) {
        var result = new Pair(
            Change(left, node => node with { Level = node.Level + 1 }),
            Change(right, node => node with { Level = node.Level + 1 }),
            0);

        // adding parents
        if (result.Left is not null) {
            result.Left.Parent = result;
        }
        
        if (result.Right is not null) {
            result.Right.Parent = result;
        }

        return result;
    }

    private static Node Change(Node node, Func<Node, Node> func) {
        var newNode = func.Invoke(node);
        var left = newNode.Left is null ? null : Change(newNode.Left, func);
        var right = newNode.Right is null ? null : Change(newNode.Right, func);

        return newNode with { Left = left, Right = right };
    }

    public static Node Reduce(Node node) {
        var newNode = node.Level >= 4 ? Explode(node) : node;
       _ = newNode.Left is null ? null : Reduce(newNode.Left);
        var right = newNode.Right is null ? null : Reduce(newNode.Right);
        var left2 = newNode.Left is null ? null : Reduce(newNode.Left);

        return newNode with { Left = left2, Right = right };
    }

    private static Node Explode(Node node) {
        if (node is not Pair literalPair) {
            throw new Exception("expected literal");
        }

        Debug.Assert(literalPair.Left != null, "literalPair.Left != null");
        var leftLiteral = (Literal)literalPair.Left;
        Debug.Assert(literalPair.Right != null, "literalPair.Right != null");
        var rightLiteral = (Literal)literalPair.Right;

        Debug.Assert(node.Parent != null, "node.Parent != null");
        AddToFirst(leftLiteral.Value, node.Parent, true);
        AddToFirst(rightLiteral.Value, node.Parent, false);

        return new Literal(0, node.Level - 1){ Parent = node.Parent.Parent};
    }

    private static void AddToFirst(short value, Node current, bool left = true) {
        Node? currentOrNull = current;
        while (currentOrNull is not null) {
            if (left && current.Left is Literal leftLiteral) {
                current.Left = leftLiteral with { Value = (short)(leftLiteral.Value + value) };
                return;
            }
            
            if (!left && current.Right is Literal rightLiteral) {
                current.Right = rightLiteral with { Value = (short)(rightLiteral.Value + value) };
                return;
            }

            currentOrNull = currentOrNull.Parent;
        }
    }

    public override ValueTask<string> Solve_1() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }

    internal static class Helper {
        public static Node Read(string input) => AddParentLink(Read(input.AsSpan(), 0, out _));


        public static Node AddParentLink(Node current, Node? parent = null) {
            if (current.Left is not null) {
                AddParentLink(current.Left, current);
            }

            if (current.Right is not null) {
                AddParentLink(current.Right, current);
            }

            if (parent is not null) {
                return current.Parent = parent;
            }

            return current;
        }

        private static Node ReadPart(ReadOnlySpan<char> input, int level, out ReadOnlySpan<char> leftover) {
            Node result;
            var firstChar = input.Slice(1, 1);
            if (firstChar[0] == '[') {
                result = Read(input.Slice(1), level + 1, out input);
                leftover = input;
            } else if (int.TryParse(firstChar, NumberStyles.Integer, CultureInfo.InvariantCulture, out var literal)) {
                result = new Literal((short)literal, level);
                leftover = input.Slice(2); // remove the number
            } else {
                throw new Exception("Unexpected char: " + firstChar[0]);
            }

            return result;
        }

        private static Node Read(ReadOnlySpan<char> input, int level, out ReadOnlySpan<char> leftOver) {
            var left = ReadPart(input, level, out input);

            if (input[0] != ',') {
                throw new Exception("expecting, found: " + input[0]);
            }

            var right = ReadPart(input, level, out input);

            leftOver = input.Slice(1);
            return new Pair(left, right, level);
        }
    }

    internal record Node {
        public Node(Node? Left, Node? Right, int Level) {
            this.Left = Left;
            this.Right = Right;
            this.Level = Level;
        }
        public Node? Parent { get; set; }
        public Node? Left { get; set; } // boo :c 
        public Node? Right { get; set; } // boo :c 
        public int Level { get; init; }
        public void Deconstruct(out Node? Left, out Node? Right, out int Level) {
            Left = this.Left;
            Right = this.Right;
            Level = this.Level;
        }
        
        public bool HasParent => Parent is not null; // purely for verify

        public override string ToString() => $"[{Left},{Right}]";
    }

    internal sealed record Pair(Node? Left, Node? Right, int Level) : Node(Left, Right, Level) {

        public override string ToString() => base.ToString();
    }

    internal sealed record Literal(short Value, int Level) : Node(null, null, Level) {
        public override string ToString() => Value.ToString();
    }
}
