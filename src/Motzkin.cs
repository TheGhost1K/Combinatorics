using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// n-е число Моцкина — число путей на решётке из (0,0) в (n,0),
        /// где шаги: (1,0), (1,1), (1,−1), и путь не опускается ниже оси.
        /// </summary>
        /// <param name="n">Длина пути, n ≥ 0.</param>
        /// <returns>Число Моцкина M_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Формула: <c>M_n = Σ_{k=0}^{⌊n/2⌋} C(n, 2k) · C_k</c>, где C_k — числа Каталана.
        /// Асимптотика: <c>M_n ~ 3^{n+3/2} / (2·√π·n^{3/2})</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var m10 = Motzkin(10); // 2188
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001006"/>
        /// <seealso cref="MotzkinSequence(int)"/>
        /// <seealso cref="MotzkinGeneralized(int, int)"/>
        public static BigInteger Motzkin(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            BigInteger sum = BigInteger.Zero;
            for (int k = 0; k <= n / 2; k++)
                sum += Combinations(n, 2 * k) * Catalan(k);
            return sum;
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Моцкина.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        /// <remarks>
        /// Использует рекуррентность <c>M_{n+1} = M_n + Σ_{i=0}^{n-1} M_i · M_{n-1-i}</c>
        /// за O(n²), что быстрее n отдельных вызовов <see cref="Motzkin(int)"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var seq = MotzkinSequence(11);
        /// // 1, 1, 2, 4, 9, 21, 51, 127, 323, 835, 2188
        /// </code>
        /// </example>
        public static BigInteger[] MotzkinSequence(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            var m = new BigInteger[count];
            if (count == 0) return m;
            m[0] = BigInteger.One;
            if (count == 1) return m;

            for (int n = 1; n < count; n++)
            {
                BigInteger s = m[n - 1];
                for (int i = 0; i < n - 1; i++) s += m[i] * m[n - 2 - i];
                m[n] = s;
            }
            return m;
        }
    }
}