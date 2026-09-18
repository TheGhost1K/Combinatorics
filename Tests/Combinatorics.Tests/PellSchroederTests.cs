using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class PellSchroederTests
{
    // ==================== ПЕЛЛЬ ====================

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 5)]
    [InlineData(4, 12)]
    [InlineData(5, 29)]
    [InlineData(6, 70)]
    [InlineData(7, 169)]
    [InlineData(8, 408)]
    [InlineData(9, 985)]
    [InlineData(10, 2378)]
    public void Pell_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Pell(n));

    [Fact]
    public void Pell_Recurrence()
    {
        for (int n = 2; n <= 30; n++)
            Assert.Equal(2 * Pell(n - 1) + Pell(n - 2), Pell(n));
    }

    [Fact]
    public void PellSequence_MatchesIndividual()
    {
        var seq = PellSequence(25);
        for (int i = 0; i < 25; i++) Assert.Equal(Pell(i), seq[i]);
    }

    [Fact]
    public void Pell_Negative_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Pell(-1));

    // ==================== ПЕЛЛЬ–ЛЮКА ====================

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 2)]
    [InlineData(2, 6)]
    [InlineData(3, 14)]
    [InlineData(4, 34)]
    [InlineData(5, 82)]
    [InlineData(6, 198)]
    [InlineData(7, 478)]
    [InlineData(8, 1154)]
    [InlineData(9, 2786)]
    public void PellLucas_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), PellLucas(n));

    [Fact]
    public void PellLucas_Recurrence()
    {
        for (int n = 2; n <= 30; n++)
            Assert.Equal(2 * PellLucas(n - 1) + PellLucas(n - 2), PellLucas(n));
    }

    [Fact]
    public void PellLucasSequence_MatchesIndividual()
    {
        var seq = PellLucasSequence(25);
        for (int i = 0; i < 25; i++) Assert.Equal(PellLucas(i), seq[i]);
    }

    [Fact]
    public void PellLucas_RelationToPell()
    {
        for (int n = 1; n <= 20; n++)  // с n=1
            Assert.Equal(2 * Pell(n) + 2 * Pell(n - 1), PellLucas(n));
    }

    [Fact]
    public void PellLucas_Identity()
    {
        // Q(n)² − 8·P(n)² = 4·(−1)^n
        for (int n = 0; n <= 20; n++)
            Assert.True(PellLucasIdentity(n), $"Identity failed for n={n}");
    }

    [Fact]
    public void PellLucas_DoublingFormula()
    {
        // P(2n) = P(n) · Q(n)
        for (int n = 0; n <= 15; n++)
            Assert.Equal(Pell(2 * n), Pell(n) * PellLucas(n));
    }

    // ==================== МОЦКИН С ДВУМЯ ЦВЕТАМИ ====================

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 5)]
    [InlineData(3, 14)]
    [InlineData(4, 42)]
    [InlineData(5, 132)]
    [InlineData(6, 429)]
    [InlineData(7, 1430)]
    public void MotzkinTwoColored_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), MotzkinTwoColored(n));

    [Fact]
    public void MotzkinTwoColored_EqualsCatalanShifted()
    {
        // M2(n) = Catalan(n+1)
        for (int n = 0; n <= 20; n++)
            Assert.Equal(Catalan(n + 1), MotzkinTwoColored(n));
    }

    [Fact]
    public void MotzkinTwoColored_Recurrence()
    {
        // M(0) = 1; M(n+1) = 2·M(n) + Σ M(i)M(n-1-i)
        for (int n = 1; n <= 20; n++)
        {
            BigInteger sum = 2 * MotzkinTwoColored(n - 1);
            for (int i = 0; i < n - 1; i++)
                sum += MotzkinTwoColored(i) * MotzkinTwoColored(n - 2 - i);
            Assert.Equal(sum, MotzkinTwoColored(n));
        }
    }

    [Fact]
    public void MotzkinTwoColoredSequence_MatchesIndividual()
    {
        var seq = MotzkinTwoColoredSequence(20);
        for (int i = 0; i < 20; i++) Assert.Equal(MotzkinTwoColored(i), seq[i]);
    }

    // ==================== ШРЁДЕР–КАТАЛАН ====================

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 11)]
    [InlineData(4, 45)]
    [InlineData(5, 197)]
    [InlineData(6, 903)]
    [InlineData(7, 4279)]
    [InlineData(8, 20793)]
    public void SchroederCatalan_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), SchroederCatalan(n));

    [Fact]
    public void SchroederCatalan_RelationToLarge()
    {
        // Для n ≥ 1: s(n) = S(n) / 2
        for (int n = 1; n <= 15; n++)
            Assert.Equal(SchroederLarge(n), 2 * SchroederCatalan(n));
    }

    [Fact]
    public void SchroederCatalan_S0_EqualsS0Large()
    {
        Assert.Equal(BigInteger.One, SchroederCatalan(0));
        Assert.Equal(BigInteger.One, SchroederLarge(0));
    }

    [Fact]
    public void SchroederCatalan_BySum_GivesLarge()
    {
        // Формула суммы R(n) = Σ C(n+k, 2k)·C_k даёт БОЛЬШИЕ числа Шрёдера
        for (int n = 0; n <= 15; n++)
            Assert.Equal(SchroederLarge(n), SchroederCatalanBySum(n));
    }

    [Fact]
    public void SchroederCatalanSequence_MatchesIndividual()
    {
        var seq = SchroederCatalanSequence(15);
        for (int i = 0; i < 15; i++) Assert.Equal(SchroederCatalan(i), seq[i]);
    }

    [Fact]
    public void SchroederCatalan_Recurrence()
    {
        // Большие Шрёдера: (n+2)S(n+1) = 3(2n+1)S(n) − (n−1)S(n−1)
        for (int n = 1; n <= 15; n++)
        {
            BigInteger lhs = (n + 2) * SchroederLarge(n + 1);
            BigInteger rhs = 3 * (2 * n + 1) * SchroederLarge(n)
                            - (n - 1) * SchroederLarge(n - 1);
            Assert.Equal(lhs, rhs);
        }
    }
}