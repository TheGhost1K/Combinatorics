using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// n-е число Белла — количество всех разбиений множества из n элементов.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Число Белла B_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Вычисляется через треугольник Белла (Aitken's array) за O(n²).</para>
        /// <para>Связь с числами Стирлинга 2-го рода: <c>B_n = Σ_k S(n, k)</c>.</para>
        /// <para>Рекуррентность: <c>B_{n+1} = Σ_{k=0}^{n} C(n, k) · B_k</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var b10 = Bell(10); // 115975
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A000110"/>
        /// <seealso cref="BellSequence(int)"/>
        /// <seealso cref="BellApprox(int)"/>
        /// <seealso cref="StirlingSecondKind(int, int)"/>
        public static BigInteger Bell(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0) return BigInteger.One;

            var row = new BigInteger[n + 1];
            row[0] = BigInteger.One;
            for (int i = 1; i <= n; i++)
            {
                var next = new BigInteger[n + 1];
                next[0] = row[i - 1];
                for (int j = 1; j <= i; j++) next[j] = next[j - 1] + row[j - 1];
                row = next;
            }
            return row[0];
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Белла: B(0), B(1), …, B(count−1).
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        /// <remarks>
        /// Эффективнее, чем <paramref name="count"/> отдельных вызовов <see cref="Bell(int)"/>:
        /// треугольник строится один раз.
        /// </remarks>
        /// <example>
        /// <code>
        /// var seq = BellSequence(11);
        /// // 1, 1, 2, 5, 15, 52, 203, 877, 4140, 21147, 115975
        /// </code>
        /// </example>
        public static BigInteger[] BellSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;

            var row = new BigInteger[count];
            row[0] = BigInteger.One;
            result[0] = BigInteger.One;

            for (int i = 1; i < count; i++)
            {
                var next = new BigInteger[count];
                next[0] = row[i - 1];
                for (int j = 1; j <= i; j++) next[j] = next[j - 1] + row[j - 1];
                row = next;
                result[i] = row[0];
            }
            return result;
        }

        /// <summary>
        /// Число Эйлера <c>A(n, k)</c> — число перестановок n элементов ровно с k подъёмами.
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 0.</param>
        /// <param name="k">Число подъёмов, 0 ≤ k &lt; n.</param>
        /// <returns>Значение A(n, k).</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// <para>Подъём — позиция i, где <c>π_i &lt; π_{i+1}</c>.</para>
        /// <para>Формула: <c>A(n, k) = Σ_{j=0}^{k+1} (−1)^j · C(n+1, j) · (k+1−j)^n</c>.</para>
        /// <para>Симметрия: <c>A(n, k) = A(n, n−1−k)</c>.</para>
        /// <para>Сумма по k даёт <c>n!</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var a = Eulerian(5, 2); // 66
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A008292"/>
        /// <seealso cref="EulerianRow(int)"/>
        /// <seealso cref="EulerianDescents(int, int)"/>
        public static BigInteger Eulerian(int n, int k)
        {
            if (n < 0 || k < 0) throw new ArgumentException("n, k ≥ 0");
            if (n == 0) return k == 0 ? BigInteger.One : BigInteger.Zero;
            if (k >= n) return BigInteger.Zero;

            BigInteger sum = BigInteger.Zero;
            for (int j = 0; j <= k + 1; j++)
            {
                BigInteger term = Combinations(n + 1, j) * BigInteger.Pow(k + 1 - j, n);
                sum += (j % 2 == 0) ? term : -term;
            }
            return sum;
        }

        /// <summary>
        /// n-я строка чисел Эйлера для k = 0..n−1.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 0.</param>
        /// <returns>Массив длины n; <c>result[k] = A(n, k)</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Использует рекуррентность <c>A(n, k) = (k+1)·A(n−1, k) + (n−k)·A(n−1, k−1)</c>
        /// за O(n²). Быстрее, чем n отдельных вызовов <see cref="Eulerian(int, int)"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var row5 = EulerianRow(5);
        /// // 1, 26, 66, 26, 1
        /// </code>
        /// </example>
        public static BigInteger[] EulerianRow(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0) return [BigInteger.One];

            var prev = new BigInteger[n];
            prev[0] = BigInteger.One;
            for (int i = 2; i <= n; i++)
            {
                var cur = new BigInteger[n];
                for (int k = 0; k < i; k++)
                {
                    BigInteger a = (k < i - 1) ? prev[k] : BigInteger.Zero;
                    BigInteger b = (k > 0) ? prev[k - 1] : BigInteger.Zero;
                    cur[k] = (k + 1) * a + (i - k) * b;
                }
                prev = cur;
            }
            return prev;
        }

        /// <summary>
        /// Число перестановок n элементов ровно с k спусками.
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 1.</param>
        /// <param name="k">Число спусков, 0 ≤ k &lt; n.</param>
        /// <returns>Значение A(n, n−1−k) — из симметрии подъёмов и спусков.</returns>
        /// <remarks>
        /// В силу симметрии <c>A(n, k) = A(n, n−1−k)</c> число спусков совпадает
        /// с числом подъёмов для зеркальной перестановки.
        /// </remarks>
        /// <example>
        /// <code>
        /// var d = EulerianDescents(5, 2); // 26 = Eulerian(5, 2)
        /// </code>
        /// </example>
        public static BigInteger EulerianDescents(int n, int k)
        {
            if (n <= 0 || k < 0 || k >= n) return BigInteger.Zero;
            return Eulerian(n, n - 1 - k);
        }
    }
}