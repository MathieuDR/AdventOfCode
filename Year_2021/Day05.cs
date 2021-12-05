namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 23 from year 2021
/// </summary>
public class Day05 : BaseDay {
    private readonly HydroThermalVent[] _vents;
    private readonly SeaFloor _seaFloor;

    public Day05() {
        var input = File.ReadAllLines(InputFilePath);
        _vents = input.Select(x => new HydroThermalVent(x)).ToArray();
        _seaFloor = new SeaFloor(_vents);
    }


    public override ValueTask<string> Solve_1() {
        return new ValueTask<string>($"Result: `{_seaFloor.HowManyNOverlaps(2)}`");
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>($"Result: `{-1}`");
    }

    public class SeaFloor {
        private readonly HydroThermalVent[] _hydroThermalVents;
        private int[][] _floor;
        private (int xBound, int yBound) _floorBound;

        public SeaFloor(HydroThermalVent[] hydroThermalVents) {
            _hydroThermalVents = hydroThermalVents;
            InitFloor();
        }

        private void InitFloor() {
            CreateSeaFloorArray();
            FillSeaFloor();
        }

        private void FillSeaFloor() {
            foreach (var vent in _hydroThermalVents) {
                if (vent.IsDiagonal) {
                    continue;
                }

                if (vent.IsHorizontal) {
                    for (var i = vent.StartCoordinate.x; i <= vent.EndCoordinate.x; i++) {
                        _floor[vent.StartCoordinate.y][i] += 1;
                    }
                } else if (vent.IsVertical) {
                    for (var i = vent.StartCoordinate.y; i <= vent.EndCoordinate.y; i++) {
                        _floor[i][vent.StartCoordinate.x] += 1;
                    }
                }
            }
        }

        private void CreateSeaFloorArray() {
            _floorBound = GetMapBounds();
            _floor = new int[_floorBound.yBound][];
            for (var i = 0; i < _floor.Length; i++) {
                //_floor[i] = GetVentsOnRow(i);
                _floor[i] = new int[_floorBound.xBound];
            }
        }

        public int HowManyNOverlaps(int n) {
            var result = 0;
            foreach (var row in _floor) {
                foreach (var i in row) {
                    if (i >= n) {
                        result++;
                    }
                }
            }

            return result;
        }

        private (int xBound, int yBound) GetMapBounds() {
            int x = 0, y = 0;
            foreach (var hydroThermalVent in _hydroThermalVents) {
                x = Math.Max(hydroThermalVent.StartCoordinate.x, x);
                x = Math.Max(hydroThermalVent.EndCoordinate.x, x);
                y = Math.Max(hydroThermalVent.StartCoordinate.x, y);
                y = Math.Max(hydroThermalVent.EndCoordinate.x, y);
            }

            // +1 since it's 0 based
            return (x + 1, y + 1);
        }
    }

    public class HydroThermalVent {
        public (int x, int y) StartCoordinate { get; set; }
        public (int x, int y) EndCoordinate { get; set; }

        public bool IsHorizontal => StartCoordinate.y == EndCoordinate.y;
        public bool IsVertical => StartCoordinate.x == EndCoordinate.x;
        public bool IsDiagonal => !IsHorizontal && !IsVertical;

        public HydroThermalVent(string input) {
            var splits = input.Split("->", StringSplitOptions.TrimEntries).Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray();
            StartCoordinate = (splits[0][0], splits[0][1]);
            EndCoordinate = (splits[1][0], splits[1][1]);

            if (IsHorizontal && StartCoordinate.x > EndCoordinate.x || IsVertical && StartCoordinate.y > EndCoordinate.y) {
                (EndCoordinate, StartCoordinate) = (StartCoordinate, EndCoordinate);
            }
        }
    }
}
