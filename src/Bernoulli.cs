namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// n-е число Бернулли <c>B_n</c> (в соглашении <c>B_1 = −1/2</c>).
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Число Бернулли как <see cref="BigRational"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>
        /// Определяются через производящую функцию: <c>x / (e^x − 1) = Σ B_n · x^n / n!</c>.
        /// </para>
        /// <para>
        /// Вычисляются по рекуррентности <c>Σ_{k=0}^{n} C(n+1, k) · B_k = 0</c> при n ≥ 1,
        /// <c>B_0 = 1</c>.
        /// </para>
        /// <para>
        /// Все <c>B_{2k+1} = 0</c> при k ≥ 1. Знаки чередуются: B_2 = 1/6, B_4 = −1/30, B_6 = 1/42, …
        /// </para>
        /// <para>
        /// Применяются в формуле Фаульхабера (суммы степеней) и в выражении дзета-функции
        /// Римана при чётных аргументах.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var b0 = Bernoulli(0); // 1
        /// var b1 = Bernoulli(1); // -1/2
        /// var b6 = Bernoulli(6); // 1/42
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A027641"/>
        /// <seealso href="https://oeis.org/A027642"/>
        /// <seealso cref="BernoulliSequence(int)"/>
        public static BigRational Bernoulli(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            var b = new BigRational[n + 1];
            b[0] = BigRational.One;

            for (int m = 1; m <= n; m++)
            {
                BigRational sum = BigRational.Zero;
                for (int k = 0; k < m; k++)
                    sum += Combinations(m + 1, k) * b[k];
                b[m] = -sum / (m + 1);
            }
            return b[n];
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Бернулли: B(0), B(1), …, B(count−1).
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        /// <remarks>Эффективнее, чем <paramref name="count"/> отдельных вызовов <see cref="Bernoulli(int)"/>.</remarks>
        /// <example>
        /// <code>
        /// var seq = BernoulliSequence(10);
        /// // 1, -1/2, 1/6, 0, -1/30, 0, 1/42, 0, -1/30, 0
        /// </code>
        /// </example>
        public static BigRational[] BernoulliSequence(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            var b = new BigRational[count];
            if (count == 0) return b;
            b[0] = BigRational.One;

            for (int m = 1; m < count; m++)
            {
                BigRational sum = BigRational.Zero;
                for (int k = 0; k < m; k++)
                    sum += Combinations(m + 1, k) * b[k];
                b[m] = -sum / (m + 1);
            }
            return b;
        }
    }
}