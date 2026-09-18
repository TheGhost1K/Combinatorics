using System.Numerics;

namespace Combinatorics
{
    /// <summary>
    /// Статический класс комбинаторных вычислений.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Содержит методы для факториалов, сочетаний, размещений, чисел Каталана,
    /// Стирлинга, Белла, Эйлера, Лаха, Нараяны, Моцкина, Шрёдера, Деланнуа,
    /// Фусса–Каталана, Фибоначчи, Люка, Пелля, Бернулли и их асимптотик.
    /// </para>
    /// <para>
    /// Класс разбит на несколько <c>partial</c>-файлов по логическим группам:
    /// <list type="bullet">
    ///   <item><description><c>Combinatorics.Basic.cs</c> — факториал, сочетания, размещения, Каталан, Паскаль.</description></item>
    ///   <item><description><c>Combinatorics.Stirling.cs</c> — числа Стирлинга 1-го и 2-го рода.</description></item>
    ///   <item><description><c>Combinatorics.BellEuler.cs</c> — числа Белла, Эйлера.</description></item>
    ///   <item><description><c>Combinatorics.Lah.cs</c> — числа Лаха и Fubini.</description></item>
    ///   <item><description><c>Combinatorics.Narayana.cs</c> — числа Нараяны.</description></item>
    ///   <item><description><c>Combinatorics.Motzkin.cs</c> — числа Моцкина.</description></item>
    ///   <item><description><c>Combinatorics.Bernoulli.cs</c> — числа Бернулли.</description></item>
    ///   <item><description><c>Combinatorics.Generators.cs</c> — генераторы перестановок и сочетаний.</description></item>
    ///   <item><description><c>Combinatorics.Extra.cs</c> — Деланнуа, Шрёдер, Фусс–Каталан.</description></item>
    ///   <item><description><c>Combinatorics.PellSchroeder.cs</c> — Пелль, Пелль–Люка, Шрёдер–Каталан.</description></item>
    ///   <item><description><c>Combinatorics.FibonacciMotzkin.cs</c> — Фибоначчи, Люка, обобщённый Моцкин.</description></item>
    ///   <item><description><c>Combinatorics.Binet.cs</c> — формулы Бине в точной арифметике.</description></item>
    ///   <item><description><c>Combinatorics.Asymptotics.cs</c> — базовые асимптотики.</description></item>
    ///   <item><description><c>Combinatorics.AsymptoticsExtended.cs</c> — расширенные асимптотики.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Все методы возвращают <see cref="BigInteger"/> или <see cref="BigRational"/>,
    /// чтобы избежать переполнения на больших n.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using static Combinatorics.Combinatorics;
    ///
    /// var f = Factorial(20);           // 2432902008176640000
    /// var c = Combinations(52, 5);     // 2598960
    /// var cat = Catalan(10);           // 16796
    /// </code>
    /// </example>
    public static partial class Combinatorics
    {
        /// <summary>
        /// Вычисляет факториал <c>n!</c> через divide &amp; conquer.
        /// </summary>
        /// <param name="n">Неотрицательное целое число.</param>
        /// <returns>Произведение всех целых от 1 до n; 0! = 1.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>
        /// В отличие от линейного цикла, использует рекурсивное разбиение диапазона
        /// пополам, что позволяет BCL применять быстрые алгоритмы умножения
        /// (Карацубы, Toom-Cook, Шёнхаге–Штрассена) на сопоставимо больших числах.
        /// </para>
        /// <para>
        /// Для n &lt; 100 разница с линейным циклом незначительна; для n &gt; 1000
        /// ускорение составляет 5–10 раз.
        /// </para>
        /// <para>
        /// Сложность: O(M(n log n) · log n), где M — сложность умножения двух n-битных чисел.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var f20  = Factorial(20);    // 2432902008176640000
        /// var f100 = Factorial(100);   // 9.33e157
        /// var f10k = Factorial(10000); // вычисляется за десятки миллисекунд
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A000142"/>
        /// <seealso cref="FactorialRange(int, int)"/>
        /// <seealso cref="Permutations(int)"/>
        public static BigInteger Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть ≥ 0");
            return FactorialRange(2, n);
        }

