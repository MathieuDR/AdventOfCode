using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common.Helpers;

namespace Tests.HelperTests;

public class CollectionTests {
    [Fact]
    public void TransposeOfBytesShouldReturnAResult() {
        var inp = new byte[] { 1, 4, 8, 16, 32, 64, 128, 255 };
        var transposed = inp.Transpose();
        transposed.Should().NotBeNull();
        transposed.Should().NotBeEmpty();
    }

    [Fact]
    public void TransposeOfBytesShouldHaveCorrectNumberOfItems() {
        var inp = new byte[] { 1, 4, 8, 16, 32, 64 };
        var transposed = inp.Transpose();
        transposed.Count().Should().Be(8);
    }

    [Fact]
    public void TransposeOfBytesShouldHaveCorrectNumberOfItemsInTransposedResult() {
        var inp = new byte[] { 1, 4, 8, 16, 32, 64 };
        var transposed = inp.Transpose();
        transposed.First().Count().Should().Be(6);
    }

    [Fact]
    public void TransposeOfBytesShouldHaveCorrectTransposedResult() {
        var inp = new byte[] { 32, 64 };
        var transposed = inp.Transpose().Select(x => x.ToArray()).ToArray();

        var bytes = new List<byte>();
        foreach (var boolArr in transposed) {
            var bitArr = new BitArray(boolArr);
            var b = new byte[1];
            bitArr.CopyTo(b, 0);
            bytes.Add(b[0]);
        }

        bytes[0].Should().Be(0);
        bytes[1].Should().Be(0);
        bytes[2].Should().Be(0);
        bytes[3].Should().Be(0);
        bytes[4].Should().Be(0);
        bytes[5].Should().Be(2);
        bytes[6].Should().Be(1);
        bytes[7].Should().Be(0);
    }
}
