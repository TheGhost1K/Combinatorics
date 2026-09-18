using System.Numerics;
using Xunit;
using Combinatorics;

namespace Combinatorics.Tests;

public class BigRationalOperatorsTests
{
    // ==================== МЕНЬШЕ ====================

    [Fact]
    public void LessThan_Works()
    {
        var oneThird = new BigRational(1, 3);
        var oneHalf = new BigRational(1, 2);

        Assert.True(oneThird < oneHalf);
        Assert.False(oneHalf < oneThird);
    }

    [Fact]
    public void LessThan_SameValue_IsFalse()
    {
        var a = new BigRational(1, 2);
        var b = new BigRational(2, 4);   // нормализуется к 1/2
        Assert.False(a < b);
    }

    [Fact]
    public void LessThan_WithNegative()
    {
        var minusHalf = new BigRational(-1, 2);
        var half = new BigRational(1, 2);
        var minusThird = new BigRational(-1, 3);

        Assert.True(minusHalf < half);
        Assert.True(minusHalf < minusThird);
        Assert.True(minusHalf < BigRational.Zero);
    }

    // ==================== БОЛЬШЕ ====================

    [Fact]
    public void GreaterThan_Works()
    {
        var oneHalf = new BigRational(1, 2);
        var oneThird = new BigRational(1, 3);

        Assert.True(oneHalf > oneThird);
        Assert.False(oneThird > oneHalf);
    }

    [Fact]
    public void GreaterThan_SameValue_IsFalse()
    {
        var a = new BigRational(1, 2);
        var b = new BigRational(2, 4);
        Assert.False(a > b);
    }

    // ==================== МЕНЬШЕ ИЛИ РАВНО ====================

    [Fact]
    public void LessOrEqual_Works()
    {
        var oneThird = new BigRational(1, 3);
        var oneHalf = new BigRational(1, 2);
        var alsoOneHalf = new BigRational(2, 4);

        Assert.True(oneThird <= oneHalf);
        Assert.True(oneHalf <= alsoOneHalf);
        Assert.False(oneHalf <= oneThird);
    }

    // ==================== БОЛЬШЕ ИЛИ РАВНО ====================

    [Fact]
    public void GreaterOrEqual_Works()
    {
        var oneHalf = new BigRational(1, 2);
        var oneThird = new BigRational(1, 3);
        var alsoOneHalf = new BigRational(2, 4);

        Assert.True(oneHalf >= oneThird);
        Assert.True(oneHalf >= alsoOneHalf);
        Assert.False(oneThird >= oneHalf);
    }

    // ==================== С ПРИМИТИВАМИ ====================

    [Fact]
    public void Compare_WithLong()
    {
        var half = new BigRational(1, 2);

        Assert.True(half < 1L);
        Assert.True(half > 0L);
        Assert.True(half <= 1L);
        Assert.True(half >= 0L);
        Assert.True(1L > half);
        Assert.True(0L < half);
    }

    [Fact]
    public void Compare_WithBigInteger()
    {
        var half = new BigRational(1, 2);
        BigInteger one = 1;
        BigInteger zero = 0;

        Assert.True(half < one);
        Assert.True(half > zero);
        Assert.True(half <= one);
        Assert.True(half >= zero);
        Assert.True(one > half);
        Assert.True(zero < half);
    }

    [Fact]
    public void Compare_WithLong_Large()
    {
        var big = new BigRational(BigInteger.Pow(10, 30), 1);
        Assert.True(big > 1L);
        Assert.True(1L < big);
    }

    // ==================== СОГЛАСОВАННОСТЬ С CompareTo ====================

    [Theory]
    [InlineData(1, 2, 1, 3)]
    [InlineData(2, 4, 1, 2)]
    [InlineData(-1, 2, -1, 3)]
    [InlineData(0, 1, 0, 5)]
    [InlineData(5, 1, 4, 1)]
    [InlineData(1, 100, 1, 101)]
    public void Operators_MatchCompareTo(int an, int ad, int bn, int bd)
    {
        var a = new BigRational(an, ad);
        var b = new BigRational(bn, bd);
        int cmp = a.CompareTo(b);

        Assert.Equal(cmp < 0, a < b);
        Assert.Equal(cmp > 0, a > b);
        Assert.Equal(cmp <= 0, a <= b);
        Assert.Equal(cmp >= 0, a >= b);
        Assert.Equal(cmp == 0, a == b);
        Assert.Equal(cmp != 0, a != b);
    }

