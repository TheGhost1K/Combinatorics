using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class LahTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(2, 2, 1)]
    [InlineData(3, 1, 6)]
    [InlineData(3, 2, 6)]
    [InlineData(3, 3, 1)]
    [InlineData(4, 2, 36)]
    [InlineData(5, 3, 120)]
    public void Lah_Works(int n, int k, long expected)
        => Assert.Equal(new BigInteger(expected), Lah(n, k));

    [Fact]
    public void Lah_FormulaCheck()
    {
        // L(n,k) = C(n-1, k-1) * n! / k!
        for (int n = 1; n <= 10; n++)
            for (int k = 1; k <= n; k++)
                Assert.Equal(Combinations(n - 1, k - 1) * Factorial(n) / Factorial(k), Lah(n, k));
    }

    [Fact]
    public void LahRow_MatchesIndividual()
    {
        for (int n = 1; n <= 8; n++)
        {
            var row = LahRow(n);
            for (int k = 1; k <= n; k++)
                Assert.Equal(Lah(n, k), row[k]);
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 13)]
    [InlineData(4, 75)]
    [InlineData(5, 541)]
    [InlineData(6, 4683)]
    public void OrderedBell_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), OrderedBell(n));

    [Fact]
    public void OrderedBell_MatchesSumOfFactorialTimesStirling()
    {
        for (int n = 0; n <= 10; n++)
        {
            BigInteger sum = 0;
            for (int k = 0; k <= n; k++)
                sum += Factorial(k) * StirlingSecondKind(n, k);
            Assert.Equal(OrderedBell(n), sum);
        }
    }
}