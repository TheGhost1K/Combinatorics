using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class BinetTests
{
    [Fact]
    public void FibonacciBinet_MatchesIterative()
    {
        for (int n = 0; n <= 100; n++)
            Assert.Equal(Fibonacci(n), FibonacciBinet(n));
    }

    [Fact]
    public void LucasBinet_MatchesIterative()
    {
        for (int n = 0; n <= 100; n++)
            Assert.Equal(Lucas(n), LucasBinet(n));
    }

    [Fact]
    public void PellBinet_MatchesIterative()
    {
        for (int n = 0; n <= 50; n++)
            Assert.Equal(Pell(n), PellBinet(n));
    }

    [Fact]
    public void PellLucasBinet_MatchesIterative()
    {
        for (int n = 0; n <= 50; n++)
            Assert.Equal(PellLucas(n), PellLucasBinet(n));
    }

    [Fact]
    public void Phi_Properties()
    {
        // φ² = φ + 1
        var phi = Phi;
        var phiSq = QuadraticSurd.Pow(phi, 2);
        var phiPlus1 = phi + QuadraticSurd.FromInteger(1, 5);
        Assert.Equal(phiPlus1, phiSq);
    }

    [Fact]
    public void PhiPsi_Product()
    {
        // φ·ψ = −1
        var product = Phi * Psi;
        Assert.True(product.IsRational);
        Assert.Equal(new BigInteger(-1), product.ToRational().Num);
    }

    [Fact]
    public void Sqrt2Properties()
    {
        // (1 + √2)(1 − √2) = −1
        var product = OnePlusSqrt2 * OneMinusSqrt2;
        Assert.True(product.IsRational);
        Assert.Equal(new BigInteger(-1), product.ToRational().Num);
    }

    [Fact]
    public void Sqrt2_PowLarge()
    {
        // (1 + √2)¹⁰ = 3363 + 2378√2
        var p = QuadraticSurd.Pow(OnePlusSqrt2, 10);
        Assert.Equal(new BigInteger(3363), p.A);
        Assert.Equal(new BigInteger(2378), p.B);
    }
}