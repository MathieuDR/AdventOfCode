using System.Collections;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 16 from year 2021
/// </summary>
internal sealed class Day16 : BaseDay {
    public Packet PayloadPacket;

    public Day16() {
        PayloadPacket = ReadPacket(new BitReader(File.ReadAllText(InputFilePath)));
    }

    public Day16(string hex) {
        PayloadPacket = ReadPacket(new BitReader(hex));
    }

    public static Packet ReadPacket(BitReader reader) {
        var version = reader.ReadInteger(3);
        var type = reader.ReadInteger(3);
        var value = 0L;
        var packets = Array.Empty<Packet>();

        if (type == 4) {
            value = ReadLiteralValue(reader);
        } else {
            packets = ReadSubPackages(reader).ToArray();
        }
        
        return new Packet(type,version, value, packets);
    }

    private static List<Packet> ReadSubPackages(BitReader reader) {
        var result = new List<Packet>();
        var lengthType = reader.ReadBool();
        
        if (lengthType) {
            // We have a package amount
            var packetAmount = reader.ReadInteger(11);
            for (int i = 0; i < packetAmount; i++) {
                result.Add(ReadPacket(reader));
            }
        } else {
            // We have the exact number of bits for the next packages
            // 15 bits
            var bitAmount = reader.ReadInteger(15);
            var startPosition = reader.Pointer;
            var endPosition = startPosition + bitAmount;
            while (reader.Pointer < endPosition) {
                result.Add(ReadPacket(reader));
            }
        }

        return result;
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
        return packet.Type switch {
            0 => packet.SubPackets.Sum(CalculatePayload),
            1 => packet.SubPackets.Aggregate(1L, (seed, p2) => seed * CalculatePayload(p2)),
            2 => packet.SubPackets.Min(CalculatePayload),
            3 => packet.SubPackets.Max(CalculatePayload),
            4 => packet.Value,
            5 => CalculatePayload(packet.SubPackets[0]) > CalculatePayload(packet.SubPackets[1]) ? 1L : 0L,
            6 => CalculatePayload(packet.SubPackets[0]) < CalculatePayload(packet.SubPackets[1]) ? 1L : 0L,
            7 => CalculatePayload(packet.SubPackets[0]) == CalculatePayload(packet.SubPackets[1]) ? 1L : 0L,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override ValueTask<string> Solve_1() {
        var result = SumVersions(PayloadPacket);
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = CalculatePayload(PayloadPacket);
        return new ValueTask<string>($"Result: `{result}`");
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
}
