using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// n-е число Пелля: <c>P(0) = 0, P(1) = 1, P(n) = 2·P(n−1) + P(n−2)</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение P(n).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Формула Бине: <c>P(n) = ((1+√2)^n − (1−√2)^n) / (2√2)</c> — см. <see cref="PellBinet(int)"/>.</para>
        /// <para>Асимптотика: <c>P(n) ~ (1+√2)^n / (2√2)</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var p10 = Pell(10); // 2378
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A000129"/>
        /// <seealso cref="PellLucas(int)"/>
        /// <seealso cref="PellBinet(int)"/>
        public static BigInteger Pell(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0) return BigInteger.Zero;
            if (n == 1) return BigInteger.One;

            BigInteger a = BigInteger.Zero;
            BigInteger b = BigInteger.One;
            for (int i = 2; i <= n; i++)
            {
                BigInteger c = 2 * b + a;
                a = b;
                b = c;
            }
            return b;
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Пелля.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] PellSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.Zero;
            if (count == 1) return result;
            result[1] = BigInteger.One;

            for (int i = 2; i < count; i++)
                result[i] = 2 * result[i - 1] + result[i - 2];
            return result;
        }

        /// <summary>
        /// n-е число Пелля–Люка: <c>Q(0) = Q(1) = 2, Q(n) = 2·Q(n−1) + Q(n−2)</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение Q(n).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Формула Бине: <c>Q(n) = (1+√2)^n + (1−√2)^n</c> — см. <see cref="PellLucasBinet(int)"/>.</para>
        /// <para>Тождество: <c>Q(n)² − 8·P(n)² = 4·(−1)^n</c> — см. <see cref="PellLucasIdentity(int)"/>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var q9 = PellLucas(9); // 2786
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A002203"/>
        public static BigInteger PellLucas(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0 || n == 1) return new BigInteger(2);

            BigInteger a = new(2);
            BigInteger b = new(2);
            for (int i = 2; i <= n; i++)
            {
                BigInteger c = 2 * b + a;
                a = b;
                b = c;
            }
            return b;
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Пелля–Люка.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] PellLucasSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = new BigInteger(2);
            if (count == 1) return result;
            result[1] = new BigInteger(2);

            for (int i = 2; i < count; i++)
                result[i] = 2 * result[i - 1] + result[i - 2];
            return result;
        }

        /// <summary>
        /// Проверяет тождество Пелля–Люка: <c>Q(n)² − 8·P(n)² = 4·(−1)^n</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns><c>true</c>, если тождество выполняется для данного n.</returns>
        /// <remarks>
        /// Используется в тестах для проверки корректности реализаций
        /// <see cref="Pell(int)"/> и <see cref="PellLucas(int)"/>.
        /// </remarks>
        public static bool PellLucasIdentity(int n)
        {
            BigInteger q = PellLucas(n);
            BigInteger p = Pell(n);
            BigInteger lhs = q * q - 8 * p * p;
            BigInteger rhs = (n % 2 == 0) ? new BigInteger(4) : new BigInteger(-4);
            return lhs == rhs;
        }

        /// <summary>
        /// Обобщённое число Моцкина с двумя цветами горизонтального шага.
        /// </summary>
        /// <param name="n">Длина пути, n ≥ 0.</param>
        /// <returns>Значение M_n^(2).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Рекуррентность: <c>M(0) = 1, M(n+1) = 2·M(n) + Σ_{i=0}^{n-1} M(i)·M(n−1−i)</c>.</para>
        /// <para>Совпадает с числом Каталана следующего порядка: <c>M_n^(2) = C_{n+1}</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var m5 = MotzkinTwoColored(5); // 132 = Catalan(6)
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A006531"/>
        /// <seealso cref="MotzkinGeneralized(int, int)"/>
        /// <seealso cref="Catalan(int)"/>
        public static BigInteger MotzkinTwoColored(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0) return BigInteger.One;
            if (n == 1) return new BigInteger(2);

            var m = new BigInteger[n + 1];
            m[0] = BigInteger.One;
            for (int k = 1; k <= n; k++)
            {
                BigInteger s = 2 * m[k - 1];
                for (int i = 0; i < k - 1; i++)
                    s += m[i] * m[k - 2 - i];
                m[k] = s;
            }
            return m[n];
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Моцкина с двумя цветами.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] MotzkinTwoColoredSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.One;

            for (int k = 1; k < count; k++)
            {
                BigInteger s = 2 * result[k - 1];
                for (int i = 0; i < k - 1; i++)
                    s += result[i] * result[k - 2 - i];
                result[k] = s;
            }
            return result;
        }

        /// <summary>
        /// Числа Шрёдера–Каталана (малые числа Шрёдера, OEIS A001003).
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение s_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>Связь с большими числами Шрёдера: <c>s_n = S_n / 2</c> для n ≥ 1.</para>
        /// <para>s_0 = S_0 = 1.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var s7 = SchroederCatalan(7); // 4279
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A001003"/>
        /// <seealso cref="SchroederLarge(int)"/>
        public static BigInteger SchroederCatalan(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            if (n == 0) return BigInteger.One;
            return SchroederLarge(n) / 2;
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Шрёдера–Каталана.
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        public static BigInteger[] SchroederCatalanSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.One;
            for (int i = 1; i < count; i++) result[i] = SchroederLarge(i) / 2;
            return result;
        }

        /// <summary>
        /// Альтернативная формула для больших чисел Шрёдера через сумму:
        /// <c>R(n) = Σ_{k=0}^{n} C(n+k, 2k) · C_k</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Значение, совпадающее с <see cref="SchroederLarge(int)"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>Используется в тестах для перекрёстной проверки.</remarks>
        public static BigInteger SchroederCatalanBySum(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            BigInteger sum = BigInteger.Zero;
            for (int k = 0; k <= n; k++)
                sum += Combinations(n + k, 2 * k) * Catalan(k);
            return sum;
        }
    }
}