        /// <summary>
        /// Вычисляет произведение всех целых в диапазоне <c>[lo, hi]</c> включительно
        /// методом divide &amp; conquer.
        /// </summary>
        /// <param name="lo">Нижняя граница диапазона включительно.</param>
        /// <param name="hi">Верхняя граница диапазона включительно.</param>
        /// <returns>
        /// Произведение <c>lo · (lo+1) · … · hi</c>.
        /// Если <paramref name="lo"/> &gt; <paramref name="hi"/> — возвращает 1 (пустое произведение).
        /// </returns>
        /// <remarks>
        /// <para>
        /// Диапазон рекурсивно делится пополам до базовых случаев:
        /// </para>
        /// <list type="bullet">
        ///   <item><description><c>lo &gt; hi</c> — возвращает 1 (пустое произведение).</description></item>
        ///   <item><description><c>lo == hi</c> — возвращает <paramref name="lo"/>.</description></item>
        ///   <item><description><c>hi − lo == 1</c> — возвращает <c>lo · hi</c>.</description></item>
        ///   <item><description>иначе — перемножает результаты двух половин.</description></item>
        /// </list>
        /// <para>
        /// Такой подход даёт сомножители примерно одинакового размера, что критично
        /// для включения быстрых алгоритмов умножения в <see cref="BigInteger"/>.
        /// </para>
        /// <para>
        /// Глубина рекурсии — O(log(hi − lo)). Память — O(log(hi − lo)) под стек.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var p1 = FactorialRange(1, 5);   // 120 = 5!
        /// var p2 = FactorialRange(3, 7);   // 2520 = 3·4·5·6·7
        /// var p3 = FactorialRange(10, 9);  // 1 — пустое произведение
        /// </code>
        /// </example>
        /// <seealso cref="Factorial(int)"/>
        private static BigInteger FactorialRange(int lo, int hi)
        {
            if (lo > hi) return BigInteger.One;
            if (lo == hi) return lo;
            if (hi - lo == 1) return (BigInteger)lo * hi;
            int mid = (lo + hi) >> 1;
            return FactorialRange(lo, mid) * FactorialRange(mid + 1, hi);
        }

        /// <summary>
        /// Вычисляет число перестановок из n элементов: <c>P(n) = n!</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое число.</param>
        /// <returns>Факториал n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Синоним <see cref="Factorial(int)"/>. Оставлен для семантической ясности
        /// в комбинаторных задачах.
        /// </remarks>
        /// <seealso href="https://oeis.org/A000142"/>
        public static BigInteger Permutations(int n) => Factorial(n);

        /// <summary>
        /// Число перестановок с повторениями: <c>n! / (n₁! · n₂! · … · n_k!)</c>.
        /// </summary>
        /// <param name="counts">Массив количеств каждого типа элементов. Сумма — общее число n.</param>
        /// <returns>Число различных перестановок мультимножества.</returns>
        /// <exception cref="ArgumentException">Если <paramref name="counts"/> пуст или содержит отрицательное число.</exception>
        /// <remarks>
        /// Используется для подсчёта анаграмм: например, для слова «MISSISSIPPI»
        /// (M×1, I×4, S×4, P×2) получаем <c>11!/(1!·4!·4!·2!) = 34650</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var anagrams = PermutationsWithRepetition(1, 4, 4, 2); // MISSISSIPPI → 34650
        /// </code>
        /// </example>
        public static BigInteger PermutationsWithRepetition(params int[] counts)
        {
            if (counts is null || counts.Length == 0)
                throw new ArgumentException("counts не может быть пустым", nameof(counts));
            int total = 0;
            BigInteger denom = BigInteger.One;
            foreach (var c in counts)
            {
                if (c < 0) throw new ArgumentException("counts содержит отрицательное число", nameof(counts));
                total += c;
                denom *= Factorial(c);
            }
            return Factorial(total) / denom;
        }

        /// <summary>
        /// Число размещений из n по k: <c>A(n, k) = n! / (n−k)!</c>.
        /// </summary>
        /// <param name="n">Общее число элементов, n ≥ 0.</param>
        /// <param name="k">Число выбираемых элементов, 0 ≤ k ≤ n.</param>
        /// <returns>Число упорядоченных выборок длины k из n элементов.</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0, k &lt; 0 или k &gt; n.</exception>
        /// <remarks>
        /// В отличие от <see cref="Combinations(int, int)"/>, порядок элементов важен.
        /// Сложность O(k).
        /// </remarks>
        /// <example>
        /// <code>
        /// var podium = Arrangements(10, 3); // 720 — сколько способов распределить 3 призовых места
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A008279"/>
        public static BigInteger Arrangements(int n, int k)
        {
            if (n < 0 || k < 0 || k > n) throw new ArgumentException("Требуется 0 ≤ k ≤ n");
            BigInteger r = BigInteger.One;
            for (int i = n - k + 1; i <= n; i++) r *= i;
            return r;
        }

