using System.Collections;

namespace AdventOfCode.Common.Helpers;

public static class ByteExtensions {
    public static byte ReverseBitsWith4Operations(this byte b) => (byte)((((b * 0x80200802ul) & 0x0884422110ul) * 0x0101010101ul) >> 32);

    public static byte[] ToReversedByteArray(this BitArray b) {
        var result = new byte[b.Length / 8];
        b.CopyTo(result, 0);
        return result.Select(x => x.ReverseBitsWith4Operations()).ToArray();
    }

    public static BitArray ToReversedBitArray(this byte[] bytes) => new(bytes.Select(x => x.ReverseBitsWith4Operations()).ToArray());

    public static byte GetBytePart(this byte bits, int mask, int shift) => (byte)((byte)(bits & mask) >> shift);
}
