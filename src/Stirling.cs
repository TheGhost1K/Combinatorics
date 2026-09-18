using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// Беззнаковое число Стирлинга первого рода: число перестановок n элементов
        /// ровно с k циклами.
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 0.</param>
        /// <param name="k">Число циклов, k ≥ 0.</param>
        /// <returns>Значение <c>c(n, k)</c>.</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// <para>Рекуррентность: <c>c(n, k) = c(n−1, k−1) + (n−1)·c(n−1, k)</c>.</para>
        /// <para>Сумма по k даёт <c>n!</c>: <c>Σ_k c(n, k) = n!</c>.</para>
        /// <para>Сложность O(n·k), память O(k).</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var c = StirlingFirstKind(5, 2); // 50
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A130534"/>
        /// <seealso cref="SignedStirlingFirstKind(int, int)"/>
        /// <seealso cref="StirlingSecondKind(int, int)"/>
        public static BigInteger StirlingFirstKind(int n, int k)
        {
            if (n < 0 || k < 0) throw new ArgumentException("n, k ≥ 0");
            if (n == 0) return k == 0 ? BigInteger.One : BigInteger.Zero;
            if (k == 0 || k > n) return BigInteger.Zero;

            var prev = new BigInteger[k + 1];
            prev[0] = BigInteger.One;
            var cur = new BigInteger[k + 1];

            for (int i = 1; i <= n; i++)
            {
                Array.Clear(cur, 0, cur.Length);
                for (int j = 1; j <= Math.Min(i, k); j++)
                    cur[j] = prev[j - 1] + (i - 1) * prev[j];
                (prev, cur) = (cur, prev);
            }
            return prev[k];
        }

        /// <summary>
        /// Знаковое число Стирлинга первого рода: <c>s(n, k) = (−1)^(n−k) · |c(n, k)|</c>.
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 0.</param>
        /// <param name="k">Степень, 0 ≤ k ≤ n.</param>
        /// <returns>Коэффициент при <c>x^k</c> в падающем факториале <c>x·(x−1)·…·(x−n+1)</c>.</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// Используется в разложении падающего факториала:
        /// <c>x^(n̲) = Σ_k s(n, k) · x^k</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// // x(x−1)(x−2) = x³ − 3x² + 2x → s(3,3)=1, s(3,2)=−3, s(3,1)=2
        /// var s1 = SignedStirlingFirstKind(3, 3); // 1
        /// var s2 = SignedStirlingFirstKind(3, 2); // -3
        /// var s3 = SignedStirlingFirstKind(3, 1); // 2
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A048994"/>
        public static BigInteger SignedStirlingFirstKind(int n, int k)
        {
            var abs = StirlingFirstKind(n, k);
            return ((n - k) % 2 == 0) ? abs : -abs;
        }

        /// <summary>
        /// n-я строка беззнаковых чисел Стирлинга первого рода для k = 0..n.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 0.</param>
        /// <returns>Массив длины n+1; <c>result[k] = c(n, k)</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>Сложность O(n²), память O(n).</remarks>
        /// <example>
        /// <code>
        /// var row5 = StirlingFirstKindRow(5);
        /// // 0, 24, 50, 35, 10, 1
        /// </code>
        /// </example>
        /// <seealso cref="StirlingFirstKind(int, int)"/>
        public static BigInteger[] StirlingFirstKindRow(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            var result = new BigInteger[n + 1];
            if (n == 0) { result[0] = BigInteger.One; return result; }

            var prev = new BigInteger[n + 1];
            prev[0] = BigInteger.One;
            var cur = new BigInteger[n + 1];

            for (int i = 1; i <= n; i++)
            {
                Array.Clear(cur, 0, cur.Length);
                for (int j = 1; j <= i; j++)
                    cur[j] = prev[j - 1] + (i - 1) * prev[j];
                (prev, cur) = (cur, prev);
            }
            Array.Copy(prev, result, n + 1);
            return result;
        }

        /// <summary>
        /// Число Стирлинга второго рода: число разбиений множества из n элементов
        /// ровно на k непустых подмножеств.
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 0.</param>
        /// <param name="k">Число подмножеств, k ≥ 0.</param>
        /// <returns>Значение <c>S(n, k)</c>.</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// <para>Рекуррентность: <c>S(n, k) = S(n−1, k−1) + k·S(n−1, k)</c>.</para>
        /// <para>Сумма по k даёт число Белла: <c>B_n = Σ_k S(n, k)</c>.</para>
        /// <para>Сложность O(n·k), память O(k).</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var s = StirlingSecondKind(5, 2); // 15
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A008277"/>
        /// <seealso cref="Bell(int)"/>
        /// <seealso cref="StirlingFirstKind(int, int)"/>
        public static BigInteger StirlingSecondKind(int n, int k)
        {
            if (n < 0 || k < 0) throw new ArgumentException("n, k ≥ 0");
            if (k == 0) return n == 0 ? BigInteger.One : BigInteger.Zero;
            if (k > n) return BigInteger.Zero;

            var prev = new BigInteger[k + 1];
            prev[0] = BigInteger.One;
            var cur = new BigInteger[k + 1];

            for (int i = 1; i <= n; i++)
            {
                Array.Clear(cur, 0, cur.Length);
                for (int j = 1; j <= Math.Min(i, k); j++)
                    cur[j] = prev[j - 1] + j * prev[j];
                (prev, cur) = (cur, prev);
            }
            return prev[k];
        }

        /// <summary>
        /// n-я строка чисел Стирлинга второго рода для k = 0..n.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 0.</param>
        /// <returns>Массив длины n+1; <c>result[k] = S(n, k)</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>Сложность O(n²), память O(n).</remarks>
        /// <example>
        /// <code>
        /// var row5 = StirlingSecondKindRow(5);
        /// // 0, 1, 15, 25, 10, 1
        /// </code>
        /// </example>
        public static BigInteger[] StirlingSecondKindRow(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            var result = new BigInteger[n + 1];
            if (n == 0) { result[0] = BigInteger.One; return result; }

            var prev = new BigInteger[n + 1];
            prev[0] = BigInteger.One;
            var cur = new BigInteger[n + 1];

            for (int i = 1; i <= n; i++)
            {
                Array.Clear(cur, 0, cur.Length);
                for (int j = 1; j <= i; j++)
                    cur[j] = prev[j - 1] + j * prev[j];
                (prev, cur) = (cur, prev);
            }
            Array.Copy(prev, result, n + 1);
            return result;
        }
    }
}