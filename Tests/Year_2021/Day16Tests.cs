using System;
using System.Collections;
using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public sealed class Day16Tests {
    [Theory]
    [InlineData("8A004A801A8002F478", 16)]
    [InlineData("620080001611562C8802118E34", 12)]
    [InlineData("C0015000016115A2E0802F182340", 23)]
    [InlineData("A0016C880162017C3686B18A3D4780", 31)]
    public void Packet_ShouldHaveCorrectSummedVersion(string input, int expected) {
        var packet = Day16.ReadPacket(new Day16.BitReader(input));
        var result = Day16.SumVersions(packet);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("C200B40A82", 3)]
    [InlineData("04005AC33890", 54)]
    [InlineData("880086C3E88112", 7)]
    [InlineData("CE00C43D881120", 9)]
    [InlineData("D8005AC2A8F0", 1)]
    [InlineData("F600BC2D8F", 0)]
    [InlineData("9C005AC2F8F0", 0)]
    [InlineData("9C0141080250320F1802104A08", 1)]
    public void Packet_ShouldHaveCorrectValue(string input, long expected) {
        var packet = Day16.ReadPacket(new Day16.BitReader(input));
        var result = Day16.CalculatePayload(packet);
        result.Should().Be(expected);
    }

    [Fact]
    public void BitReader_ShouldHaveCorrectBitArray_WhenReceivingLiteralHex() {
        var hex = "D2FE28";
        var expected = new BitArray(new[] {
            true, true, false, true, false, false, true, false, true, true, true, true, true, true, 
            true, false, false, false, true, false, true, false, false, false
        });

        var reader = new Day16.BitReader(hex);

        reader.Bits.Should().BeEquivalentTo(expected);

    }
    
    [Fact]
    public void BitReader_ShouldHaveCorrectBitArray_WhenReceivingOperatorHex() {
        var hex = "38006F45291200";
        var expected = new BitArray(new[] {
            false, false, true, true, true, false, false, false, false, false, false, false, false, false, 
            false, false, false, true, true, false, true, true, true, true, false, true, false, false, false, 
            true, false, true, false, false, true, false, true, false, false, true, false, false, false, true, 
            false, false, true, false, false, false, false, false, false, false, false, false
        });

        var reader = new Day16.BitReader(hex);

        reader.Bits.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void BitReader_ShouldReadCorrectInt_WhenOnlyReadingItOnce() {
        var hex = "D2FE28";
        var reader = new Day16.BitReader(hex);

        var result = reader.ReadInteger(3);

        result.Should().Be(6);
    }
    
    [Fact]
    public void BitReader_ShouldReadCorrectInt_WhenReadingTwice() {
        var hex = "D2FE28";
        var reader = new Day16.BitReader(hex);
            
        _ = reader.ReadInteger(3);
        var result = reader.ReadInteger(3);

        result.Should().Be(4);
    }
    
    [Fact]
    public void ReadPacket_ShouldReadPacket_WhenItsLiteralPacket() {
        var hex = "D2FE28";
        var expected = new Day16.Packet(4, 6, 2021, Array.Empty<Day16.Packet>());

        var result = Day16.ReadPacket(new Day16.BitReader(hex));

        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadPacket_ShouldReadPacket_WhenItsOperatorPacketWithLengthType0() {
        var hex = "38006F45291200";
        var expected = new Day16.Packet(6, 1, 0L, new [] {
            new Day16.Packet(4, 6, 10L, Array.Empty<Day16.Packet>()),
            new Day16.Packet(4, 2, 20L, Array.Empty<Day16.Packet>())
        });

        var result = Day16.ReadPacket(new Day16.BitReader(hex));

        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadPacket_ShouldReadPacket_WhenItsOperatorPacketWithLengthType1() {
        var hex = "EE00D40C823060";
        var expected = new Day16.Packet(3, 7, 0L, new [] {
            new Day16.Packet(4, 2, 1L, Array.Empty<Day16.Packet>()),
            new Day16.Packet(4, 4, 2L, Array.Empty<Day16.Packet>()),
            new Day16.Packet(4, 1, 3L, Array.Empty<Day16.Packet>())
        });

        var result = Day16.ReadPacket(new Day16.BitReader(hex));

        result.Should().BeEquivalentTo(expected);
    }
}
