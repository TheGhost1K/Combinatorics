using System.Numerics;

namespace Combinatorics;

public static partial class Combinatorics
{
    // ==================== ФИБОНАЧЧИ / ЛЮКА ====================

    /// <summary>Асимптотика <c>F(n) ≈ φ^n / √5</c>.</summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение F(n) в <see cref="double"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>Относительная погрешность O(φ^{−2n}) — убывает экспоненциально.</remarks>
    public static double FibonacciApprox(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        return Math.Pow(MathConstants.Phi, n) / MathConstants.Sqrt5;
    }

    /// <summary>Асимптотика <c>L(n) ≈ φ^n</c>.</summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение L(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    public static double LucasApprox(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        return Math.Pow(MathConstants.Phi, n);
    }

    // ==================== ПЕЛЛЬ ====================

    /// <summary>Асимптотика <c>P(n) ≈ (1+√2)^n / (2√2)</c>.</summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение P(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    public static double PellApprox(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        return Math.Pow(MathConstants.SilverRatio, n) / (2.0 * MathConstants.Sqrt2);
    }

    /// <summary>Асимптотика <c>Q(n) ≈ (1+√2)^n</c>.</summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение Q(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    public static double PellLucasApprox(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        return Math.Pow(MathConstants.SilverRatio, n);
    }

    // ==================== КАТАЛАН ====================

    /// <summary>Асимптотика <c>C(n) ≈ 4^n / (n^{3/2} · √π)</c>.</summary>
    /// <param name="n">Число, n ≥ 1.</param>
    /// <returns>Приближённое значение C_n.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> ≤ 0.</exception>
    /// <remarks>Относительная погрешность O(1/n). См. <see cref="CatalanApproxExtended(int)"/> для O(1/n⁴).</remarks>
    public static double CatalanApprox(int n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n), "n ≥ 1");
        return Math.Pow(4.0, n) / (Math.Pow(n, 1.5) * MathConstants.SqrtPi);
    }

    // ==================== ФАКТОРИАЛ ====================

    /// <summary>Приближение Стирлинга: <c>n! ≈ √(2πn) · (n/e)^n</c>.</summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение n!.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>Относительная погрешность O(1/n). См. <see cref="FactorialStirlingExtended(int)"/> для O(1/n⁴).</remarks>
    public static double FactorialStirling(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        if (n == 0) return 1.0;
        return Math.Sqrt(2.0 * Math.PI * n) * Math.Pow(n / MathConstants.E, n);
    }

    /// <summary>
    /// Уточнённое приближение Стирлинга: <c>n! ≈ √(2πn) · (n/e)^n · (1 + 1/(12n))</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Приближённое значение n!.</returns>
    /// <remarks>Относительная погрешность O(1/n³) — на порядки точнее базовой.</remarks>
    public static double FactorialStirlingRefined(int n)
    {
        if (n <= 0) return 1.0;
        double baseApprox = Math.Sqrt(2.0 * Math.PI * n) * Math.Pow(n / MathConstants.E, n);
        return baseApprox * (1.0 + 1.0 / (12.0 * n));
    }

    // ==================== СОЧЕТАНИЯ ====================

    /// <summary>
    /// Асимптотика <c>C(n, k) ≈ n^n / (k^k · (n−k)^{n−k}) · √(n / (2π·k·(n−k)))</c>.
    /// </summary>
    /// <param name="n">Общее число, n ≥ 0.</param>
    /// <param name="k">Число выбираемых, 0 ≤ k ≤ n.</param>
    /// <returns>Приближённое значение C(n, k).</returns>
    /// <exception cref="ArgumentException">Если n &lt; 0, k &lt; 0 или k &gt; n.</exception>
    /// <remarks>Для граничных случаев k = 0 и k = n возвращает 1.</remarks>
    public static double CombinationsApprox(int n, int k)
    {
        if (n < 0 || k < 0 || k > n) throw new ArgumentException("0 ≤ k ≤ n");
        if (k == 0 || k == n) return 1.0;
        double logN = n * Math.Log(n);
        double logK = k * Math.Log(k);
        double logNK = (n - k) * Math.Log(n - k);
        double logPrefactor = 0.5 * (Math.Log(n) - Math.Log(2.0 * Math.PI * k * (n - k)));
        return Math.Exp(logN - logK - logNK + logPrefactor);
    }

    // ==================== БЕЛЛ ====================

    /// <summary>
    /// Асимптотика Белла через функцию Ламберта W:
    /// <c>B(n) ≈ n^{−1/2} · (n/W(n))^{n+1/2} · exp(n/W(n) − n − 1)</c>.
    /// </summary>
    /// <param name="n">Число, n ≥ 1.</param>
    /// <returns>Приближённое значение B_n.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 1.</exception>
    /// <remarks>Относительная погрешность O(1/n). См. <see cref="BellApproxExtended(int)"/>.</remarks>
    public static double BellApprox(int n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n), "n ≥ 1");
        double w = LambertW(n);
        double logB = -0.5 * Math.Log(n)
                    + (n + 0.5) * Math.Log(n / w)
                    + n / w - n - 1.0;
        return Math.Exp(logB);
    }

    // ==================== ДЕЛАННУА / ШРЁДЕР / МОЦКИН ====================

    /// <summary>
    /// Асимптотика центральных Деланнуа: <c>D(n,n) ≈ (3+2√2)^n / (2^{3/4} · √(πn))</c>.
    /// </summary>
    /// <param name="n">Число, n ≥ 1.</param>
    /// <returns>Приближённое значение D(n, n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> ≤ 0.</exception>
    public static double DelannoyCentralApprox(int n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n), "n ≥ 1");
        double numer = Math.Pow(MathConstants.CatalConst, n);
        double denom = Math.Sqrt(4.0 * Math.PI * (3.0 * MathConstants.Sqrt2 - 4.0) * n);//double denom = Math.Pow(2.0, 0.75) * Math.Sqrt(Math.PI * n);
        return numer / denom;
    }

    /// <summary>
    /// Асимптотика больших Шрёдера: <c>S(n) ≈ (3+2√2)^n / (2^{3/4} · √(πn³))</c>.
    /// </summary>
    /// <param name="n">Число, n ≥ 1.</param>
    /// <returns>Приближённое значение S_n.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> ≤ 0.</exception>
    public static double SchroederLargeApprox(int n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n), "n ≥ 1");
        double numer = Math.Pow(MathConstants.CatalConst, n);
        double denom = Math.Sqrt(2.0 * Math.PI) * Math.Pow(n, 1.5) * Math.Sqrt(3.0 * MathConstants.Sqrt2 - 4.0);
        return numer / denom;
    }

    /// <summary>
    /// Асимптотика Моцкина: <c>M(n) ≈ 3^{n+3/2} / (2 · √π · n^{3/2})</c>.
    /// </summary>
    /// <param name="n">Число, n ≥ 1.</param>
    /// <returns>Приближённое значение M_n.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> ≤ 0.</exception>
    public static double MotzkinApprox(int n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n), "n ≥ 1");
        double numer = Math.Pow(3.0, n + 1.5);
        double denom = 2.0 * MathConstants.SqrtPi * Math.Pow(n, 1.5);
        return numer / denom;
    }

    // ==================== ВСПОМОГАТЕЛЬНЫЕ ====================

    /// <summary>
    /// Функция Ламберта <c>W(x)</c> — решение уравнения <c>W·e^W = x</c>.
    /// </summary>
    /// <param name="x">Аргумент, x &gt; 0.</param>
    /// <returns>Значение W(x).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="x"/> ≤ 0.</exception>
    /// <remarks>
    /// Использует метод Галлея с кубической сходимостью. Точность ~10^{-15}.
    /// Для <c>x = e</c> возвращает 1, для <c>x = 1</c> — 0.5671432904…
    /// </remarks>
    /// <example>
    /// <code>
    /// var w = LambertW(Math.E); // 1.0
    /// </code>
    /// </example>
    public static double LambertW(double x)
    {
        if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x), "x > 0");
        if (double.IsPositiveInfinity(x)) return double.PositiveInfinity;

        double w;
        if (x < 3.0) w = 0.5;
        else if (x < 100.0) w = Math.Log(x) - Math.Log(Math.Log(x));
        else w = Math.Log(x) - Math.Log(Math.Log(x)) + Math.Log(Math.Log(x)) / Math.Log(x);

        for (int i = 0; i < 30; i++)
        {
            double ew = Math.Exp(w);
            double f = w * ew - x;
            double wp1 = w + 1.0;
            double denom = ew * wp1 - (w + 2.0) * f / (2.0 * wp1);
            double delta = f / denom;
            w -= delta;
            if (Math.Abs(delta) < 1e-15 * Math.Abs(w) + 1e-15) break;
        }
        return w;
    }

    /// <summary>
    /// Относительная погрешность приближения: <c>|approx − exact| / exact</c>.
    /// </summary>
    /// <param name="exact">Точное значение.</param>
    /// <param name="approx">Приближённое значение.</param>
    /// <returns>Относительная погрешность как доля от 1.</returns>
    /// <remarks>Если <paramref name="exact"/> равен нулю, возвращает <c>|approx|</c>.</remarks>
    /// <example>
    /// <code>
    /// var err = RelativeError(Factorial(20), FactorialStirling(20));
    /// // ~0.0004 (0.04%)
    /// </code>
    /// </example>
    public static double RelativeError(BigInteger exact, double approx)
    {
        if (exact.IsZero) return Math.Abs(approx);
        double exactDouble = (double)exact;
        return Math.Abs(approx - exactDouble) / exactDouble;
    }
}