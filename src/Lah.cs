using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// Число Лаха <c>L(n, k)</c> — число разбиений n элементов на k непустых
        /// линейно упорядоченных подмножеств (списков).
        /// </summary>
        /// <param name="n">Число элементов, n ≥ 0.</param>
        /// <param name="k">Число списков, 0 ≤ k ≤ n.</param>
        /// <returns>Значение L(n, k).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// Формула: <c>L(n, k) = C(n−1, k−1) · n! / k!</c>.
        /// Связь со Стирлингом: <c>L(n, k) = Σ_j |s(n, j)| · S(j, k)</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var l = Lah(5, 3); // 120
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A105278"/>
        /// <seealso cref="LahRow(int)"/>
        /// <seealso cref="OrderedBell(int)"/>
        public static BigInteger Lah(int n, int k)
        {
            if (n < 0 || k < 0 || k > n) return BigInteger.Zero;
            if (n == 0) return k == 0 ? BigInteger.One : BigInteger.Zero;
            if (k == 0) return BigInteger.Zero;
            return Combinations(n - 1, k - 1) * Factorial(n) / Factorial(k);
        }

        /// <summary>
        /// n-я строка чисел Лаха для k = 0..n.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 0.</param>
        /// <returns>Массив длины n+1; <c>result[k] = L(n, k)</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <example>
        /// <code>
        /// var row5 = LahRow(5);
        /// // 0, 120, 240, 120, 20, 1
        /// </code>
        /// </example>
        public static BigInteger[] LahRow(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            var row = new BigInteger[n + 1];
            if (n == 0) { row[0] = BigInteger.One; return row; }
            for (int k = 1; k <= n; k++) row[k] = Lah(n, k);
            return row;
        }

        /// <summary>
        /// n-е упорядоченное число Белла (Fubini) — число всех упорядоченных разбиений
        /// множества из n элементов.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Сумма <c>Σ_k L(n, k)</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Также называется «упорядоченные числа Белла». Отличается от обычного
        /// числа Белла тем, что блоки разбиения различимы (упорядочены).
        /// Асимптотика: <c>n! / (2 · (ln 2)^{n+1})</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var f6 = OrderedBell(6); // 4683
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A000670"/>
        public static BigInteger OrderedBell(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            BigInteger sum = BigInteger.Zero;
            for (int k = 0; k <= n; k++)
                sum += Factorial(k) * StirlingSecondKind(n, k);
            return sum;
        }
    }
}