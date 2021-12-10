namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 10 from year 2021
/// </summary>
public class Day10 : BaseDay {
    private readonly char[] _endCharacters = { ')', ']', '}', '>' };
    private readonly char[][] _illegalChars;
    private readonly string[] _lines;
    private readonly string[] _nonIllegalLines;
    private readonly char[] _openCharacters = { '(', '[', '{', '<' };


    public Day10() {
        _lines = File.ReadAllLines(InputFilePath);

        // Set vars
        var illegalChars = new List<char[]>();
        var goodLines = new List<string>();
        foreach (var line in _lines) {
            var illegal = GetIllegalCharacters(line).ToArray();
            if (illegal.Length > 0) {
                illegalChars.Add(illegal);
                continue;
            }

            goodLines.Add(line);
        }

        _illegalChars = illegalChars.ToArray();
        _nonIllegalLines = goodLines.ToArray();
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

    private string GetLeftoversString(string input) {
        var openCharacter = GetOpenCharacters(input);
        var reversed = openCharacter.Reverse().ToArray();
        var result = GetCloseCharacterTwin(reversed).ToArray();


        return new string(result);
    }

    private IEnumerable<char> GetCloseCharacterTwin(char[] reversed) {
        foreach (var c in reversed) {
            var indexOfEndChar = Array.IndexOf(_openCharacters, c);
            yield return _endCharacters[indexOfEndChar];
        }
    }

    private char[] GetOpenCharacters(string input) {
        var openCharacter = new List<char>();

        foreach (var character in input) {
            if (_openCharacters.Contains(character)) {
                openCharacter.Add(character);
                continue;
            }

            var indexOfEndChar = Array.IndexOf(_endCharacters, character);
            var popped = openCharacter.Last();
            if (popped == _openCharacters[indexOfEndChar]) {
                openCharacter.RemoveAt(openCharacter.Count - 1);
            }
        }

        return openCharacter.ToArray();
    }

    public override ValueTask<string> Solve_1() {
        var result = 0;

        foreach (var chars in _illegalChars) {
            result += GetValueFromIllegalCharacter(chars.First());
        }

        // `358737`
        return new ValueTask<string>($"Result: `{result}`");
    }

    private ulong GetAutoCompleteWinner(IEnumerable<ulong> scores) {
        var arr = scores.OrderBy(x => x).ToArray();
        return arr[arr.Length / 2];
    }

    private int GetValueFromIllegalCharacter(char firstIllegal) {
        return firstIllegal switch {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => 0
        };
    }

    private ulong GetValueFromIncompleteString(string autoComplete) {
        ulong result = 0;
        foreach (var character in autoComplete) {
            result *= 5;
            result += GetValueFromIncompleteCharacter(character);
        }

        return result;
    }

    private uint GetValueFromIncompleteCharacter(char firstIllegal) {
        return firstIllegal switch {
            ')' => 1,
            ']' => 2,
            '}' => 3,
            '>' => 4,
            _ => 0
        };
    }

    public override ValueTask<string> Solve_2() {
        var autoCompletes = _nonIllegalLines.Select(GetLeftoversString).ToArray();

        var autoCompleteScores = autoCompletes.Select(GetValueFromIncompleteString);
        var result = GetAutoCompleteWinner(autoCompleteScores);
        return new ValueTask<string>($"Result: `{result}`");
    }
}
