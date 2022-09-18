using System.Collections;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 16 from year 2021
/// </summary>
internal sealed class Day16 : BaseDay {
    public Packet Packet;

    public Day16() {
        Packet = ReadPacket(new BitReader(File.ReadAllText(InputFilePath)));
    }

    public Day16(string hex) {
        Packet = ReadPacket(new BitReader(hex));
    }

    public static Packet ReadPacket(BitReader reader) {
        var version = reader.ReadInteger(3);
        var type = reader.ReadInteger(3);
        var value = 0L;
        var packets = new List<Packet>();

        if (type == 4) {
            value = ReadLiteralValue(reader);
        } else {
            var lengthType = reader.ReadBool();
            if (lengthType) {
                // We have a package amount
                var packetAmount = reader.ReadInteger(11);
                for (int i = 0; i < packetAmount; i++) {
                    packets.Add(ReadPacket(reader));
                }
            } else {
                // We have the exact number of bits for the next packages
                // 15 bits
                var bitAmount = reader.ReadInteger(15);
                var startPosition = reader.Pointer;
                var endPosition = startPosition + bitAmount;
                while (reader.Pointer < endPosition) {
                    packets.Add(ReadPacket(reader));
                }
            }
        }
        
        return new Packet(type,version, value, packets.ToArray());
    }

    private static long ReadLiteralValue(BitReader reader) {
        bool isLast;
        var result = 0L;
        
        do {
            isLast = !reader.ReadBool();
            var temp = (long)reader.ReadInteger(4);
            result = result << 4 | temp;
        } while (!isLast);

        return result;
    }

    public static long SumVersions(Packet packet) {
        var summed = packet.SubPackets.Sum(SumVersions);
        return summed + packet.Version;
    }

    public static long CalculatePayload(Packet packet) {
        return 0L;
    }

    public override ValueTask<string> Solve_1() {
        var result = SumVersions(Packet);
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = CalculatePayload(Packet);
        return new ValueTask<string>($"Result: `{result}`");
    }
}

internal sealed class BitReader {
    public int Pointer { get; private set; } = 0;
    public BitArray Bits;

    public BitReader(string hex) : this(Convert.FromHexString(hex).ToReversedBitArray()){}

    public BitReader(BitArray bits) {
        Bits = bits;
    }

    public bool ReadBool() => Bits[Pointer++];

    public int ReadInteger(int bitCount) {
        var result = 0;
        var offset = bitCount - 1;
        for (var i = 0; i < bitCount; i++) {
            if (ReadBool()) {
                result |= 1 << offset - i;
            }
        }

        return result;
    }
}

internal sealed record Packet(int Type, int Version, long Value, Packet[] SubPackets);