using System.Text.RegularExpressions;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 17 from year 2021
/// </summary>
internal sealed class Day17 : BaseDay {
    private Regex _inputRegex = new Regex(
        @"target area: x=([-0-9]+)..([-0-9]+), y=([-0-9]+)..([-0-9]+)", 
        RegexOptions.Compiled);

    public Point LeftTargetBound { get; private init; }
    public Point RightTargetBound { get; private init; }
    
    public Day17() {
        var input = File.ReadAllText(InputFilePath);
        var captures = _inputRegex.Match(input);
        var x1 = int.Parse(captures.Groups[1].Value);
        var x2 = int.Parse(captures.Groups[2].Value);
        var y1 = int.Parse(captures.Groups[3].Value);
        var y2 = int.Parse(captures.Groups[4].Value);
        LeftTargetBound = new Point(Math.Min(x1, x2), Math.Max(y1, y2));
        RightTargetBound = new Point(Math.Max(x1, x2), Math.Min(y1, y2));
    }

    private int FindHighestYVelocity() {
        bool isInRange;
        int velocity = Math.Abs(RightTargetBound.Y);
        do {
            var probe = new Probe(new Point(0, --velocity));
            isInRange = probe.IsProbeInVerticalRange(LeftTargetBound, RightTargetBound);
        } while (!isInRange);

        return velocity;
    }
    
    private int FindHighestXVelocity() {
        return RightTargetBound.X;
    }

    private int FindLowestYVelocity() {
        return RightTargetBound.Y;
    }
   
    public override ValueTask<string> Solve_1() {
        var probe = new Probe(new Point(0, FindHighestYVelocity()));
        _ = probe.IsProbeInVerticalRange(LeftTargetBound, RightTargetBound);
        var maxY = probe.Points.Max(point => point.Y);
        
        return new ValueTask<string>($"Result: `{maxY}`");
    }

    private int FindLowestXVelocity() {
        var velocity = 1;
        var abs = Math.Abs(LeftTargetBound.X);
        while (true) {
            var result =  (velocity * (velocity + 1)) / 2;
            if (result >= abs) {
                return LeftTargetBound.X > 0 ? velocity : velocity * -1;
            }

            velocity++;
        }
    }

    private List<Point> FindAllPoints() {
        var p1 = new Point(FindHighestXVelocity(), FindLowestYVelocity());
        var p2 = new Point(FindLowestXVelocity(), FindHighestYVelocity());
        var result = new List<Point>();

        var i = 0;
        
        for (int y = p1.Y; y <= p2.Y; y++) {
            for (int x = p2.X; x <= p1.X; x++) {
                i++;
                var velocity = new Point(x, y);
                var probe = new Probe(velocity);
                var inRange = probe.IsInRange(LeftTargetBound, RightTargetBound);
                
                if (inRange) {
                    result.Add(velocity);
                }
            }
        }

        Console.WriteLine($"Tried {i} probes");

        return result;
    }

    public override ValueTask<string> Solve_2() {
        var probeVelocities = FindAllPoints().Select(x => x.ToString());
        var result = probeVelocities.Count(); //string.Join(" ", probeVelocities);
        return new ValueTask<string>($"Result: `{result}`");
    }

    internal sealed class Probe {
        private Point _velocity;
        public List<Point> Points { get; init; } = new (){new Point(0,0)};

        public Probe(Point velocity) => _velocity = velocity;

        private void Step() {
            Points.Add(CalculatePoint(_velocity, Points[^1]));
            _velocity = CalculateDrag(_velocity);
        }

        public bool IsProbeInVerticalRange(Point leftBound, Point rightBound) {
            bool isInRange;
            do {
                Step();
                isInRange = IsInVerticalRange(Points[^1], leftBound, rightBound);
                
                if (isInRange) {
                    break;
                }
            } while (!IsVerticalOverShot(Points[^1], rightBound));

            return isInRange;
        }
        
        public bool IsInRange(Point leftBound, Point rightBound) {
            bool isInRange;
            do {
                Step();
                isInRange = IsInRange(Points[^1], leftBound, rightBound);
                if (isInRange) {
                    break;
                }
            } while (!IsOvershot(Points[^1], rightBound));

            return isInRange;
        }

        private Point CalculatePoint(Point velocity, Point currentPoint) => new Point(currentPoint.X + velocity.X, currentPoint.Y + velocity.Y);

        private Point CalculateDrag(Point velocity) {
            int x = velocity.X switch {
                > 0 => velocity.X - 1,
                < 0 => velocity.X + 1,
                _ => 0
            };
            return new Point(x, velocity.Y - 1 );
        }

        private bool IsVerticalOverShot(Point current, Point rightBound) 
            => IsInRange(current.Y, int.MinValue, rightBound.Y);
        
        private bool IsOvershot(Point current, Point rightBound) 
            => IsInRange(current.X, rightBound.X, int.MaxValue) || IsVerticalOverShot(current, rightBound);

        private bool IsInVerticalRange(Point point, Point leftBound, Point rightBound) 
            =>  IsInRange(point.Y, rightBound.Y, leftBound.Y);
        
        private bool IsInRange(Point point, Point leftBound, Point rightBound) 
            =>  IsInRange(point.X, leftBound.X, rightBound.X) && IsInVerticalRange(point, leftBound, rightBound);

        private bool IsInRange(int p, int left, int right) => left <= p && p <= right;
    }

    internal sealed record Point {
        public Point(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }
        public int X { get; init; }
        public int Y { get; init; }
        public void Deconstruct(out int X, out int Y) {
            X = this.X;
            Y = this.Y;
        }

        public override string ToString() => $"{X},{Y}";
    }
}
