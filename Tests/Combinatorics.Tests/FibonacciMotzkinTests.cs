using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class FibonacciMotzkinTests
{
    // ==================== ФИБОНАЧЧИ ====================

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    [InlineData(7, 13)]
    [InlineData(8, 21)]
    [InlineData(10, 55)]
    [InlineData(20, 6765)]
    [InlineData(30, 832040)]
    [InlineData(50, 12586269025L)]
    public void Fibonacci_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Fibonacci(n));

    [Fact]
    public void Fibonacci_Recurrence()
    {
        for (int n = 2; n <= 100; n++)
            Assert.Equal(Fibonacci(n - 1) + Fibonacci(n - 2), Fibonacci(n));
    }

    [Fact]
    public void FibonacciFast_MatchesIterative()
    {
        for (int n = 0; n <= 200; n++)
            Assert.Equal(Fibonacci(n), FibonacciFast(n));
    }

    [Fact]
    public void FibonacciSequence_MatchesIndividual()
    {
        var seq = FibonacciSequence(50);
        for (int i = 0; i < 50; i++) Assert.Equal(Fibonacci(i), seq[i]);
    }

    [Fact]
    public void Fibonacci_Negative_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Fibonacci(-1));

    [Fact]
    public void FibonacciPair_Consistency()
    {
        for (int n = 0; n <= 50; n++)
        {
            var (fn, fn1) = FibonacciPair(n);
            Assert.Equal(Fibonacci(n), fn);
            Assert.Equal(Fibonacci(n + 1), fn1);
        }
    }

    [Fact]
    public void FibonacciFast_LargeN()
    {
        // F(1000) — известное значение из OEIS
        var f1000 = FibonacciFast(1000);
        var expected = BigInteger.Parse(
            "43466557686937456435688527675040625802564660517371780402481729089536555417949051890403879840079255169295922593080322634775209689623239873322471161642996440906533187938298969649928516003704476137795166849228875");
        Assert.Equal(expected, f1000);
    }

    // ==================== ЛЮКА ====================

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(4, 7)]
    [InlineData(5, 11)]
    [InlineData(6, 18)]
    [InlineData(7, 29)]
    [InlineData(8, 47)]
    [InlineData(10, 123)]
    public void Lucas_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), Lucas(n));

    [Fact]
    public void Lucas_Recurrence()
    {
        for (int n = 2; n <= 100; n++)
            Assert.Equal(Lucas(n - 1) + Lucas(n - 2), Lucas(n));
    }

    [Fact]
    public void LucasSequence_MatchesIndividual()
    {
        var seq = LucasSequence(50);
        for (int i = 0; i < 50; i++) Assert.Equal(Lucas(i), seq[i]);
    }

    // ==================== ТОЖДЕСТВА ====================

    [Fact]
    public void CassiniIdentity_Holds()
    {
        for (int n = 0; n <= 50; n++)
            Assert.True(CassiniIdentity(n), $"Cassini failed for n={n}");
    }

    [Fact]
    public void LucasIdentity_Holds()
    {
        for (int n = 0; n <= 50; n++)
            Assert.True(LucasIdentity(n), $"Lucas identity failed for n={n}");
    }

    [Fact]
    public void DoublingFormula_Holds()
    {
        for (int n = 0; n <= 50; n++)
            Assert.True(DoublingFormula(n), $"F(2n)=F(n)L(n) failed for n={n}");
    }

    [Fact]
    public void LucasFromFibonacci_Holds()
    {
        for (int n = 1; n <= 50; n++)
            Assert.True(LucasFromFibonacci(n), $"L(n)=F(n-1)+F(n+1) failed for n={n}");
    }

    [Fact]
    public void Fibonacci_GcdProperty()
    {
        // gcd(F(m), F(n)) = F(gcd(m, n))
        for (int m = 1; m <= 20; m++)
            for (int n = 1; n <= 20; n++)
            {
                BigInteger g1 = BigInteger.GreatestCommonDivisor(Fibonacci(m), Fibonacci(n));
                BigInteger g2 = Fibonacci(Gcd(m, n));
                Assert.Equal(g2, g1);
            }
    }

    // ==================== ОБОБЩЁННЫЕ МОЦКИНА ====================

    [Fact]
    public void MotzkinGeneralized_M1_EqualsMotzkin()
    {
        for (int n = 0; n <= 20; n++)
            Assert.Equal(Motzkin(n), MotzkinGeneralized(n, 1));
    }

    [Fact]
    public void MotzkinGeneralized_M2_EqualsTwoColored()
    {
        for (int n = 0; n <= 20; n++)
            Assert.Equal(MotzkinTwoColored(n), MotzkinGeneralized(n, 2));
    }

    [Fact]
    public void MotzkinGeneralized_M2_EqualsCatalanShifted()
    {
        for (int n = 0; n <= 20; n++)
            Assert.Equal(Catalan(n + 1), MotzkinGeneralized(n, 2));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 3)]
    [InlineData(2, 10)]
    [InlineData(3, 36)]
    [InlineData(4, 137)]
    [InlineData(5, 543)]
    [InlineData(6, 2219)]
    [InlineData(7, 9285)]
    public void MotzkinGeneralized_M3_Works(int n, long expected)
        => Assert.Equal(new BigInteger(expected), MotzkinGeneralized(n, 3));

    [Fact]
    public void MotzkinGeneralized_M0_AllOnes()
    {
        // m = 0: только пустой путь остаётся → M(0)=1, M(n>0)=0? Нет:
        // с m=0 формула даёт M(0)=1, M(1)=0, M(2)=1·1=1? Проверим численно:
        // M(n+1) = 0·M(n) + Σ M(i)M(n-1-i).
        // M(1) = 0·M(0) = 0.
        // M(2) = 0·M(1) + M(0)·M(0) = 1.
        // M(3) = 0·M(2) + M(0)·M(1) + M(1)·M(0) = 0.
        // Значит ненулевые только чётные: 1, 0, 1, 0, 2, 0, 5, ... — это числа Каталана через раз.
        Assert.Equal(BigInteger.One, MotzkinGeneralized(0, 0));
        Assert.Equal(BigInteger.Zero, MotzkinGeneralized(1, 0));
        Assert.Equal(BigInteger.One, MotzkinGeneralized(2, 0));
        Assert.Equal(BigInteger.Zero, MotzkinGeneralized(3, 0));
        Assert.Equal(new BigInteger(2), MotzkinGeneralized(4, 0));
        Assert.Equal(BigInteger.Zero, MotzkinGeneralized(5, 0));
        Assert.Equal(new BigInteger(5), MotzkinGeneralized(6, 0));
    }

    [Fact]
    public void MotzkinGeneralized_BySum_MatchesRecurrence()
    {
        for (int m = 0; m <= 5; m++)
            for (int n = 0; n <= 20; n++)
                Assert.Equal(MotzkinGeneralized(n, m), MotzkinGeneralizedBySum(n, m));
    }

    [Fact]
    public void MotzkinGeneralizedSequence_MatchesIndividual()
    {
        for (int m = 1; m <= 4; m++)
        {
            var seq = MotzkinGeneralizedSequence(15, m);
            for (int i = 0; i < 15; i++)
                Assert.Equal(MotzkinGeneralized(i, m), seq[i]);
        }
    }

    [Fact]
    public void MotzkinGeneralized_NegativeArgs_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MotzkinGeneralized(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => MotzkinGeneralized(5, -1));
    }

    [Fact]
    public void MotzkinGeneralized_ConvenienceWrappers()
    {
        for (int n = 0; n <= 15; n++)
        {
            Assert.Equal(MotzkinGeneralized(n, 1), MotzkinGeneralized1(n));
            Assert.Equal(MotzkinGeneralized(n, 2), MotzkinGeneralized2(n));
            Assert.Equal(MotzkinGeneralized(n, 3), MotzkinGeneralized3(n));
        }
    }

    // ==================== ВСПОМОГАТЕЛЬНЫЕ ====================

    private static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);
}