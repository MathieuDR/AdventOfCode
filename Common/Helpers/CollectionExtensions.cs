namespace AdventOfCode.Common.Helpers;

public static class CollectionExtensions {
    public static IEnumerable<IEnumerable<bool>> Transpose(this IEnumerable<byte> bytes) {
        var bits = bytes.Select(GetBitsStartingFromLSB);
        return bits.Transpose();
    }

    public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> matrix) {
        List<List<T>> transposed = new();
        var matrixArr = matrix.Select(x => x.ToArray()).ToArray();

        var maxCols = matrixArr.Select(x => x.Length).Max();


        for (var col = 0; col < maxCols; col++) {
            var newRow = new List<T>();
            for (var row = matrixArr.Length - 1; row >= 0; row--) {
                newRow.Add((matrixArr[row].Length <= col ? default : matrixArr[row][col]) ?? default);
            }

            transposed.Add(newRow);
        }

        return transposed;
    }

    public static IEnumerable<bool> GetBitsStartingFromLSB(byte b) {
        for (var i = 0; i < 8; i++) {
            yield return b % 2 != 0;
            b = (byte)(b >> 1);
        }
    }

    public static IEnumerable<T> ToType<T, TT>(this IEnumerable<TT> strings, Func<TT, T> act) {
        var list = new List<T>();
        foreach (var s in strings) {
            list.Add(act(s));
        }

        return list;
    }
}
