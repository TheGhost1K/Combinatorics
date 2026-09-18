using System.Numerics;

namespace Combinatorics;

public static partial class Combinatorics
{
    /// <summary>
    /// n-е число Фибоначчи: <c>F(0) = 0, F(1) = 1, F(n) = F(n−1) + F(n−2)</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение F(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// <para>Итеративная реализация O(n). Для больших n используйте <see cref="FibonacciFast(int)"/> (O(log n)).</para>
    /// <para>Формула Бине: <c>F(n) = (φ^n − ψ^n) / √5</c> — см. <see cref="FibonacciBinet(int)"/>.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var f30 = Fibonacci(30); // 832040
    /// </code>
    /// </example>
    /// <seealso href="https://oeis.org/A000045"/>
    /// <seealso cref="Lucas(int)"/>
    /// <seealso cref="FibonacciFast(int)"/>
    /// <seealso cref="FibonacciBinet(int)"/>
    public static BigInteger Fibonacci(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n == 0) return BigInteger.Zero;
        if (n == 1) return BigInteger.One;

        BigInteger a = BigInteger.Zero;
        BigInteger b = BigInteger.One;
        for (int i = 2; i <= n; i++)
        {
            BigInteger c = a + b;
            a = b;
            b = c;
        }
        return b;
    }

    /// <summary>
    /// Возвращает пару <c>(F(n), F(n+1))</c> через быстрое удвоение за O(log n).
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Кортеж из двух последовательных чисел Фибоначчи.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// Использует тождества:
    /// <list type="bullet">
    ///   <item><description><c>F(2k) = F(k)·(2·F(k+1) − F(k))</c></description></item>
    ///   <item><description><c>F(2k+1) = F(k)² + F(k+1)²</c></description></item>
    /// </list>
    /// </remarks>
    public static (BigInteger Fn, BigInteger Fn1) FibonacciPair(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n == 0) return (BigInteger.Zero, BigInteger.One);

        var (a, b) = FibonacciPair(n >> 1);
        BigInteger c = a * (2 * b - a);
        BigInteger d = a * a + b * b;

        return (n & 1) == 0
            ? (c, d)
            : (d, c + d);
    }

    /// <summary>
    /// n-е число Фибоначчи через быстрое удвоение за O(log n).
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение F(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// Предпочтительный метод для больших n (&gt; 100). Например, <c>FibonacciFast(1000)</c>
    /// вычисляется мгновенно, тогда как <see cref="Fibonacci(int)"/> потребует 1000 итераций.
    /// </remarks>
    /// <example>
    /// <code>
    /// var f1000 = FibonacciFast(1000); // 4.35e208 — огромное число
    /// </code>
    /// </example>
    public static BigInteger FibonacciFast(int n) => FibonacciPair(n).Fn;

    /// <summary>
    /// Первые <paramref name="count"/> чисел Фибоначчи.
    /// </summary>
    /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
    /// <returns>Массив длины <paramref name="count"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
    public static BigInteger[] FibonacciSequence(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new BigInteger[count];
        if (count == 0) return result;
        result[0] = BigInteger.Zero;
        if (count == 1) return result;
        result[1] = BigInteger.One;

        for (int i = 2; i < count; i++)
            result[i] = result[i - 1] + result[i - 2];
        return result;
    }

    /// <summary>
    /// n-е число Люка: <c>L(0) = 2, L(1) = 1, L(n) = L(n−1) + L(n−2)</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение L(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// <para>«Компаньон» чисел Фибоначчи.</para>
    /// <para>Формула Бине: <c>L(n) = φ^n + ψ^n</c> — см. <see cref="LucasBinet(int)"/>.</para>
    /// <para>Связи: <c>L(n) = F(n−1) + F(n+1)</c>, <c>F(2n) = F(n)·L(n)</c>.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var l10 = Lucas(10); // 123
    /// </code>
    /// </example>
    /// <seealso href="https://oeis.org/A000032"/>
    /// <seealso cref="Fibonacci(int)"/>
    public static BigInteger Lucas(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n == 0) return new BigInteger(2);
        if (n == 1) return BigInteger.One;

        BigInteger a = new BigInteger(2);
        BigInteger b = BigInteger.One;
        for (int i = 2; i <= n; i++)
        {
            BigInteger c = a + b;
            a = b;
            b = c;
        }
        return b;
    }

    /// <summary>
    /// Первые <paramref name="count"/> чисел Люка.
    /// </summary>
    /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
    /// <returns>Массив длины <paramref name="count"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
    public static BigInteger[] LucasSequence(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new BigInteger[count];
        if (count == 0) return result;
        result[0] = new BigInteger(2);
        if (count == 1) return result;
        result[1] = BigInteger.One;

        for (int i = 2; i < count; i++)
            result[i] = result[i - 1] + result[i - 2];
        return result;
    }

    /// <summary>
    /// Проверяет тождество Кассини: <c>F(n+1)² − F(n)·F(n+2) = (−1)^n</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns><c>true</c>, если тождество выполняется.</returns>
    public static bool CassiniIdentity(int n)
    {
        BigInteger f0 = Fibonacci(n);
        BigInteger f1 = Fibonacci(n + 1);
        BigInteger f2 = Fibonacci(n + 2);
        BigInteger lhs = f1 * f1 - f0 * f2;
        BigInteger rhs = (n % 2 == 0) ? BigInteger.One : -BigInteger.One;
        return lhs == rhs;
    }

    /// <summary>
    /// Проверяет тождество Люка: <c>L(n)² − 5·F(n)² = 4·(−1)^n</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns><c>true</c>, если тождество выполняется.</returns>
    public static bool LucasIdentity(int n)
    {
        BigInteger l = Lucas(n);
        BigInteger f = Fibonacci(n);
        BigInteger lhs = l * l - 5 * f * f;
        BigInteger rhs = (n % 2 == 0) ? new BigInteger(4) : new BigInteger(-4);
        return lhs == rhs;
    }

    /// <summary>
    /// Проверяет формулу удвоения: <c>F(2n) = F(n)·L(n)</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns><c>true</c>, если тождество выполняется.</returns>
    public static bool DoublingFormula(int n)
        => Fibonacci(2 * n) == Fibonacci(n) * Lucas(n);

    /// <summary>
    /// Проверяет связь Люка с Фибоначчи: <c>L(n) = F(n−1) + F(n+1)</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое, n ≥ 1.</param>
    /// <returns><c>true</c>, если тождество выполняется.</returns>
    public static bool LucasFromFibonacci(int n)
        => n >= 1 && Lucas(n) == Fibonacci(n - 1) + Fibonacci(n + 1);

    /// <summary>
    /// Обобщённое число Моцкина <c>M_n^(m)</c>: пути с шагами ↑, ↓ и m цветами →.
    /// </summary>
    /// <param name="n">Длина пути, n ≥ 0.</param>
    /// <param name="m">Число цветов горизонтального шага, m ≥ 0.</param>
    /// <returns>Значение M_n^(m).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если n &lt; 0 или m &lt; 0.</exception>
    /// <remarks>
    /// <para>Рекуррентность: <c>M(0) = 1, M(n+1) = m·M(n) + Σ_{i=0}^{n-1} M(i)·M(n−1−i)</c>.</para>
    /// <para>Закрытая формула: <c>M_n^(m) = Σ_{k=0}^{⌊n/2⌋} C(n, 2k) · C_k · m^{n−2k}</c> —
    /// см. <see cref="MotzkinGeneralizedBySum(int, int)"/>.</para>
    /// <para>При m = 1 — обычные числа Моцкина, при m = 2 — <c>C_{n+1}</c>.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var m3 = MotzkinGeneralized(5, 3); // 543
    /// </code>
    /// </example>
    /// <seealso cref="Motzkin(int)"/>
    /// <seealso cref="MotzkinTwoColored(int)"/>
    /// <seealso cref="MotzkinGeneralizedBySum(int, int)"/>
    public static BigInteger MotzkinGeneralized(int n, int m)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (m < 0) throw new ArgumentOutOfRangeException(nameof(m));
        if (n == 0) return BigInteger.One;

        var M = new BigInteger[n + 1];
        M[0] = BigInteger.One;

        for (int k = 1; k <= n; k++)
        {
            BigInteger s = m * M[k - 1];
            for (int i = 0; i < k - 1; i++)
                s += M[i] * M[k - 2 - i];
            M[k] = s;
        }
        return M[n];
    }

    /// <summary>
    /// Первые <paramref name="count"/> обобщённых чисел Моцкина порядка m.
    /// </summary>
    /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
    /// <param name="m">Число цветов, m ≥ 0.</param>
    /// <returns>Массив длины <paramref name="count"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если count &lt; 0 или m &lt; 0.</exception>
    public static BigInteger[] MotzkinGeneralizedSequence(int count, int m)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (m < 0) throw new ArgumentOutOfRangeException(nameof(m));

        var M = new BigInteger[count];
        if (count == 0) return M;
        M[0] = BigInteger.One;

        for (int k = 1; k < count; k++)
        {
            BigInteger s = m * M[k - 1];
            for (int i = 0; i < k - 1; i++)
                s += M[i] * M[k - 2 - i];
            M[k] = s;
        }
        return M;
    }

    /// <summary>
    /// Обобщённое число Моцкина через закрытую сумму:
    /// <c>M_n^(m) = Σ_{k=0}^{⌊n/2⌋} C(n, 2k) · C_k · m^{n−2k}</c>.
    /// </summary>
    /// <param name="n">Длина пути, n ≥ 0.</param>
    /// <param name="m">Число цветов, m ≥ 0.</param>
    /// <returns>Значение M_n^(m).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если n &lt; 0 или m &lt; 0.</exception>
    /// <remarks>Используется в тестах для перекрёстной проверки рекуррентной реализации.</remarks>
    public static BigInteger MotzkinGeneralizedBySum(int n, int m)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (m < 0) throw new ArgumentOutOfRangeException(nameof(m));

        BigInteger sum = BigInteger.Zero;
        for (int k = 0; k <= n / 2; k++)
        {
            int pow = n - 2 * k;
            BigInteger term = Combinations(n, 2 * k) * Catalan(k) * BigInteger.Pow(m, pow);
            sum += term;
        }
        return sum;
    }

    /// <summary>Обычные числа Моцкина (m = 1). Синоним <see cref="Motzkin(int)"/>.</summary>
    /// <param name="n">Длина пути, n ≥ 0.</param>
    /// <returns>M_n^(1).</returns>
    public static BigInteger MotzkinGeneralized1(int n) => MotzkinGeneralized(n, 1);

    /// <summary>Two-colored числа Моцкина (m = 2). Синоним <see cref="MotzkinTwoColored(int)"/>.</summary>
    /// <param name="n">Длина пути, n ≥ 0.</param>
    /// <returns>M_n^(2).</returns>
    public static BigInteger MotzkinGeneralized2(int n) => MotzkinGeneralized(n, 2);

    /// <summary>Three-colored числа Моцкина (m = 3, OEIS A002212).</summary>
    /// <param name="n">Длина пути, n ≥ 0.</param>
    /// <returns>M_n^(3).</returns>
    public static BigInteger MotzkinGeneralized3(int n) => MotzkinGeneralized(n, 3);
}