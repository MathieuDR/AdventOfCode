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
   
    public override ValueTask<string> Solve_1() {
        bool isInRange;
        int start = Math.Abs(RightTargetBound.Y);
        int maxY;
        do {
            var probe = new Probe(new Point(0, --start));
            isInRange = probe.IsProbeInVerticalRange(LeftTargetBound, RightTargetBound);
            maxY = probe.Points.Max(point => point.Y);
        } while (!isInRange);
        
        return new ValueTask<string>($"Result: `{maxY}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0L;
        return new ValueTask<string>($"Result: `{result}`");
    }

    internal sealed class Probe {
        private Point _velocity;
        public List<Point> Points { get; init; } = new ();

        public Probe(Point velocity) => _velocity = velocity;

        public bool IsProbeInVerticalRange(Point leftBound, Point rightBound) {
            Points.Add(_velocity);
            bool isInRange;
            do {
                _velocity = CalculateDrag(_velocity);
                Points.Add(CalculatePoint(_velocity, Points[^1]));

                isInRange = IsInVerticalRange(Points[^1], leftBound, rightBound);
                if (isInRange) {
                    break;
                }

            } while (!IsVerticalOverShot(Points[^1], rightBound));

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
            
       
        private bool IsInVerticalRange(Point point, Point leftBound, Point rightBound) 
            =>  IsInRange(point.Y, rightBound.Y, leftBound.Y);

        private bool IsInRange(int p, int left, int right) => left <= p && p <= right;
    }

    internal sealed record Point(int X, int Y);
}
