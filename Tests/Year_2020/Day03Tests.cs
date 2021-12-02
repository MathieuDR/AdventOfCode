using System.Threading.Tasks;
using AdventOfCode.Year_2020;

namespace Tests.Year_2020;

public class Day03Tests {
    [Fact]
    public async Task canCalculateCorrectCollisions() {
        var input =
            "..##.......\n#...#...#..\n.#....#..#.\n..#.#...#.#\n.#...##..#.\n..#.##.....\n.#.#.#....#\n.#........#\n#.##...#...\n#...##....#\n.#..#...#.#";

        var day = new Day03(input);
        var solve = await day.Solve_1();

        solve.Should().Be("We hit 7 trees");
    }

    [Fact]
    public async Task canCalculateCorrectMultipleCollisions() {
        var input =
            "..##.......\n#...#...#..\n.#....#..#.\n..#.#...#.#\n.#...##..#.\n..#.##.....\n.#.#.#....#\n.#........#\n#.##...#...\n#...##....#\n.#..#...#.#";

        var day = new Day03(input);
        var solve = await day.Solve_2();

        solve.Should().Be("We hit 336 trees");
    }
}
