using System.Collections;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 16 from year 2021
/// </summary>
internal sealed class Day16 : BaseDay {
    private readonly byte[] _bytes;
    private readonly string _hex;
    private readonly BitArray _bitArray;

    public Day16() {
        _hex = File.ReadAllText(InputFilePath);
        _bytes = ConvertToBytes(_hex, 0);
        _bitArray = _bytes.ToReversedBitArray();
    }

    private byte[] ConvertToBytes(string hex, int startIndex, int? length = null) =>
        ConvertToBytes(hex, startIndex, length ?? hex.Length - startIndex);

    private byte[] ConvertToBytes(string hex, int startIndex, int length) => ConvertToBytes(hex.Substring(startIndex, length));

    private byte[] ConvertToBytes(string hex) => Convert.FromHexString(hex);


    public override ValueTask<string> Solve_1() {
        var result = new Packet(_bitArray).SummedVersion;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = 0;
        return new ValueTask<string>($"Result: `{result}`");
    }
}

internal sealed class Packet {
    public int Length => 6 + Payload.Length;

    private const int TypeMask = 7 << 2;
    private const int VersionMask = 7 << 5;
    private byte[] Bits { get; }
    public byte Version { get; }

    public int SummedVersion => Version + Payload.SummedVersion;
    public byte Type { get; }

    public byte[] LeftOver { get; }
    private Payload Payload { get; }

    public Packet(BitArray bits) {
        var bytes = new byte[bits.Length / 8];
        bits.CopyTo(bytes, 0);
        var @byte = bytes[0].ReverseBitsWith4Operations();

        Version = @byte.GetBytePart(VersionMask, 5);
        Type = @byte.GetBytePart(TypeMask, 2);
        bits.RightShift(6);

        if (Type == 4) {
            Payload = new LiteralPayload(bits);
        } else {
            Payload = new OperatorPayload(bits);
        }
    }
}

internal abstract class Payload {
    public abstract int Length { get; }
    public virtual int SummedVersion => 0;
}

internal sealed class OperatorPayload : Payload {
    private readonly bool _lenghtType;
    private readonly Packet[] _packets;

    public override int SummedVersion => _packets.Sum(x => x.SummedVersion);

    public OperatorPayload(BitArray bits) {
        _lenghtType = bits[0];
        var bytes = bits.ToReversedByteArray();
        if (!_lenghtType) {
            bits.RightShift(16);
            var totalBits = (short)((bytes[0] << 8) | bytes[1]);
            var result = new List<Packet>();
            while (result.Sum(x => x.Length) < totalBits) {
                result.Add(new Packet(bits));
            }

            _packets = result.ToArray();
        } else {
            bits.RightShift(12);
            var secondBits = bytes[..2].ToReversedBitArray();
            secondBits.LeftShift(4);
            var bytes2 = secondBits.ToReversedByteArray();
            var totalBits = (short)(((bytes2[0] & 7) << 7) | bytes2[1]);

            var result = new List<Packet>();
            for (var i = 0; i < totalBits; i++) {
                result.Add(new Packet(bits));
            }

            _packets = result.ToArray();
        }
    }

    public override int Length => 1 + _packets.Sum(x => x.Length);
}

internal sealed class LiteralPayload : Payload {
    private const int Mask = 15 << 3;
    private const int FirstBitMask = 1 << 7;

    public int Value { get; }

    public LiteralPayload(BitArray bits) {
        var firstBit = false;
        var results = new List<int>();
        do {
            var bytes = bits.ToReversedByteArray();
            firstBit = (bytes[0] & FirstBitMask) == 128;
            var number = (bytes[0] & Mask) >> 3;
            results.Add(number);
            bits.RightShift(5);
        } while (firstBit);


        for (var i = 0; i < results.Count; i++) {
            var number = results[i];
            var part = number << ((results.Count - i - 1) * 4);
            Value = part | Value;
        }

        var resultLength = results.Count * 5;
        //Length = resultLength + (4 - (resultLength + 6) % 4); // padding
        Length = resultLength;
    }

    private byte[] Parts { get; }
    public override int Length { get; }
}