        /// <summary>
        /// Число сочетаний из n по k: <c>C(n, k) = n! / (k! · (n−k)!)</c>.
        /// </summary>
        /// <param name="n">Общее число элементов, n ≥ 0.</param>
        /// <param name="k">Число выбираемых элементов, 0 ≤ k ≤ n.</param>
        /// <returns>Биномиальный коэффициент C(n, k).</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0, k &lt; 0 или k &gt; n.</exception>
        /// <remarks>
        /// <para>
        /// Используется симметрия <c>C(n, k) = C(n, n−k)</c> и поочерёдное умножение/деление,
        /// чтобы промежуточные значения оставались малыми. Точное деление гарантировано
        /// на каждом шаге (произведение i последовательных целых делится на i!).
        /// </para>
        /// <para>Сложность O(min(k, n−k)).</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var hands = Combinations(52, 5);   // 2598960 — покерные руки
        /// var small = Combinations(5, 2);    // 10
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A007318"/>
        /// <seealso cref="CombinationsWithRepetition(int, int)"/>
        public static BigInteger Combinations(int n, int k)
        {
            if (n < 0 || k < 0 || k > n) throw new ArgumentException("Требуется 0 ≤ k ≤ n");
            k = Math.Min(k, n - k);
            BigInteger r = BigInteger.One;
            for (int i = 1; i <= k; i++)
            {
                r *= (n - k + i);
                r /= i;
            }
            return r;
        }

        /// <summary>
        /// Число сочетаний с повторениями: <c>C(n+k−1, k)</c>.
        /// </summary>
        /// <param name="n">Число типов элементов, n ≥ 0.</param>
        /// <param name="k">Размер выборки, k ≥ 0.</param>
        /// <returns>Число мультимножеств размера k из n типов.</returns>
        /// <exception cref="ArgumentException">Если n &lt; 0 или k &lt; 0.</exception>
        /// <remarks>
        /// Эквивалентно числу способов разложить k неразличимых шаров по n различимым ящикам.
        /// </remarks>
        /// <example>
        /// <code>
        /// var ways = CombinationsWithRepetition(3, 4); // 15
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A059481"/>
        public static BigInteger CombinationsWithRepetition(int n, int k)
        {
            if (n < 0 || k < 0) throw new ArgumentException("n, k ≥ 0");
            if (n == 0) return k == 0 ? BigInteger.One : BigInteger.Zero;
            return Combinations(n + k - 1, k);
        }

        /// <summary>
        /// n-е число Каталана: <c>C_n = C(2n, n) / (n+1)</c>.
        /// </summary>
        /// <param name="n">Неотрицательное целое.</param>
        /// <returns>Число Каталана C_n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// <para>
        /// Считает: правильные скобочные последовательности, бинарные деревья,
        /// пути Дика, триангуляции выпуклого (n+2)-угольника.
        /// </para>
        /// <para>Асимптотика: <c>C_n ~ 4^n / (n^(3/2)·√π)</c> — см. <see cref="CatalanApprox(int)"/>.</para>
        /// <para>Рекуррентность: <c>C_{n+1} = Σ_{i=0}^{n} C_i · C_{n-i}</c>.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var c10 = Catalan(10);   // 16796
        /// var c15 = Catalan(15);   // 9694845
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A000108"/>
        /// <seealso cref="CatalanApprox(int)"/>
        /// <seealso cref="FussCatalan(int, int)"/>
        /// <seealso cref="Narayana(int, int)"/>
        public static BigInteger Catalan(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            return Combinations(2 * n, n) / (n + 1);
        }

        /// <summary>
        /// Первые <paramref name="count"/> чисел Каталана: C(0), C(1), …, C(count−1).
        /// </summary>
        /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
        /// <returns>Массив длины <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
        /// <remarks>
        /// Использует рекуррентность <c>C_{n+1} = C_n · 2·(2n+1) / (n+2)</c> за O(n) операций.
        /// В ~50 раз быстрее, чем <paramref name="count"/> отдельных вызовов <see cref="Catalan(int)"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var seq = CatalanSequence(11);
        /// // 1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862, 16796
        /// </code>
        /// </example>
        public static BigInteger[] CatalanSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            var result = new BigInteger[count];
            if (count == 0) return result;
            result[0] = BigInteger.One;
            for (int n = 0; n < count - 1; n++)
            {
                // C_{n+1} = C_n * 2*(2n+1) / (n+2)
                result[n + 1] = result[n] * (2 * (2 * n + 1)) / (n + 2);
            }
            return result;
        }

        /// <summary>
        /// n-я строка треугольника Паскаля — все биномиальные коэффициенты C(n, k) для k = 0..n.
        /// </summary>
        /// <param name="n">Номер строки, n ≥ 0.</param>
        /// <returns>Массив длины n+1 с коэффициентами C(n, k).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
        /// <remarks>
        /// Строится за O(n²) с использованием свойства <c>C(n, k) = C(n−1, k−1) + C(n−1, k)</c>.
        /// Сумма элементов строки = <c>2^n</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var row10 = PascalRow(10);
        /// // 1, 10, 45, 120, 210, 252, 210, 120, 45, 10, 1
        /// </code>
        /// </example>
        /// <seealso href="https://oeis.org/A007318"/>
        public static BigInteger[] PascalRow(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            var row = new BigInteger[n + 1];
            row[0] = BigInteger.One;
            for (int i = 1; i <= n; i++)
            {
                row[i] = BigInteger.One;
                for (int j = i - 1; j > 0; j--) row[j] += row[j - 1];
            }
            return row;
        }
    }
}