using System.Numerics;

namespace Combinatorics;

public static partial class Combinatorics
{
    // ==================== ДЕЛАННУА (ОПТИМИЗИРОВАННЫЙ) ====================

    /// <summary>
    /// Число Деланнуа D(m, n) через альтернативную формулу, эффективную при больших m, n.
    /// </summary>
    /// <param name="m">Горизонтальная координата, m ≥ 0.</param>
    /// <param name="n">Вертикальная координата, n ≥ 0.</param>
    /// <returns>Значение D(m, n).</returns>
    /// <exception cref="ArgumentException">Если m &lt; 0 или n &lt; 0.</exception>
    /// <remarks>
    /// <para>
    /// Использует симметрию <c>D(m, n) = D(n, m)</c> и формулу
    /// <c>D(m, n) = Σ_{k=0}^{m} C(m, k) · C(n+k, m)</c>, где m = min(m, n).
    /// </para>
    /// <para>
    /// Для малых значений (m·n ≤ 2500) делегирует обычному <see cref="Delannoy(int, int)"/>,
    /// который считает через <c>Σ C(m,k)·C(n,k)·2^k</c>.
    /// </para>
    /// <para>
    /// Обе формулы эквивалентны, но альтернативная короче при большом разбросе m и n,
    /// так как суммирование идёт по меньшему из них.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// DelannoyFast(3, 3);       // 63   — идёт через Delannoy (мало)
    /// DelannoyFast(100, 100);   // быстро, несмотря на большое значение
    /// DelannoyFast(500, 500);   // всё ещё быстро
    /// </code>
    /// </example>
    /// <seealso cref="Delannoy(int, int)"/>
    /// <seealso cref="DelannoyCentral(int)"/>
    public static BigInteger DelannoyFast(int m, int n)
    {
        if (m < 0 || n < 0) throw new ArgumentException("m, n ≥ 0");
        if (m == 0 || n == 0) return BigInteger.One;
        if (m * n <= 2500) return Delannoy(m, n);

        // Симметрия: D(m,n) = D(n,m). Работаем с меньшим m.
        if (m > n) (m, n) = (n, m);

        // D(m, n) = Σ_{k=0}^{m} C(m, k) · C(n+k, m)
        BigInteger sum = BigInteger.Zero;
        for (int k = 0; k <= m; k++)
            sum += Combinations(m, k) * Combinations(n + k, m);
        return sum;
    }

    // ==================== ЧИСЛА ДЖЕНОККИ ====================

    /// <summary>
    /// n-е число Дженокки G_n.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение G_n (знак может быть отрицательным).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// <para>
    /// Определяются через производящую функцию: <c>2x / (e^x + 1) = Σ G_n · x^n / n!</c>.
    /// </para>
    /// <para>
    /// Связь с числами Бернулли: <c>G_n = 2(1 − 2^n) · B_n</c>.
    /// </para>
    /// <para>
    /// Свойства:
    /// <list type="bullet">
    ///   <item><description><c>G_0 = 0</c>, <c>G_1 = 1</c>.</description></item>
    ///   <item><description>Все <c>G_{2k+1} = 0</c> для k ≥ 1.</description></item>
    ///   <item><description>Знаки чередуются: <c>G_2 = −1</c>, <c>G_4 = 1</c>, <c>G_6 = −3</c>, <c>G_8 = 17</c>.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Комбинаторный смысл: <c>|G_{2n}|</c> — число чередующихся перестановок
    /// с чётным числом элементов в специальном классе.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// Genocchi(0);   // 0
    /// Genocchi(2);   // -1
    /// Genocchi(4);   // 1
    /// Genocchi(6);   // -3
    /// Genocchi(8);   // 17
    /// </code>
    /// </example>
    /// <seealso href="https://oeis.org/A036968"/>
    /// <seealso cref="Bernoulli(int)"/>
    /// <seealso cref="GenocchiSequence(int)"/>
    public static BigInteger Genocchi(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        if (n == 0) return BigInteger.Zero;
        if (n == 1) return BigInteger.One;

        // G_n = 2 · (1 − 2^n) · B_n
        var b = Bernoulli(n);
        BigInteger coeff = 2 * (BigInteger.One - BigInteger.Pow(2, n));
        var result = coeff * b;
        // B_n — рациональное, но G_n всегда целое
        return result.Num / result.Den;
    }

