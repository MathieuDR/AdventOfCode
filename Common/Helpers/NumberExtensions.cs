using System.Collections;

namespace AdventOfCode.Common.Helpers;

public static class NumberExtensions {
    public static IEnumerable<IEnumerable<int>> GetNumbersThatSumTo(this int[] numbers, int amountOfNumbers, int amountToSumTo) {
        var set = new HashSet<HashSet<int>>();
        var breakPoint = amountOfNumbers - 1;

        foreach (var current in numbers) {
            if (current > amountToSumTo) {
                continue;
            }

            var candidates = set.Where(x => x.Count <= breakPoint && x.Sum() + current <= amountToSumTo).ToHashSet();
            foreach (var candidate in candidates) {
                if (candidate.Count == breakPoint && candidate.Sum() + current == amountToSumTo) {
                    yield return candidate.Concat(new[] { current }).ToArray();
                } else if (candidate.Count != breakPoint) {
                    var newSet = new HashSet<int>(candidate) { current };
                    set.Add(newSet);
                }
            }

            // Add current to set
            set.Add(new HashSet<int> { current });
        }
    }

    public static int ToInt(this IEnumerable<bool> bits) {
        var bitArr = new BitArray(bits.ToArray());
        var bytes = bitArr.BitArrayToByteArray(4);
        return BitConverter.ToInt32(bytes);
    }

    public static byte[] BitArrayToByteArray(this BitArray bits, int? size = null) {
        var minSize = (bits.Length - 1) / 8 + 1;
        if (!size.HasValue) {
            size = minSize;
        }

        if (minSize > size) {
            throw new ArgumentException("There are too many bits for this size");
        }

        var result = new byte[size.Value];
        bits.CopyTo(result, 0);
        return result;
    }
}
