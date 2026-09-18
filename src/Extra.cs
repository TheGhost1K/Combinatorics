using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// Вычисляет число Деланнуа <c>D(m, n)</c> — число путей на решётке
        /// из (0,0) в (m,n) с шагами (1,0), (0,1), (1,1).
        /// </summary>
        /// <param name="m">Горизонтальная координата, m ≥ 0.</param>
        /// <param name="n">Вертикальная координата, n ≥ 0.</param>
        /// <returns>Значение <c>D(m, n)</c>.</returns>
        /// <exception cref="ArgumentException">Если <paramref name="m"/> &lt; 0 или <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>
        /// Автоматически выбирает алгоритм в зависимости от размера задачи:
        /// </para>
        /// <list type="bullet">
        ///   <item><description>
        ///     При <c>m · n ≤ 2500</c> — закрытая формула <see cref="DelannoyClosed(int, int)"/> (O(min(m,n)²)).
        ///   </description></item>
        ///   <item><description>
        ///     При <c>m · n &gt; 2500</c> — рекуррентность <see cref="DelannoyByRecurrence(int, int)"/> (O(m·n), без вызовов <see cref="Combinations(int, int)"/>).
        ///   </description></item>
        /// </list>
        /// <para>
        /// Порог 2500 (≈ 50×50) подобран эмпирически: при меньших размерах
        /// накладные расходы рекуррентности превышают выигрыш; при больших —
        /// рекуррентность заметно быстрее.
        /// </para>
        /// <para>
        /// Обладает симметрией: <c>D(m, n) = D(n, m)</c>.
        /// </para>
        /// <para>
        /// Рекуррентность: <c>D(m,n) = D(m−1,n) + D(m,n−1) + D(m−1,n−1)</c>, D(0,n) = D(m,0) = 1.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var d1 = Delannoy(3, 3);       // 63
        /// var d2 = Delannoy(10, 10);     // 8097453
        /// var d3 = Delannoy(100, 100);   // вычисляется через рекуррентность — быстро
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A008288"/>
        /// <seealso cref="DelannoyClosed(int, int)"/>
        /// <seealso cref="DelannoyByRecurrence(int, int)"/>
        /// <seealso cref="DelannoyCentral(int)"/>
        /// <seealso cref="DelannoyCentralSequence(int)"/>
        public static BigInteger Delannoy(int m, int n)
        {
            if (m < 0 || n < 0) throw new ArgumentException("m, n ≥ 0");
            return (m * n > 2500)
                ? DelannoyByRecurrence(m, n)
                : DelannoyClosed(m, n);
        }

        /// <summary>
        /// Вычисляет число Деланнуа <c>D(m, n)</c> по закрытой формуле через биномиальные коэффициенты.
        /// </summary>
        /// <param name="m">Горизонтальная координата, m ≥ 0.</param>
        /// <param name="n">Вертикальная координата, n ≥ 0.</param>
        /// <returns>Значение <c>D(m, n)</c> — число путей из (0,0) в (m,n) с шагами (1,0), (0,1), (1,1).</returns>
        /// <remarks>
        /// <para>
        /// Использует формулу:
        /// <c>D(m, n) = Σ_{k=0}^{min(m,n)} C(m, k) · C(n, k) · 2^k</c>.
        /// </para>
        /// <para>
        /// Оптимизации:
        /// </para>
        /// <list type="bullet">
        ///   <item><description>
        ///     Верхний предел суммы берётся как <c>min(m, n)</c>, чтобы число слагаемых было минимальным.
        ///   </description></item>
        ///   <item><description>
        ///     Степень <c>2^k</c> накапливается умножением на 2, а не через <see cref="BigInteger.Pow"/>.
        ///   </description></item>
        /// </list>
        /// <para>
        /// Сложность: O(min(m, n)²) — каждое <c>C(m, k)</c> вычисляется за O(k).
        /// Для больших m и n предпочтительнее <see cref="DelannoyByRecurrence(int, int)"/> (O(m·n) без умножений).
        /// </para>
        /// <para>
        /// Метод вызывается из <see cref="Delannoy(int, int)"/> для малых значений m·n ≤ 2500,
        /// где закрытая формула быстрее рекуррентности.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var d1 = DelannoyClosed(3, 3);   // 63
        /// var d2 = DelannoyClosed(5, 5);   // 1683
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A008288"/>
        /// <seealso cref="Delannoy(int, int)"/>
        /// <seealso cref="DelannoyByRecurrence(int, int)"/>
        /// <seealso cref="DelannoyCentral(int)"/>
        private static BigInteger DelannoyClosed(int m, int n)
        {
            int kMax = Math.Min(m, n);
            BigInteger sum = BigInteger.Zero;
            BigInteger twoPowK = BigInteger.One;
            for (int k = 0; k <= kMax; k++)
            {
                sum += Combinations(m, k) * Combinations(n, k) * twoPowK;
                twoPowK *= 2;
            }
            return sum;
        }

        /// <summary>
        /// Центральные числа Деланнуа <c>D(n, n)</c> — число путей из (0,0) в (n,n).
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение D(n, n).</returns>
        /// <example>
        /// <code>
        /// var d5 = DelannoyCentral(5); // 1683
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001850"/>
        public static BigInteger DelannoyCentral(int n) => Delannoy(n, n);

        /// <summary>
        /// Первые <paramref name="count"/> центральных чисел Деланнуа.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] DelannoyCentralSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            for (int i = 0; i < count; i++) result[i] = Delannoy(i, i);
            return result;
        }

        /// <summary>
        /// Число Деланнуа через рекуррентность (для проверки формулы).
        /// </summary>
        /// <param name="m">Горизонтальная координата, m ≥ 0.</param>
        /// <param name="n">Вертикальная координата, n ≥ 0.</param>
        /// <returns>Значение D(m, n).</returns>
        /// <exception cref="ArgumentException">Если m &lt; 0 или n &lt; 0.</exception>
        /// <remarks>
        /// Использует <c>D(m,n) = D(m−1,n) + D(m,n−1) + D(m−1,n−1)</c> за O(m·n).
        /// Работает быстрее закрытой формулы для больших m и n, так как не вызывает
        /// <see cref="Combinations(int, int)"/> на каждом шаге.
        /// </remarks>
        public static BigInteger DelannoyByRecurrence(int m, int n)
        {
            if (m < 0 || n < 0) throw new ArgumentException("m, n ≥ 0");

            var prev = new BigInteger[n + 1];
            for (int j = 0; j <= n; j++) prev[j] = 1;

            for (int i = 1; i <= m; i++)
            {
                var cur = new BigInteger[n + 1];
                cur[0] = 1;
                for (int j = 1; j <= n; j++)
                    cur[j] = cur[j - 1] + prev[j] + prev[j - 1];
                prev = cur;
            }
            return prev[n];
        }

        /// <summary>
        /// Большие числа Шрёдера <c>S_n</c> — число путей из (0,0) в (n,n)
        /// с шагами (1,0), (0,1), (1,1), не поднимающихся выше диагонали y = x.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение S_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Формула: <c>S_n = Σ_{k=0}^{n} C(n+k, n−k) · C_k</c>, где C_k — числа Каталана.
        /// Рекуррентность: <c>(n+2)·S_{n+1} = 3(2n+1)·S_n − (n−1)·S_{n−1}</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var s6 = SchroederLarge(6); // 1806
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A006318"/>
        /// <seealso cref="SchroederSmall(int)"/>
        public static BigInteger SchroederLarge(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            BigInteger sum = BigInteger.Zero;
            for (int k = 0; k <= n; k++)
                sum += Combinations(n + k, n - k) * Catalan(k);
            return sum;
        }

        /// <summary>
        /// Первые <paramref name="count"/> больших чисел Шрёдера.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] SchroederLargeSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.One;
            if (count == 1) return result;
            result[1] = new BigInteger(2);

            // (n+2)·S_{n+1} = 3(2n+1)·S_n − (n−1)·S_{n−1}
            for (int n = 1; n < count - 1; n++)
                result[n + 1] = (3 * (2 * n + 1) * result[n] - (n - 1) * result[n - 1]) / (n + 2);
            return result;
        }

        /// <summary>
        /// Малые числа Шрёдера <c>s_n</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение s_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Рекуррентность: <c>(n+1)·s_{n+1} = 3(2n−1)·s_n − (n−2)·s_{n−1}</c>, s_0 = s_1 = 1.</para>
        /// <para>Связь с большими: <c>s_n = S_n / 2</c> для n ≥ 1; s_0 = S_0 = 1.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var s6 = SchroederSmall(6); // 903
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001003"/>
        public static BigInteger SchroederSmall(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n <= 1) return BigInteger.One;

            BigInteger s0 = BigInteger.One; // s_0
            BigInteger s1 = BigInteger.One; // s_1

            for (int m = 2; m <= n; m++)
            {
                // (m+1)·s_m = 3(2m−1)·s_{m−1} − (m−2)·s_{m−2}
                BigInteger sm = (3 * (2 * m - 1) * s1 - (m - 2) * s0) / (m + 1);
                s0 = s1;
                s1 = sm;
            }
            return s1;
        }

        /// <summary>
        /// Первые <paramref name="count"/> малых чисел Шрёдера.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] SchroederSmallSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.One;
            if (count == 1) return result;
            result[1] = BigInteger.One;

            for (int m = 2; m < count; m++)
            {
                // (m+1)·s_m = 3(2m−1)·s_{m−1} − (m−2)·s_{m−2}
                result[m] = (3 * (2 * m - 1) * result[m - 1] - (m - 2) * result[m - 2]) / (m + 1);
            }
            return result;
        }

        /// <summary>
        /// Число Фусса–Каталана <c>A_n^(m) = C((m+1)·n, n) / (m·n + 1)</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <param name="m">Порядок, m ≥ 1.</param>
        /// <returns>Значение A_n^(m).</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или m &lt; 1.</exception>
        /// <remarks>
        /// <para>Обобщает числа Каталана на (m+1)-арные деревья.</para>
        /// <para>При m = 1 получаем <see cref="Catalan(int)"/>.</para>
        /// <para>При m = 2 — числа тернарных деревьев (OEIS A001764).</para>
        /// <para>При m = 3 — числа четвертичных деревьев (OEIS A002293).</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var t = FussCatalan(5, 2); // 273 — тернарные деревья
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001764"/>
        /// <seealso cref="Catalan(int)"/>
        /// <seealso cref="FussCatalanSequence(int, int)"/>
        public static BigInteger FussCatalan(int n, int m)
        {
            if (n < 0 || m < 1) throw new ArgumentException("n ≥ 0, m ≥ 1");
            return Combinations((m + 1) * n, n) / (m * n + 1);
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Фусса–Каталана порядка m.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <param name="m">Порядок, m ≥ 1.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        /// <exception cref="ArgumentException">Если m &lt; 1.</exception>
        public static BigInteger[] FussCatalanSequence(int count, int m)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            if (m < 1) throw new ArgumentException("m ≥ 1");

            var result = new BigInteger[count];
            for (int i = 0; i < count; i++) result[i] = FussCatalan(i, m);
            return result;
        }
    }
}