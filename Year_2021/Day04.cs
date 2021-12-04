namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 23 from year 2021
/// </summary>
public class Day04 : BaseDay {
    private readonly BingoBoard[] _boards;
    private readonly int[] _draws;
    private readonly int drawIndex = 0;

    public Day04() {
        var input = File.ReadAllText(InputFilePath);
        var splits = input.Split(Environment.NewLine + Environment.NewLine);
        _draws = splits[0].Split(",").Select(int.Parse).ToArray();
        _boards = splits.Skip(1).Select(x => new BingoBoard(x)).ToArray();

        // Subscribe our boards
        foreach (var board in _boards) {
            board.Subscribe(this);
        }
    }

    public event Func<int, BingoBoard> DrawNumber;

    public override ValueTask<string> Solve_1() {
        var result = 0;
        for (var index = drawIndex; index < _draws.Length; index++) {
            var draw = _draws[index];
            var winningBoard = OnDrawNumber(draw).FirstOrDefault();
            if (winningBoard is not null) {
                result = winningBoard.Score;
                break;
            }
        }

        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        BingoBoard last = null;

        // Continuing our number draw
        for (var index = drawIndex; index < _draws.Length; index++) {
            var draw = _draws[index];
            var winningBoard = OnDrawNumber(draw).LastOrDefault();
            if (winningBoard is not null) {
                // This round has a winner, lets update
                last = winningBoard;

                // There are no more boards participating
                if (DrawNumber is null) {
                    break;
                }
            }
        }

        return new ValueTask<string>($"Result: `{last?.Score ?? -1}`");
    }

    protected virtual IEnumerable<BingoBoard> OnDrawNumber(int arg) {
        if (DrawNumber == null) {
            return null;
        }

        // Adds all the boards in a list
        var list = new List<BingoBoard>();
        foreach (var @delegate in DrawNumber.GetInvocationList()) {
            var result = @delegate.DynamicInvoke(arg);
            if (result is not null && result is BingoBoard board) {
                board.UnSubscribe(this);
                list.Add(board);
            }
        }

        return list;
    }

    public class BingoBoard {
        private readonly int[][] _board;
        private readonly bool[][] _drawn;
        private int _lastNumber;

        private bool _won;
        private int? _score;

        public BingoBoard(string input) {
            _board = input.Split(Environment.NewLine)
                .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray();
            
            _drawn = _board.Select(x => new bool[x.Length]).ToArray();
        }

        public void Subscribe(Day04 day) {
            day.DrawNumber += OnNewNumber;
        }

        public void UnSubscribe(Day04 day) {
            day.DrawNumber -= OnNewNumber;
        }

        private BingoBoard OnNewNumber(int number) {
            _lastNumber = number;
            for (var i = 0; i < _board.Length; i++) {
                var row = _board[i];
                for (var j = 0; j < row.Length; j++) {
                    if (number == row[j]) {
                        _drawn[i][j] = true;
                        if (IsWon(i, j)) {
                            return this;
                        }

                        // We did not win, but we can stop the loog
                        break;
                    }
                }
            }

            return null;
        }

        private bool IsWon(int rowIndex, int colIndex) {
            var rowWon = true;
            var colWon = true;

            for (var i = 0; i < _board.Length; i++) {
                var row = _board[i];
                for (var j = 0; j < row.Length; j++) {
                    if (i == rowIndex) {
                        rowWon &= _drawn[i][j];
                    }

                    if (j == colIndex) {
                        colWon &= _drawn[i][j];
                    }

                    if (!rowWon && !colWon) {
                        // Are we still in the running?
                        break;
                    }
                }
            }

            _won = rowWon || colWon;
            return _won;
        }

        public int Score => _score ??= CalculateScore();

        private int CalculateScore() {
            if (!_won) {
                return -1;
            }

            var score = 0;
            for (var i = 0; i < _board.Length; i++) {
                var row = _board[i];
                for (var j = 0; j < row.Length; j++) {
                    if (!_drawn[i][j]) {
                        score += _board[i][j];
                    }
                }
            }

            return score * _lastNumber;
        }
    }
}