    // ==================== ТРАНЗИТИВНОСТЬ ====================

    [Fact]
    public void Ordering_Transitive()
    {
        var a = new BigRational(1, 4);
        var b = new BigRational(1, 3);
        var c = new BigRational(1, 2);

        Assert.True(a < b);
        Assert.True(b < c);
        Assert.True(a < c);
    }

    // ==================== СРАВНЕНИЕ С НУЛЁМ ====================

    [Fact]
    public void Compare_WithZero()
    {
        var half = new BigRational(1, 2);
        var minusHalf = new BigRational(-1, 2);

        Assert.True(half > BigRational.Zero);
        Assert.True(minusHalf < BigRational.Zero);
    }

    [Fact]
    public void Compare_ZeroWithItself_ReturnsEqual()
    {
        var zero1 = BigRational.Zero;
        var zero2 = BigRational.Zero;

        Assert.True(zero1 <= zero2);
        Assert.True(zero1 >= zero2);
        Assert.True(zero1 == zero2);
        Assert.False(zero1 < zero2);
        Assert.False(zero1 > zero2);
    }

    // ==================== СРАВНЕНИЕ С ЕДИНИЦЕЙ ====================

    [Fact]
    public void Compare_WithOne()
    {
        var half = new BigRational(1, 2);
        var threeHalves = new BigRational(3, 2);

        Assert.True(half < BigRational.One);
        Assert.True(threeHalves > BigRational.One);
    }

    [Fact]
    public void Compare_OneWithItself_ReturnsEqual()
    {
        var one1 = BigRational.One;
        var one2 = BigRational.One;

        Assert.True(one1 >= one2);
        Assert.True(one1 <= one2);
        Assert.True(one1 == one2);
    }

    // ==================== IComparable ====================

    [Fact]
    public void IComparable_NonGeneric_Works()
    {
        object a = new BigRational(1, 3);
        object b = new BigRational(1, 2);

        Assert.True(((IComparable)a).CompareTo(b) < 0);
        Assert.True(((IComparable)b).CompareTo(a) > 0);
    }

    [Fact]
    public void IComparable_Null_ReturnsPositive()
    {
        object a = new BigRational(1, 2);
        Assert.True(((IComparable)a).CompareTo(null) > 0);
    }

    [Fact]
    public void IComparable_WrongType_Throws()
    {
        object a = new BigRational(1, 2);
        Assert.Throws<ArgumentException>(() => ((IComparable)a).CompareTo("not a rational"));
    }

    // ==================== СОРТИРОВКА ====================

    [Fact]
    public void Sorting_Works()
    {
        var list = new List<BigRational>
        {
            new(1, 2), new(1, 4), new(3, 4), new(1, 3), new(2, 3)
        };
        list.Sort();

        var expected = new[]
        {
            new BigRational(1, 4),
            new BigRational(1, 3),
            new BigRational(1, 2),
            new BigRational(2, 3),
            new BigRational(3, 4)
        };
        Assert.Equal(expected, list);
    }

    [Fact]
    public void MinMax_Work()
    {
        var a = new BigRational(1, 3);
        var b = new BigRational(1, 2);

        Assert.Equal(a, BigRationalMin(a, b));
        Assert.Equal(b, BigRationalMax(a, b));
    }

    // ==================== ToDecimalString ====================

    [Fact]
    public void ToDecimalString_Works()
    {
        Assert.Equal("1.500000", new BigRational(3, 2).ToDecimalString());
        Assert.Equal("0.333333", new BigRational(1, 3).ToDecimalString());
        Assert.Equal("5", new BigRational(5, 1).ToDecimalString());
        Assert.Equal("-0.500000", new BigRational(-1, 2).ToDecimalString());
    }

    // ==================== ХЕШ-КОД ====================

    [Fact]
    public void GetHashCode_ConsistentWithEquality()
    {
        var a = new BigRational(1, 2);
        var b = new BigRational(2, 4);

        Assert.Equal(a, b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    // ==================== ВСПОМОГАТЕЛЬНЫЕ ====================

    private static BigRational BigRationalMin(BigRational a, BigRational b)
        => a < b ? a : b;

    private static BigRational BigRationalMax(BigRational a, BigRational b)
        => a > b ? a : b;
}