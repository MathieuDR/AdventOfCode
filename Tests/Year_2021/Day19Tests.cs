using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using AdventOfCode.Year_2021;
using Xunit.Abstractions;
using Vector = MathNet.Numerics.LinearAlgebra.Single.Vector;

namespace Tests.Year_2021; 

public sealed class Day19Tests {
    private readonly ITestOutputHelper _helper;
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
    
    public float ConvertToRadians(float angle)
    {
        return (float)((Math.PI / 180) * angle);
    }

    public Day19Tests(ITestOutputHelper helper) {
        _helper = helper;
    }

    [Fact]
    public void Test() {
        //Arrange
        var x = 10;
        var y = 50;
        var z = 100;
        var v1 = new Vector3(x, y, z);
        
        //Act
        var turn = 1.5708f;

        var result = new List<(int xRotation, int yRotation, int zRotation, string coords)>();
        var xQuat = Quaternion.CreateFromYawPitchRoll(0, 0, turn);
        var yQuat = Quaternion.CreateFromYawPitchRoll(0, turn, 0);
        var zQuat = Quaternion.CreateFromYawPitchRoll(turn, 0, 0);

        var rotated = v1;
        for (int i = 0; i < 4; i++) {
            if(i>0)
                rotated = Vector3.Transform(rotated, xQuat);
            for (int j = 0; j < 4; j++) {
                if(j>0)
                    rotated = Vector3.Transform(rotated, yQuat);
                for (int k = 0; k < 4; k++) {
                    if(k>0)
                        rotated = Vector3.Transform(rotated, zQuat);
                    
                    result.Add((i, j, k, $"{FindAxis(v1, rotated, 0)}, {FindAxis(v1, rotated, 1)}, {FindAxis(v1, rotated, 2)}"));
                }
            }
        }
        
        _helper.WriteLine("Total rotations: " + result.Count);
        var distinct = result.DistinctBy(t => t.coords).ToList();
        _helper.WriteLine("distinct rotations: " + distinct.Count);
        foreach (var (xRotation, yRotation, zRotation, coords) in distinct) {
            _helper.WriteLine($"Rotation {xRotation}.{yRotation}.{zRotation}: {coords}");
        }

    }

    private string FindAxis(Vector3 original, Vector3 rotated, int axis) {
        var tolerance = 0.05f;
        var value = axis switch {
            0 => rotated.X,
            1 => rotated.Y,
            2 => rotated.Z,
            _ => throw new ArgumentOutOfRangeException()
        };

        var sign = value switch {
            < 0 => "-",
            _ => ""
        };

        value = Math.Abs(value);

        if (value - tolerance <= original.X && value + tolerance >= original.X) {
            return $"{sign}x";
        }
        
        if (value - tolerance <= original.Y && value + tolerance >= original.Y) {
            return $"{sign}y";
        }
        
        if (value - tolerance <= original.Z && value + tolerance >= original.Z) {
            return $"{sign}z";
        }

        throw new ArgumentOutOfRangeException();
    }
}
