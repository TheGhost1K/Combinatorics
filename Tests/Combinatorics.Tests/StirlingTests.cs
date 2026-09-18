using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class StirlingTests
{
    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(4, 1, 6)]
    [InlineData(4, 2, 11)]
    [InlineData(4, 3, 6)]
    [InlineData(4, 4, 1)]
    [InlineData(5, 2, 50)]
    [InlineData(5, 3, 35)]
    public void StirlingFirstKind_Works(int n, int k, long expected)
        => Assert.Equal(new BigInteger(expected), StirlingFirstKind(n, k));

    [Fact]
    public void StirlingFirstKind_RowSumEqualsFactorial()
    {
        for (int n = 0; n <= 20; n++)
        {
            BigInteger sum = 0;
            for (int k = 0; k <= n; k++) sum += StirlingFirstKind(n, k);
            Assert.Equal(Factorial(n), sum);
        }
    }

    [Fact]
    public void SignedStirlingFirstKind_AlternatesSign()
    {
        Assert.Equal(-50, (long)SignedStirlingFirstKind(5, 2));
        Assert.Equal(35, (long)SignedStirlingFirstKind(5, 3));
        Assert.Equal(-10, (long)SignedStirlingFirstKind(5, 4));
    }

    [Fact]
    public void SignedStirlingFirstKind_FallingFactorialIdentity()
    {
        // x(x-1)(x-2) = x^3 - 3x^2 + 2x = sum_k s(3,k) x^k
        // s(3,3)=1, s(3,2)=-3, s(3,1)=2
        Assert.Equal(1, (long)SignedStirlingFirstKind(3, 3));
        Assert.Equal(-3, (long)SignedStirlingFirstKind(3, 2));
        Assert.Equal(2, (long)SignedStirlingFirstKind(3, 1));
    }

    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(3, 1, 1)]
    [InlineData(3, 2, 3)]
    [InlineData(3, 3, 1)]
    [InlineData(5, 2, 15)]
    [InlineData(5, 3, 25)]
    [InlineData(10, 5, 42525)]
    public void StirlingSecondKind_Works(int n, int k, long expected)
        => Assert.Equal(new BigInteger(expected), StirlingSecondKind(n, k));

    [Fact]
    public void StirlingSecondKind_RowSumEqualsBell()
    {
        for (int n = 0; n <= 15; n++)
        {
            BigInteger sum = 0;
            for (int k = 0; k <= n; k++) sum += StirlingSecondKind(n, k);
            Assert.Equal(Bell(n), sum);
        }
    }

    [Fact]
    public void StirlingSecondKind_EdgeCases()
    {
        Assert.Equal(BigInteger.One, StirlingSecondKind(0, 0));
        Assert.Equal(BigInteger.Zero, StirlingSecondKind(0, 1));
        Assert.Equal(BigInteger.Zero, StirlingSecondKind(5, 0));
        Assert.Equal(BigInteger.Zero, StirlingSecondKind(3, 5));
        Assert.Equal(BigInteger.One, StirlingSecondKind(7, 7));
    }
}