    /// <summary>
    /// Первые <paramref name="count"/> чисел Дженокки.
    /// </summary>
    /// <param name="count">Сколько чисел вернуть, count ≥ 0.</param>
    /// <returns>Массив длины <paramref name="count"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="count"/> &lt; 0.</exception>
    /// <example>
    /// <code>
    /// GenocchiSequence(10);
    /// // 0, 1, -1, 0, 1, 0, -3, 0, 17, 0
    /// </code>
    /// </example>
    public static BigInteger[] GenocchiSequence(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        var result = new BigInteger[count];
        for (int i = 0; i < count; i++) result[i] = Genocchi(i);
        return result;
    }

    // ==================== ВКЛЮЧЕНИЯ-ИСКЛЮЧЕНИЯ ====================

    /// <summary>
    /// Формула включений-исключений: мощность объединения множеств.
    /// </summary>
    /// <param name="setSizes">Размеры множеств (для одноэлементных пересечений).</param>
    /// <param name="intersectionSizes">
    /// Функция, возвращающая размер пересечения заданного подмножества множеств
    /// (принимает массив индексов).
    /// </param>
    /// <returns><c>|A₁ ∪ A₂ ∪ … ∪ A_n|</c>.</returns>
    /// <exception cref="ArgumentNullException">Если аргумент равен <c>null</c>.</exception>
    /// <remarks>
    /// <para>
    /// Формула: <c>|∪Aᵢ| = Σ|Aᵢ| − Σ|Aᵢ ∩ Aⱼ| + Σ|Aᵢ ∩ Aⱼ ∩ A_k| − …</c>.
    /// </para>
    /// <para>
    /// Общая форма: <c>|∪Aᵢ| = Σ_{∅≠S} (−1)^{|S|+1} · |∩_{i∈S} Aᵢ|</c>.
    /// </para>
    /// <para>
    /// Сложность O(2ⁿ) по числу подмножеств. Для больших n применяются
    /// другие методы (DP по маскам при n ≤ 20).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // |A ∪ B ∪ C| = 10 + 15 + 20 − 5 − 4 − 3 + 1 = 34
    /// var sizes = new[] { 10, 15, 20 };
    /// var union = InclusionExclusion(sizes, idx => idx.Length switch
    /// {
    ///     1 => sizes[idx[0]],
    ///     2 => idx switch { [0,1] => 5, [0,2] => 4, [1,2] => 3, _ => 0 },
    ///     3 => 1,
    ///     _ => 0
    /// });
    /// </code>
    /// </example>
    public static BigInteger InclusionExclusion(
        int[] setSizes,
        Func<int[], BigInteger> intersectionSizes)
    {
        ArgumentNullException.ThrowIfNull(setSizes);
        ArgumentNullException.ThrowIfNull(intersectionSizes);

        int n = setSizes.Length;
        if (n == 0) return BigInteger.Zero;

        BigInteger result = BigInteger.Zero;
        int subsets = 1 << n;

        for (int mask = 1; mask < subsets; mask++)
        {
            var indices = new List<int>();
            for (int i = 0; i < n; i++)
                if ((mask & (1 << i)) != 0) indices.Add(i);

            var intersection = intersectionSizes(indices.ToArray());

            // Знак: + для нечётных подмножеств, − для чётных
            if (indices.Count % 2 == 1) result += intersection;
            else result -= intersection;
        }
        return result;
    }
}