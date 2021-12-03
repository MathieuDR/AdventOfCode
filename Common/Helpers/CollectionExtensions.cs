namespace AdventOfCode.Common.Helpers;

public static class CollectionExtensions {
    public static IEnumerable<IEnumerable<bool>> Transpose(this IEnumerable<byte> bytes) {
        var bits = bytes.Select(GetBitsStartingFromLSB);
        return bits.Transpose();
    }

    public static IEnumerable<IEnumerable<bool>> Transpose(this IEnumerable<IEnumerable<bool>> bits) {
        List<List<bool>> transposed = new();
        var bitsArr = bits.Select(x => x.ToArray()).ToArray();

        var maxCols = bitsArr.Select(x => x.Length).Max();


        for (var col = 0; col < maxCols; col++) {
            var bools = new List<bool>();
            for (var row = bitsArr.Length - 1; row >= 0; row--) {
                if (bitsArr[row].Length <= col) {
                    bools.Add(false);
                } else {
                    bools.Add(bitsArr[row][col]);
                }
            }

            transposed.Add(bools);
        }

        return transposed;
    }

    public static IEnumerable<bool> GetBitsStartingFromLSB(byte b) {
        for (var i = 0; i < 8; i++) {
            yield return b % 2 != 0;
            b = (byte)(b >> 1);
        }
    }
}
