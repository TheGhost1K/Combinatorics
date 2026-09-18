using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class BernoulliTests
{
    [Fact]
    public void Bernoulli_FirstValues()
    {
        Assert.Equal(new BigRational(1, 1), Bernoulli(0));
        Assert.Equal(new BigRational(-1, 2), Bernoulli(1));
        Assert.Equal(new BigRational(1, 6), Bernoulli(2));
        Assert.Equal(BigRational.Zero, Bernoulli(3));
        Assert.Equal(new BigRational(-1, 30), Bernoulli(4));
        Assert.Equal(BigRational.Zero, Bernoulli(5));
        Assert.Equal(new BigRational(1, 42), Bernoulli(6));
        Assert.Equal(BigRational.Zero, Bernoulli(7));
        Assert.Equal(new BigRational(-1, 30), Bernoulli(8));
    }

    [Fact]
    public void Bernoulli_OddTermsAboveOneAreZero()
    {
        for (int n = 3; n <= 20; n += 2)
            Assert.Equal(BigRational.Zero, Bernoulli(n));
    }

    [Fact]
    public void BernoulliSequence_MatchesIndividual()
    {
        var seq = BernoulliSequence(20);
        for (int i = 0; i < 20; i++) Assert.Equal(Bernoulli(i), seq[i]);
    }

    [Fact]
    public void Bernoulli_RecurrenceHolds()
    {
        // sum_{k=0}^{n} C(n+1, k) B_k = 0 для n ≥ 1
        for (int n = 1; n <= 12; n++)
        {
            BigRational sum = BigRational.Zero;
            for (int k = 0; k <= n; k++)
                sum += Combinations(n + 1, k) * Bernoulli(k);
            Assert.Equal(BigRational.Zero, sum);
        }
    }
}