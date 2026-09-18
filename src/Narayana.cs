using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// Число Нараяны <c>N(n, k) = (1/n) · C(n, k) · C(n, k−1)</c>.
        /// </summary>
        /// <param name="n">Число пар скобок, n ≥ 1.</param>
        /// <param name="k">Число «внешних» пар, 1 ≤ k ≤ n.</param>
        /// <returns>Значение N(n, k).</returns>
        /// <remarks>
        /// <para>
        /// Считает правильные скобочные последовательности из n пар ровно с k «верхними»
        /// (внешними) парами. Эквивалентно — пути Дика длины 2n ровно с k пиками.
        /// </para>
        /// <para>Сумма по k даёт число Каталана: <c>Σ_k N(n, k) = C_n</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var n = Narayana(4, 2); // 6
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001263"/>
        /// <seealso cref="NarayanaRow(int)"/>
        /// <seealso cref="Catalan(int)"/>
        public static BigInteger Narayana(int n, int k)
        {
            if (n < 1 || k < 1 || k > n) return BigInteger.Zero;
            return Combinations(n, k) * Combinations(n, k - 1) / n;
        }

        /// <summary>
        /// n-я строка чисел Нараяны для k = 1..n. Индекс 0 не используется.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 1.</param>
        /// <returns>Массив длины n+1; <c>result[k] = N(n, k)</c>; <c>result[0] = 0</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 1.</exception>
        /// <example>
        /// <code>
        /// var row6 = NarayanaRow(6);
        /// // 0, 1, 15, 50, 50, 15, 1
        /// </code>
        /// </example>
        public static BigInteger[] NarayanaRow(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
            var row = new BigInteger[n + 1];
            var pascal = PascalRow(n);
            for (int k = 1; k <= n; k++)
                row[k] = pascal[k] * pascal[k - 1] / n;
            return row;
        }
    }
}