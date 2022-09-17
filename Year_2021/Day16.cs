using System.Collections;
using AdventOfCode.Common.Helpers;

namespace AdventOfCode.Year_2021;

/// <summary>
///     Day 16 from year 2021
/// </summary>
internal sealed class Day16 : BaseDay {
    private byte[] _bytes;
    private readonly string _hex;
    private BitArray _bitArray;
    public Packet Packet;

    public Day16() {
        _hex = File.ReadAllText(InputFilePath);
        Init();
    }

    public Day16(string hex) {
        _hex = hex;
        Init();
    }

    private void Init() {
        _bytes = ConvertToBytes(_hex);
        _bitArray = _bytes.ToReversedBitArray();
        Packet = new Packet(_bitArray);
    }

    private byte[] ConvertToBytes(string hex) => Convert.FromHexString(hex);


    public override ValueTask<string> Solve_1() {
        var result = Packet.SummedVersion;
        return new ValueTask<string>($"Result: `{result}`");
    }

    public override ValueTask<string> Solve_2() {
        var result = Packet.Value;
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

    public ulong Value => Payload.Value;
    
    public byte Type { get; }
    private Payload Payload { get; }

    public Packet(BitArray bits) {
        var bytes = new byte[bits.Length / 8];
        bits.CopyTo(bytes, 0);
        var @byte = bytes[0].ReverseBitsWith4Operations();

        Version = @byte.GetBytePart(VersionMask, 5);
        Type = @byte.GetBytePart(TypeMask, 2);
        bits.RightShift(6);

        Payload = Type switch {
            0 => new SumOperatorPayload(bits),
            1 => new ProductOperatorPayload(bits),
            2 => new MinOperatorPayload(bits),
            3 => new MaxOperatorPayload(bits),
            4 => new LiteralPayload(bits),
            5 => new GreaterOperatorPayload(bits),
            6 => new LessOperatorPayload(bits),
            7 => new EqualOperatorPayload(bits),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

internal abstract class Payload {
    public abstract int Length { get; }
    public virtual int SummedVersion => 0;
    public abstract ulong Value { get; }
}

internal sealed class SumOperatorPayload : OperatorPayload {
    public SumOperatorPayload(BitArray bits) : base(bits) { }

    public override ulong Value {
        get {
            ulong num = 0;
            foreach (var packet in _packets) {
                num += packet.Value;
            }

            return num;
        }
    }
}

internal sealed class ProductOperatorPayload : OperatorPayload {
    public ProductOperatorPayload(BitArray bits) : base(bits) { }

    public override ulong Value {
        get {
            ulong num = 1;
            foreach (var packet in _packets) {
                num *= packet.Value;
            }

            return num;
        }
    }
}

internal sealed class MinOperatorPayload : OperatorPayload {
    public MinOperatorPayload(BitArray bits) : base(bits) { }
    public override ulong Value => _packets.Min(x => x.Value);
}

internal sealed class MaxOperatorPayload : OperatorPayload {
    public MaxOperatorPayload(BitArray bits) : base(bits) { }
    public override ulong Value => _packets.Max(x => x.Value);
}

internal sealed class GreaterOperatorPayload : OperatorPayload {
    public GreaterOperatorPayload(BitArray bits) : base(bits) { }
    public override ulong Value => _packets[0].Value > _packets[1].Value ? (ulong)1 : 0;
}

internal sealed class LessOperatorPayload : OperatorPayload {
    public LessOperatorPayload(BitArray bits) : base(bits) { }
    public override ulong Value => _packets[0].Value < _packets[1].Value ? (ulong)1 : 0;
}

internal sealed class EqualOperatorPayload : OperatorPayload {
    public EqualOperatorPayload(BitArray bits) : base(bits) { }
    public override ulong Value => _packets[0].Value == _packets[1].Value ? (ulong)1 : 0;
}

internal abstract class OperatorPayload : Payload {
    private readonly bool _lenghtType;
    protected readonly Packet[] _packets;

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
            // 00000110101
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
            var number = (ulong)results[i];
            var shift = (results.Count - i - 1) * 4;
            var part = number << shift;
            Value = part | Value;
        }

        var resultLength = results.Count * 5;
        //Length = resultLength + (4 - (resultLength + 6) % 4); // padding
        Length = resultLength;
    }
    public override int Length { get; }
    public override ulong Value { get; }
}
