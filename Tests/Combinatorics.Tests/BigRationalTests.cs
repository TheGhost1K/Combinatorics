using System.Numerics;
using Xunit;

namespace Combinatorics.Tests;

public class BigRationalTests
{
    [Fact]
    public void Constructor_ReducesFraction()
    {
        var r = new BigRational(4, 8);
        Assert.Equal(new BigInteger(1), r.Num);
        Assert.Equal(new BigInteger(2), r.Den);
    }

    [Fact]
    public void Constructor_NormalizesSign()
    {
        var r = new BigRational(1, -2);
        Assert.Equal(new BigInteger(-1), r.Num);
        Assert.Equal(new BigInteger(2), r.Den);
    }

    [Fact]
    public void ZeroDenominator_Throws()
        => Assert.Throws<DivideByZeroException>(() => new BigRational(1, 0));

    [Fact]
    public void Addition_Works()
    {
        var a = new BigRational(1, 3);
        var b = new BigRational(1, 6);
        Assert.Equal(new BigRational(1, 2), a + b);
    }

    [Fact]
    public void Subtraction_Works()
    {
        var a = new BigRational(1, 2);
        var b = new BigRational(1, 3);
        Assert.Equal(new BigRational(1, 6), a - b);
    }

    [Fact]
    public void Multiplication_Works()
    {
        var a = new BigRational(2, 3);
        var b = new BigRational(3, 4);
        Assert.Equal(new BigRational(1, 2), a * b);
    }

    [Fact]
    public void Division_Works()
    {
        var a = new BigRational(1, 2);
        var b = new BigRational(1, 4);
        Assert.Equal(new BigRational(2, 1), a / b);
    }

    [Fact]
    public void Equality_Works()
    {
        Assert.Equal(new BigRational(1, 2), new BigRational(2, 4));
        Assert.True(new BigRational(1, 2) == new BigRational(2, 4));
        Assert.True(new BigRational(1, 2) != new BigRational(1, 3));
    }

    [Fact]
    public void Comparison_Works()
    {
        Assert.True(new BigRational(1, 3) < new BigRational(1, 2));
        Assert.True(new BigRational(1, 2) > new BigRational(1, 3));
    }

    [Fact]
    public void ToString_Formats()
    {
        Assert.Equal("3", new BigRational(3, 1).ToString());
        Assert.Equal("1/2", new BigRational(1, 2).ToString());
    }
}