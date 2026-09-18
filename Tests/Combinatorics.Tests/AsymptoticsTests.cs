using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class AsymptoticsTests
{

    // Порог относительной погрешности для каждого семейства
    private const double TolLoose = 0.05;  // 5 %
    private const double TolTight = 0.01;  // 1 %

    [Fact]
    public void FibonacciApprox_Converges()
    {
        // относительная погрешность убывает с ростом n
        for (int n = 5; n <= 30; n++)
        {
            double approx = FibonacciApprox(n);
            BigInteger exact = Fibonacci(n);
            double err = RelativeError(exact, approx);
            Assert.True(err < TolTight, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void LucasApprox_Converges()
    {
        for (int n = 5; n <= 30; n++)
        {
            double approx = LucasApprox(n);
            double err = RelativeError(Lucas(n), approx);
            Assert.True(err < TolTight, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void PellApprox_Converges()
    {
        for (int n = 5; n <= 25; n++)
        {
            double approx = PellApprox(n);
            double err = RelativeError(Pell(n), approx);
            Assert.True(err < TolTight, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void PellLucasApprox_Converges()
    {
        for (int n = 5; n <= 25; n++)
        {
            double approx = PellLucasApprox(n);
            double err = RelativeError(PellLucas(n), approx);
            Assert.True(err < TolTight, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void FactorialStirling_Converges()
    {
        for (int n = 10; n <= 30; n++)
        {
            double approx = FactorialStirling(n);
            double err = RelativeError(Factorial(n), approx);
            Assert.True(err < TolLoose, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void FactorialStirlingRefined_BetterThanBasic()
    {
        for (int n = 5; n <= 100; n += 5) 
        {
            double lnFact = LogFactorialLanczos(n);
            double lnBasic = Math.Log(FactorialStirling(n));
            double lnRefined = Math.Log(FactorialStirlingRefined(n));
            double errBasic = Math.Abs(lnBasic - lnFact);
            double errRefined = Math.Abs(lnRefined - lnFact);
            Assert.True(errRefined < errBasic, $"n={n}: refined={errRefined} basic={errBasic}");
        }
    }

    [Fact]
    public void CombinationsApprox_Converges()
    {
        for (int n = 20; n <= 200; n += 20)
        {
            for (int k = 3; k <= n - 3; k += Math.Max(1, n / 5))
            {
                double approx = CombinationsApprox(n, k);
                BigInteger exact = Combinations(n, k);
                double err = RelativeError(exact, approx);
                Assert.True(err < 0.05, $"n={n}, k={k}, err={err:P}");  // 5%
            }
        }
    }

    [Fact]
    public void MotzkinApprox_Converges()
    {
        for (int n = 50; n <= 500; n += 50)
        {
            double approx = MotzkinApprox(n);
            double err = RelativeError(Motzkin(n), approx);
            Assert.True(err < TolLoose, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void CatalanApprox_Converges()
    {
        for (int n = 50; n <= 500; n += 50)   // n ≥ 50
        {
            double approx = CatalanApprox(n);
            BigInteger exact = Catalan(n);
            double err = RelativeError(exact, approx);
            Assert.True(err < 0.10, $"n={n}, err={err:P}");   // 10 %
        }
    }

    [Fact]
    public void SchroederLargeApprox_Converges()
    {
        double prevErr = double.PositiveInfinity;
        for (int n = 30; n <= 300; n += 30)
        {
            double approx = SchroederLargeApproxExtended(n);
            BigInteger exact = SchroederLarge(n);
            double err = RelativeError(exact, approx);
            Assert.True(err < prevErr, $"n={n}: err={err:P} >= prev={prevErr:P}");
            prevErr = err;
        }
        Assert.True(prevErr < 0.01, $"final err={prevErr:P}");
    }

    [Fact]
    public void BellApprox_Converges()
    {
        for (int n = 10; n <= 200; n += 10)
        {
            double approx = BellApproxExtended(n);
            BigInteger exact = Bell(n);
            double err = RelativeError(exact, approx);
            Assert.True(err < 0.02, $"n={n}, err={err:P}");
        }
    }

    [Fact]
    public void DelannoyCentralApprox_Converges()
    {
        for (int n = 20; n <= 200; n += 20)
        {
            double approx = DelannoyCentralApprox(n);
            BigInteger exact = DelannoyCentral(n);
            double err = RelativeError(exact, approx);
            Assert.True(err < 0.01, $"n={n}, err={err:P}");   // 1 %
        }
    }

    // ==================== ФУНКЦИЯ ЛАМБЕРТА ====================

    [Fact]
    public void LambertW_AtOne()
    {
        // W(1) ≈ 0.5671432904...
        double w = LambertW(1.0);
        Assert.Equal(0.5671432904, w, 8);
    }

    [Fact]
    public void LambertW_AtE()
    {
        // W(e) = 1
        double w = LambertW(Math.E);
        Assert.Equal(1.0, w, 10);
    }

    [Fact]
    public void LambertW_Identity()
    {
        // W(x)·exp(W(x)) = x
        for (double x = 0.5; x <= 100; x *= 2)
        {
            double w = LambertW(x);
            double check = w * Math.Exp(w);
            Assert.Equal(x, check, 8);
        }
    }

    [Fact]
    public void LambertW_LargeX()
    {
        double x = 1e15;
        double w = LambertW(x);
        // W(x) ≈ ln(x) − ln(ln(x)) для больших x
        double expected = Math.Log(x) - Math.Log(Math.Log(x));
        Assert.True(Math.Abs(w - expected) / expected < 0.01);
    }

    // ==================== ГРАНИЧНЫЕ СЛУЧАИ ====================

    [Fact]
    public void Approx_ZeroArgs()
    {
        Assert.Equal(1.0, FactorialStirling(0));
        Assert.Equal(1.0, FactorialStirlingRefined(0));
        Assert.Equal(1.0, CombinationsApprox(5, 0));
        Assert.Equal(1.0, CombinationsApprox(5, 5));
    }

    [Fact]
    public void Approx_InvalidArgs_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CatalanApprox(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => BellApprox(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => MotzkinApprox(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => LambertW(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => LambertW(-1));
    }
}