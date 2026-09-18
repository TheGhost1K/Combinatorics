using System.Numerics;
using Xunit;
using Combinatorics;

namespace Combinatorics.Tests;

public class QuadraticSurdTests
{
    // ==================== КОНСТРУКТОРЫ ====================

    [Fact]
    public void FromInteger_Works()
    {
        var q = QuadraticSurd.FromInteger(5, 5);
        Assert.Equal(new BigInteger(5), q.A);
        Assert.Equal(BigInteger.Zero, q.B);
        Assert.Equal(BigInteger.One, q.C);
        Assert.Equal(new BigInteger(5), q.D);
        Assert.True(q.IsRational);
    }

    [Fact]
    public void Sqrt_Works()
    {
        var q = QuadraticSurd.Sqrt(5);
        Assert.Equal(BigInteger.Zero, q.A);
        Assert.Equal(BigInteger.One, q.B);
        Assert.Equal(BigInteger.One, q.C);
        Assert.False(q.IsRational);
    }

    [Fact]
    public void Constructor_NormalizesGcd()
    {
        var q = new QuadraticSurd(2, 2, 4, 5);
        Assert.Equal(BigInteger.One, q.A);
        Assert.Equal(BigInteger.One, q.B);
        Assert.Equal(new BigInteger(2), q.C);
    }

    [Fact]
    public void Constructor_NormalizesSign()
    {
        var q = new QuadraticSurd(1, 1, -2, 5);
        Assert.Equal(new BigInteger(-1), q.A);
        Assert.Equal(new BigInteger(-1), q.B);
        Assert.Equal(new BigInteger(2), q.C);
    }

    // ==================== АРИФМЕТИКА ====================

    [Fact]
    public void Addition_Works()
    {
        var a = new QuadraticSurd(1, 1, 1, 5);  // 1 + √5
        var b = new QuadraticSurd(2, 3, 1, 5);  // 2 + 3√5
        var sum = a + b;                        // 3 + 4√5
        Assert.Equal(new BigInteger(3), sum.A);
        Assert.Equal(new BigInteger(4), sum.B);
        Assert.Equal(BigInteger.One, sum.C);
    }

    [Fact]
    public void Subtraction_Works()
    {
        var a = new QuadraticSurd(5, 5, 1, 5);
        var b = new QuadraticSurd(2, 2, 1, 5);
        var diff = a - b; // 3 + 3√5
        Assert.Equal(new BigInteger(3), diff.A);
        Assert.Equal(new BigInteger(3), diff.B);
    }

    [Fact]
    public void Multiplication_Works()
    {
        // (1 + √2)(1 − √2) = 1 − 2 = −1
        var a = new QuadraticSurd(1, 1, 1, 2);
        var b = new QuadraticSurd(1, -1, 1, 2);
        var prod = a * b;
        Assert.Equal(new BigInteger(-1), prod.A);
        Assert.Equal(BigInteger.Zero, prod.B);
    }

    [Fact]
    public void Division_Works()
    {
        var a = new QuadraticSurd(1, 1, 1, 5);   // 1 + √5
        var b = new QuadraticSurd(1, 1, 1, 5);   // 1 + √5
        var quot = a / b;                        // = 1
        Assert.True(quot.IsRational);
        var r = quot.ToRational();
        Assert.Equal(BigInteger.One, r.Num);
        Assert.Equal(BigInteger.One, r.Den);
    }

    [Fact]
    public void Conjugate_Works()
    {
        var a = new QuadraticSurd(3, 4, 5, 2);
        var c = a.Conjugate;
        Assert.Equal(new BigInteger(3), c.A);
        Assert.Equal(new BigInteger(-4), c.B);
    }

    [Fact]
    public void Norm_Works()
    {
        // N(1 + √2) = 1 − 2 = −1
        var a = new QuadraticSurd(1, 1, 1, 2);
        Assert.Equal(new BigRational(-1, 1), a.Norm);
    }

    // ==================== СТЕПЕНИ ====================

    [Fact]
    public void Pow_Zero()
    {
        var a = new QuadraticSurd(1, 1, 1, 5);
        var p = QuadraticSurd.Pow(a, 0);
        Assert.True(p.IsRational);
        Assert.Equal(BigInteger.One, p.ToRational().Num);
    }

    [Fact]
    public void Pow_One()
    {
        var a = new QuadraticSurd(3, 4, 5, 2);
        var p = QuadraticSurd.Pow(a, 1);
        Assert.Equal(a, p);
    }

    [Fact]
    public void Pow_Two()
    {
        // (1 + √2)² = 3 + 2√2
        var a = new QuadraticSurd(1, 1, 1, 2);
        var sq = QuadraticSurd.Pow(a, 2);
        Assert.Equal(new BigInteger(3), sq.A);
        Assert.Equal(new BigInteger(2), sq.B);
        Assert.Equal(BigInteger.One, sq.C);
    }

    [Fact]
    public void Pow_Negative()
    {
        // (1 + √2)⁻¹ = −1 + √2 (поскольку норма = −1)
        var a = new QuadraticSurd(1, 1, 1, 2);
        var inv = QuadraticSurd.Pow(a, -1);
        Assert.Equal(new BigInteger(-1), inv.A);
        Assert.Equal(BigInteger.One, inv.B);
    }

    [Fact]
    public void Pow_BigExponent()
    {
        // (1 + √2)¹⁰ = 3363 + 2378√2 (известное значение)
        var a = new QuadraticSurd(1, 1, 1, 2);
        var p = QuadraticSurd.Pow(a, 10);
        Assert.Equal(new BigInteger(3363), p.A);
        Assert.Equal(new BigInteger(2378), p.B);
    }

    // ==================== ПРОВЕРКА D ====================

    [Fact]
    public void DifferentD_Throws()
    {
        var a = QuadraticSurd.Sqrt(5);
        var b = QuadraticSurd.Sqrt(7);
        Assert.Throws<ArgumentException>(() => a + b);
        Assert.Throws<ArgumentException>(() => a * b);
        Assert.Throws<ArgumentException>(() => a - b);
        Assert.Throws<ArgumentException>(() => a / b);
    }

    // ==================== РАВЕНСТВО ====================

    [Fact]
    public void Equality_Works()
    {
        var a = new QuadraticSurd(1, 2, 3, 5);
        var b = new QuadraticSurd(2, 4, 6, 5); // нормализуется к тому же
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    // ==================== ВЫВОД ====================

    [Fact]
    public void ToString_Works()
    {
        Assert.Equal("5", QuadraticSurd.FromInteger(5, 5).ToString());
        Assert.Equal("√5", QuadraticSurd.Sqrt(5).ToString());
        Assert.Equal("(1 + √5)/2", new QuadraticSurd(1, 1, 2, 5).ToString());
        Assert.Equal("(1 - √5)/2", new QuadraticSurd(1, -1, 2, 5).ToString());
    }
}