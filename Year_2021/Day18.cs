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

        return Reduce(result);
    }

    private static Node Change(Node node, Func<Node, Node> func) {
        var newNode = func.Invoke(node);
        var left = newNode.Left is null ? null : Change(newNode.Left, func);
        var right = newNode.Right is null ? null : Change(newNode.Right, func);

        return newNode with { Left = left, Right = right };
    }

    public static Node Reduce(Node node) {
        Explode(node, false);
        var newNode =  Change(node, n => n is Literal { Value: > 9 } ? Split(n) : n);
        newNode = Helper.FixParents(newNode);
        var str = newNode.ToString();
        Explode(newNode, false);
        
        if (str == newNode.ToString()) {
            return newNode;
        }

        return Reduce(newNode);
    }

    private static Node Split(Node node) {
        if (node is not Literal { Value: > 9 } l) {
            return node;
        }

        var newLevel = l.Level + 1;
        var p = new Pair(new Literal((short)Math.Floor((decimal)(l.Value / 2F)), newLevel),
            new Literal((short)Math.Ceiling((decimal)(l.Value / 2F)), newLevel), newLevel);

        // p.Left!.Parent = p;
        // p.Right!.Parent = p;
        // p.Parent = node.Parent;
        // // }

        return p;
    }

    // we´re mutating this boys
    private static void Explode(Node node, bool isLeft) {
        if (node.Level < 4) {
            if (node.Left is not null) {
                Explode(node.Left, true);
            }

            if (node.Right is not null) {
                Explode(node.Right, false);
            }

            return;
        }

        // we´re deeper then 4. must be literal
        var leftLiteral = (Literal)node.Left!;
        var rightLiteral = (Literal)node.Right!;

        var newValue = new Literal(0, 3) { Parent = node.Parent!.Parent };

        AddToFirst(leftLiteral.Value, node.Parent!, true, node);
        AddToFirst(rightLiteral.Value, node.Parent!, false, node);

        if (isLeft) {
            node.Parent.Left = newValue;
        } else {
            node.Parent.Right = newValue;
        }
    }

    private static void AddToFirst(short value, Node? current, bool left, Node previousNode) {
        if (current is null) {
            return;
        }

        var toCheckNode = left ? current.Left : current.Right;

        if (toCheckNode is Literal literal) {
            var newLiteral = literal with { Value = (short)(literal.Value + value) };
            if (left) {
                current.Left = newLiteral;
            } else {
                current.Right = newLiteral;
            }

            return;
        }

        if (toCheckNode is Pair pair && pair.ToString() != previousNode.ToString()) {
            AddToFirst(value, pair, !left, previousNode);
        } else {
            AddToFirst(value, current.Parent, left, current);
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
        public static Node Read(string input) => FixParents(Read(input.AsSpan(), 0, out _));


        public static Node FixParents(Node current, Node? parent = null) {
            if (current.Left is not null) {
                FixParents(current.Left, current);
            }

            if (current.Right is not null) {
                FixParents(current.Right, current);
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
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public int Level { get; init; }

        public bool HasParent => Parent is not null; // purely for verify

        public void Deconstruct(out Node? Left, out Node? Right, out int Level) {
            Left = this.Left;
            Right = this.Right;
            Level = this.Level;
        }

        public override string ToString() => $"[{Left},{Right}]";
    }

    internal sealed record Pair(Node? Left, Node? Right, int Level) : Node(Left, Right, Level) {
        public override string ToString() => base.ToString();
    }

    internal sealed record Literal(short Value, int Level) : Node(null, null, Level) {
        public override string ToString() => Value.ToString();
    }
}
