using AdventOfCode.Year_2021;

namespace Tests.Year_2021;

public sealed class Day16Tests {
    [Theory]
    [InlineData("8A004A801A8002F478", 16)]
    [InlineData("620080001611562C8802118E34", 12)]
    [InlineData("C0015000016115A2E0802F182340", 23)]
    [InlineData("A0016C880162017C3686B18A3D4780", 31)]
    public void Packet_ShouldHaveCorrectSummedVersion(string input, int expected) {
        var day = new Day16(input);
        day.Packet.SummedVersion.Should().Be(expected);
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
    public void Packet_ShouldHaveCorrectValue(string input, ulong expected) {
        var day = new Day16(input);
        day.Packet.Value.Should().Be(expected);
    }
}
