namespace AdventOfCode.Year_2020;

internal sealed class Day05 : BaseDay {
    public enum Direction {
        Lower,
        Upper
    }

    public Seat[] Seats { get; init; }

    public Day05() {
        Seats = File.ReadAllLines(InputFilePath).Select(x => new Seat(x)).ToArray();
    }

    public override ValueTask<string> Solve_1() {
        return new ValueTask<string>(
            $"The highest seat is {Seats.Max(x => x.SeatId)}");
    }

    public override ValueTask<string> Solve_2() {
        var ordered = Seats.Select(x => x.SeatId).OrderBy(x => x).ToArray();
        var missingSeat = 0;
        for (var i = 1; i < ordered.Length - 1; i++) {
            if (ordered[i - 1] == ordered[i] - 1) {
                continue;
            }

            missingSeat = ordered[i] - 1;
            break;
        }

        return new ValueTask<string>(
            $"The missing seat is {missingSeat}");
    }

    internal sealed class Seat {
        public Seat(string direction) : this(ToDirections(direction)) { }

        public Seat(Direction[] directions) {
            Directions = directions;

            if (directions.Length != 10) {
                throw new ArgumentException("We need at least 10 directions.");
            }

            int[] rowRange = { 0, 127 };
            int[] colRange = { 0, 7 };
            for (var i = 0; i < directions.Length; i++) {
                if (i >= 7) {
                    colRange = Split(colRange, directions[i]);
                } else {
                    rowRange = Split(rowRange, directions[i]);
                }
            }

            Row = rowRange.First();
            Column = colRange.First();
        }

        public Direction[] Directions { get; }
        public int Row { get; }
        public int Column { get; }
        public int SeatId => Row * 8 + Column;

        private static Direction[] ToDirections(string directions) {
            return directions.ToUpperInvariant().ToType(c => c switch {
                'F' => Direction.Lower,
                'L' => Direction.Lower,
                'B' => Direction.Upper,
                'R' => Direction.Upper,
                _ => throw new ArgumentOutOfRangeException(nameof(c))
            }).ToArray();
        }

        private int[] Split(int[] toSplit, Direction directionToSplit) {
            if (toSplit[0] == toSplit[1]) {
                // Cannot split more
                return toSplit;
            }

            var value = (toSplit[1] - toSplit[0] + 1) / 2;

            if (directionToSplit == Direction.Upper) {
                toSplit[0] += value;
            } else {
                toSplit[1] -= value;
            }

            return toSplit;
        }
    }
}
