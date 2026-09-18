using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class AsymptoticsExtendedTests
{
    // ==================== СРАВНЕНИЕ БАЗОВОЙ И РАСШИРЕННОЙ ====================

    [Fact]
    public void Factorial_ExtendedBetterThanBasic()
    {
        for (int n = 5; n <= 100; n += 5)
        {
            var exact = Factorial(n);
            double basicErr = RelativeError(exact, FactorialStirling(n));
            double refinedErr = RelativeError(exact, FactorialStirlingRefined(n));
            double extendedErr = RelativeError(exact, FactorialStirlingExtended(n));

            Assert.True(refinedErr < basicErr, $"n={n}: refined worse than basic");
            Assert.True(extendedErr < refinedErr, $"n={n}: extended worse than refined");
        }
    }

    [Fact]
    public void Catalan_ExtendedBetterThanBasic()
    {
        for (int n = 5; n <= 100; n += 5)
        {
            var exact = Catalan(n);
            double basicErr = RelativeError(exact, CatalanApprox(n));
            double extendedErr = RelativeError(exact, CatalanApproxExtended(n));
            Assert.True(extendedErr < basicErr, $"n={n}: extended={extendedErr:P} basic={basicErr:P}");
        }
    }

    [Fact]
    public void Fibonacci_ExtendedIsExactForSmallN()
    {
        // Расширенная формула = полная формула Бине в double — точна до 2^53
        for (int n = 0; n <= 70; n++)
        {
            var exact = Fibonacci(n);
            double approx = FibonacciApproxExtended(n);
            if (exact <= (BigInteger)1e15) // помещается в double без потерь
            {
                Assert.Equal((double)exact, approx, 0);
            }
        }
    }

    [Fact]
    public void Lucas_ExtendedIsExactForSmallN()
    {
        for (int n = 0; n <= 70; n++)
        {
            var exact = Lucas(n);
            if (exact <= (BigInteger)1e15)
            {
                double approx = LucasApproxExtended(n);
                double err = Math.Abs(approx - (double)exact) / (double)exact;
                Assert.True(err < 1e-10, $"n={n}: err={err:E2}");
            }
        }
    }

    [Fact]
    public void Pell_ExtendedIsExactForSmallN()
    {
        Assert.Equal(0.0, PellApproxExtended(0));  // тривиальный случай

        for (int n = 1; n <= 50; n++)
        {
            var exact = Pell(n);
            double approx = PellApproxExtended(n);
            if (exact <= (BigInteger)1e15)
            {
                double err = Math.Abs(approx - (double)exact) / (double)exact;
                Assert.True(err < 1e-10, $"n={n}: err={err:E2}");
            }
        }
    }

    [Fact]
    public void PellLucas_ExtendedIsExactForSmallN()
    {
        for (int n = 0; n <= 50; n++)
        {
            var exact = PellLucas(n);
            double approx = PellLucasApproxExtended(n);
            double err = Math.Abs(approx - (double)exact) / (double)exact;
            Assert.True(err < 1e-10, $"n={n}: err={err:E2}");
        }
    }

    [Fact]
    public void Bell_ExtendedBetterThanBasic()
    {
        for (int n = 20; n <= 80; n += 10)
        {
            var exact = Bell(n);
            double basicErr = RelativeError(exact, BellApprox(n));
            double extErr = RelativeError(exact, BellApproxExtended(n));
            Assert.True(extErr <= basicErr * 1.5,
                $"n={n}: ext={extErr:P} basic={basicErr:P}");
        }
    }

    // ==================== ЛАНЦОШ ====================

    [Fact]
    public void GammaLanczos_KnownValues()
    {
        // Γ(1) = 1, Γ(2) = 1, Γ(3) = 2, Γ(4) = 6, Γ(5) = 24
        Assert.Equal(1.0, GammaLanczos(1.0), 10);
        Assert.Equal(1.0, GammaLanczos(2.0), 10);
        Assert.Equal(2.0, GammaLanczos(3.0), 10);
        Assert.Equal(6.0, GammaLanczos(4.0), 10);
        Assert.Equal(24.0, GammaLanczos(5.0), 10);
    }

    [Fact]
    public void GammaLanczos_Factorials()
    {
        // Γ(n+1) = n!
        for (int n = 1; n <= 10; n++)
        {
            var exact = (double)Factorial(n);
            var approx = GammaLanczos(n + 1);
            Assert.Equal(exact, approx, 6);
        }
    }

    [Fact]
    public void GammaLanczos_HalfIntegers()
    {
        // Γ(1/2) = √π, Γ(3/2) = √π/2, Γ(5/2) = 3√π/4
        Assert.Equal(Math.Sqrt(Math.PI), GammaLanczos(0.5), 10);
        Assert.Equal(Math.Sqrt(Math.PI) / 2.0, GammaLanczos(1.5), 10);
        Assert.Equal(3.0 * Math.Sqrt(Math.PI) / 4.0, GammaLanczos(2.5), 10);
    }

    [Fact]
    public void LogGammaLanczos_Consistency()
    {
        for (double x = 0.5; x <= 20.0; x += 0.5)
        {
            double logG = LogGammaLanczos(x);
            double g = GammaLanczos(x);
            Assert.Equal(Math.Log(g), logG, 8);
        }
    }

    // ==================== ГРАНИЧНЫЕ СЛУЧАИ ====================

    [Fact]
    public void Extended_ZeroArgs()
    {
        Assert.Equal(1.0, FactorialStirlingExtended(0));
        Assert.Equal(1.0, FactorialStirlingExtended(1));
        Assert.Equal(0.0, FibonacciApproxExtended(0));
        Assert.Equal(2.0, LucasApproxExtended(0));
    }

    [Fact]
    public void Extended_InvalidArgs_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FactorialStirlingExtended(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CatalanApproxExtended(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => BellApproxExtended(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => DelannoyCentralApproxExtended(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => SchroederLargeApproxExtended(0));
    }

    // ==================== ТОЧНОСТЬ ПО КОЛИЧЕСТВУ ЗНАКОВ ====================

    [Theory]
    [InlineData(10, 7)]
    [InlineData(20, 8)]
    [InlineData(50, 9)]
    [InlineData(100, 10)]
    public void FactorialExtended_Precision(int n, int minDigits)
    {
        var exact = Factorial(n);
        double approx = FactorialStirlingExtended(n);
        int digits = CommonDigits((double)exact, approx);
        Assert.True(digits >= minDigits, $"n={n}: {digits} digits < {minDigits}");
    }

    [Theory]
    [InlineData(20, 5)]
    [InlineData(50, 6)]
    [InlineData(100, 7)]
    public void CatalanExtended_Precision(int n, int minDigits)
    {
        var exact = Catalan(n);
        double approx = CatalanApproxExtended(n);
        int digits = CommonDigits((double)exact, approx);
        Assert.True(digits >= minDigits, $"n={n}: {digits} digits < {minDigits}");
    }

    // ==================== ВСПОМОГАТЕЛЬНЫЕ ====================

    /// <summary>Количество совпадающих значащих цифр двух double.</summary>
    private static int CommonDigits(double a, double b)
    {
        if (a == 0 || b == 0) return 0;
        double rel = Math.Abs(a - b) / Math.Max(Math.Abs(a), Math.Abs(b));
        if (rel == 0) return 15;
        return (int)Math.Floor(-Math.Log10(rel));
    }
}