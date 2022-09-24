using System;
using System.Numerics;
using AdventOfCode.Year_2021;
using Vector = MathNet.Numerics.LinearAlgebra.Single.Vector;

namespace Tests.Year_2021; 

public sealed class Day19Tests {
    Func<string, Day19.Scanner[]> ReadScanners = Day19.Reader.ReadScanners;

    [Fact]
    public void Reader_ShouldHaveTwoScanners_WhenGivenMultipleScanners() {
        //Arrange
        var input = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600";

        //Act
        var result = ReadScanners(input);

        //Assert
        result.Should().HaveCount(2);
        result[0].Index.Should().Be(0);
        result[1].Index.Should().Be(1);
    }
    
    [Fact]
    public void Reader_ShouldHaveCorrectAmountOfProbes_WhenGivenMultipleScanners() {
        //Arrange
        var input = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600";

        //Act
        var result = ReadScanners(input);

        //Assert
        result[0].Beacons.Should().HaveCount(8);
        result[1].Beacons.Should().HaveCount(10);
    }
    
    [Fact]
    public void Reader_ShouldHaveCorrectBeaconVector_WhenGivenMultipleScanners() {
        //Arrange
        var input = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,452
669,-402,600";

        //Act
        var result = ReadScanners(input);

        //Assert
        result[0].Beacons[0].Location.Should().BeEquivalentTo(new Day19.Vector(404, -588, -901));
        result[1].Beacons[8].Location.Should().BeEquivalentTo(new Day19.Vector(-460, 603, 452));
    }
}
