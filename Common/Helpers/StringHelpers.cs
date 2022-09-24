namespace AdventOfCode.Common.Helpers;

public static class StringHelpers {
    public static IEnumerable<string> ReadLines(this string input) {
        if (string.IsNullOrWhiteSpace(input)) {
            yield break;
        }

        using var reader = new StringReader(input);
        while (reader.ReadLine() is { } line) {
            yield return line;
        }
    }
}
