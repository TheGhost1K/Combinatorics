using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class BellEulerTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 5)]
    [InlineData(4, 15)]
    [InlineData(5, 52)]
    [InlineData(6, 203)]
    [InlineData(10, 115975)]
    public void Bell_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Bell(n));

    [Fact]
    public void BellSequence_MatchesIndividualCalls()
    {
        var seq = BellSequence(15);
        for (int i = 0; i < 15; i++) Assert.Equal(Bell(i), seq[i]);
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(2, 0, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(3, 0, 1)]
    [InlineData(3, 1, 4)]
    [InlineData(3, 2, 1)]
    [InlineData(4, 1, 11)]
    [InlineData(5, 2, 66)]
    [InlineData(5, 3, 26)]
    public void Eulerian_Works(int n, int k, long expected)
        => Assert.Equal(new BigInteger(expected), Eulerian(n, k));

    [Fact]
    public void Eulerian_RowSumEqualsFactorial()
    {
        for (int n = 1; n <= 15; n++)
        {
            BigInteger sum = 0;
            for (int k = 0; k < n; k++) sum += Eulerian(n, k);
            Assert.Equal(Factorial(n), sum);
        }
    }

    [Fact]
    public void Eulerian_Symmetry()
    {
        for (int n = 1; n <= 12; n++)
            for (int k = 0; k < n; k++)
                Assert.Equal(Eulerian(n, k), Eulerian(n, n - 1 - k));
    }

    [Fact]
    public void Eulerian_MatchesRow()
    {
        for (int n = 1; n <= 10; n++)
        {
            var row = EulerianRow(n);
            for (int k = 0; k < n; k++)
                Assert.Equal(Eulerian(n, k), row[k]);
        }
    }

    [Fact]
    public void EulerianDescents_IsMirror()
    {
        for (int n = 1; n <= 10; n++)
            for (int k = 0; k < n; k++)
                Assert.Equal(Eulerian(n, k), EulerianDescents(n, n - 1 - k));
    }
}