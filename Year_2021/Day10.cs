namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 10 from year 2021
/// </summary>
public class Day10 : BaseDay {
    private readonly string[] _lines;
    private readonly char[] _openCharacters = { '(', '[', '{', '<' };
    private readonly char[] _endCharacters = { ')', ']', '}', '>' };


    public Day10() {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private IEnumerable<char> GetIllegalCharacters(string input) {
        //List<char> characters = new();
        var openCharacter = new List<char>();

        foreach (var character in input) {
            if (_openCharacters.Contains(character)) {
                openCharacter.Add(character);
                continue;
            }

            if (openCharacter.Count == 0) {
                // There are no open ones, so we have an issue!
                yield return character;
                //characters.Add(character);
                continue;
            }

            var indexOfEndChar = Array.IndexOf(_endCharacters, character);
            var popped = openCharacter.Last();
            if (popped == _openCharacters[indexOfEndChar]) {
                // Correct
                // Remove last character
                openCharacter.RemoveAt(openCharacter.Count - 1);
                continue;
            }

            yield return character;
        }
    }

    public override ValueTask<string> Solve_1() {
        var result = 0;
        foreach (var line in _lines) {
            var firstIllegal = GetIllegalCharacters(line).FirstOrDefault();
            result += GetValueFromCharacter(firstIllegal);
        }

        return new ValueTask<string>($"Result: `{result}`");
    }

    private int GetValueFromCharacter(char firstIllegal) {
        return firstIllegal switch {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => 0
        };
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }
}
