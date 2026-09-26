using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class ExtendedTests
{
    // ==================== DELANNOY FAST ====================

    [Fact]
    public void DelannoyFast_MatchesDelannoy_Small()
    {
        for (int m = 0; m <= 20; m += 5)
            for (int n = 0; n <= 20; n += 5)
                Assert.Equal(Delannoy(m, n), DelannoyFast(m, n));
    }

    [Fact]
    public void DelannoyFast_MatchesDelannoy_Large()
    {
        // Порог 2500 — проверяем и выше, и ниже
        Assert.Equal(Delannoy(30, 30), DelannoyFast(30, 30));
        Assert.Equal(Delannoy(50, 50), DelannoyFast(50, 50));
        Assert.Equal(Delannoy(100, 100), DelannoyFast(100, 100));
    }

    [Fact]
    public void DelannoyFast_Symmetry()
    {
        for (int m = 0; m <= 15; m++)
            for (int n = 0; n <= 15; n++)
                Assert.Equal(DelannoyFast(m, n), DelannoyFast(n, m));
    }

    [Fact]
    public void DelannoyFast_ZeroAxis()
    {
        Assert.Equal(BigInteger.One, DelannoyFast(0, 0));
        Assert.Equal(BigInteger.One, DelannoyFast(0, 10));
        Assert.Equal(BigInteger.One, DelannoyFast(10, 0));
    }

    [Fact]
    public void DelannoyFast_NegativeThrows()
    {
        Assert.Throws<ArgumentException>(() => DelannoyFast(-1, 0));
        Assert.Throws<ArgumentException>(() => DelannoyFast(0, -1));
    }

    // ==================== ЧИСЛА ДЖЕНОККИ ====================

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, -1)]
    [InlineData(3, 0)]
    [InlineData(4, 1)]
    [InlineData(5, 0)]
    [InlineData(6, -3)]
    [InlineData(7, 0)]
    [InlineData(8, 17)]
    public void Genocchi_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Genocchi(n));

    [Fact]
    public void Genocchi_OddTermsAboveOneAreZero()
    {
        for (int n = 3; n <= 20; n += 2)
            Assert.Equal(BigInteger.Zero, Genocchi(n));
    }

    [Fact]
    public void Genocchi_RelationToBernoulli()
    {
        // G_n = 2(1 − 2^n) · B_n
        for (int n = 2; n <= 15; n += 2)
        {
            var b = Bernoulli(n);
            BigInteger coeff = 2 * (BigInteger.One - BigInteger.Pow(2, n));
            var expected = coeff * b;
            var actual = new BigRational(Genocchi(n), 1);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void GenocchiSequence_MatchesIndividual()
    {
        var seq = GenocchiSequence(15);
        for (int i = 0; i < 15; i++)
            Assert.Equal(Genocchi(i), seq[i]);
    }

    [Fact]
    public void GenocchiSequence_FirstValues()
    {
        var seq = GenocchiSequence(10);
        var expected = new BigInteger[]
        {
            0, 1, -1, 0, 1, 0, -3, 0, 17, 0
        };
        Assert.Equal(expected, seq);
    }

    [Fact]
    public void Genocchi_Negative_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Genocchi(-1));

    // ==================== ВКЛЮЧЕНИЯ-ИСКЛЮЧЕНИЯ ====================

    [Fact]
    public void InclusionExclusion_ThreeSets()
    {
        // |A ∪ B ∪ C| = 10 + 15 + 20 − 5 − 4 − 3 + 1 = 34
        var sizes = new[] { 10, 15, 20 };
        var result = InclusionExclusion(sizes, idx => idx.Length switch
        {
            1 => sizes[idx[0]],
            2 => idx switch
            {
                [0, 1] => 5,
                [0, 2] => 4,
                [1, 2] => 3,
                _ => 0
            },
            3 => 1,
            _ => 0
        });
        Assert.Equal(new BigInteger(34), result);
    }

    [Fact]
    public void InclusionExclusion_TwoSets()
    {
        // |A ∪ B| = |A| + |B| − |A ∩ B| = 10 + 20 − 3 = 27
        var sizes = new[] { 10, 20 };
        var result = InclusionExclusion(sizes, idx =>
            idx.Length == 1 ? sizes[idx[0]] : new BigInteger(3));
        Assert.Equal(new BigInteger(27), result);
    }

    [Fact]
    public void InclusionExclusion_OneSet()
    {
        var sizes = new[] { 42 };
        var result = InclusionExclusion(sizes, _ => new BigInteger(42));
        Assert.Equal(new BigInteger(42), result);
    }

    [Fact]
    public void InclusionExclusion_Empty()
    {
        var result = InclusionExclusion(Array.Empty<int>(), _ => BigInteger.Zero);
        Assert.Equal(BigInteger.Zero, result);
    }

    [Fact]
    public void InclusionExclusion_EmptyIntersections()
    {
        // Если все пересечения пусты, |∪ Aᵢ| = Σ |Aᵢ|
        var sizes = new[] { 5, 10, 15, 20 };
        var result = InclusionExclusion(sizes, idx =>
            idx.Length == 1 ? sizes[idx[0]] : BigInteger.Zero);
        Assert.Equal(new BigInteger(50), result);
    }

    [Fact]
    public void InclusionExclusion_FullOverlap()
    {
        // Если все множества совпадают и |Aᵢ| = 10, то |∪ Aᵢ| = 10
        var sizes = new[] { 10, 10, 10 };
        var result = InclusionExclusion(sizes, _ => new BigInteger(10));
        Assert.Equal(new BigInteger(10), result);
    }

    [Fact]
    public void InclusionExclusion_NullThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
            InclusionExclusion(null!, _ => BigInteger.Zero));
        Assert.Throws<ArgumentNullException>(() =>
            InclusionExclusion(new[] { 1 }, null!));
    }
}