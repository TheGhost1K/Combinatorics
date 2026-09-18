using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class NarayanaMotzkinTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(2, 2, 1)]
    [InlineData(3, 2, 3)]
    [InlineData(4, 2, 6)]
    [InlineData(4, 3, 6)]
    [InlineData(5, 3, 20)]
    [InlineData(6, 3, 50)]
    public void Narayana_Works(int n, int k, long expected)
        => Assert.Equal(new BigInteger(expected), Narayana(n, k));

    [Fact]
    public void Narayana_RowSumEqualsCatalan()
    {
        for (int n = 1; n <= 15; n++)
        {
            BigInteger sum = 0;
            for (int k = 1; k <= n; k++) sum += Narayana(n, k);
            Assert.Equal(Catalan(n), sum);
        }
    }

    [Fact]
    public void Narayana_Symmetry()
    {
        for (int n = 1; n <= 12; n++)
            for (int k = 1; k <= n; k++)
                Assert.Equal(Narayana(n, k), Narayana(n, n + 1 - k));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    [InlineData(4, 9)]
    [InlineData(5, 21)]
    [InlineData(10, 2188)]
    public void Motzkin_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Motzkin(n));

    [Fact]
    public void MotzkinSequence_MatchesIndividualCalls()
    {
        var seq = MotzkinSequence(15);
        for (int i = 0; i < 15; i++) Assert.Equal(Motzkin(i), seq[i]);
    }
}