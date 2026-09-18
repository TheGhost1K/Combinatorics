using System.Numerics;

namespace Combinatorics
{
    public static partial class Combinatorics
    {
        /// <summary>
        /// Перебирает все перестановки коллекции (Heap's algorithm).
        /// </summary>
        /// <typeparam name="T">Тип элементов.</typeparam>
        /// <param name="items">Исходная коллекция. Не изменяется.</param>
        /// <returns>Ленивая последовательность массивов — перестановок элементов.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="items"/> равен <c>null</c>.</exception>
        /// <remarks>
        /// <para>
        /// Возвращает <c>n!</c> перестановок в произвольном (не лексикографическом) порядке.
        /// Каждая перестановка — новый массив. Исходная коллекция не модифицируется.
        /// </para>
        /// <para>Сложность O(n!) по времени, O(n) по памяти.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach (var p in Permutations(new[] { 1, 2, 3 }))
        ///     Console.WriteLine(string.Join(", ", p));
        /// // 1, 2, 3
        /// // 2, 1, 3
        /// // 3, 1, 2
        /// // 1, 3, 2
        /// // 2, 3, 1
        /// // 3, 2, 1
        /// </code>
        /// </example>
        /// <seealso cref="Permutations(int)"/>
        public static IEnumerable<T[]> Permutations<T>(IReadOnlyList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            int n = items.Count;
            var arr = new T[n];
            for (int i = 0; i < n; i++) arr[i] = items[i];

            if (n == 0) { yield return Array.Empty<T>(); yield break; }
            if (n == 1) { yield return arr; yield break; }

            var c = new int[n];
            yield return (T[])arr.Clone();

            int idx = 0;
            while (idx < n)
            {
                if (c[idx] < idx)
                {
                    int swapWith = (idx % 2 == 0) ? 0 : c[idx];
                    (arr[swapWith], arr[idx]) = (arr[idx], arr[swapWith]);
                    yield return (T[])arr.Clone();
                    c[idx]++;
                    idx = 0;
                }
                else
                {
                    c[idx] = 0;
                    idx++;
                }
            }
        }

        /// <summary>
        /// Перебирает все сочетания индексов <c>0..n−1</c> по k в лексикографическом порядке.
        /// </summary>
        /// <param name="n">Общее число индексов, n ≥ 0.</param>
        /// <param name="k">Размер сочетания, 0 ≤ k ≤ n.</param>
        /// <returns>Ленивая последовательность отсортированных массивов индексов.</returns>
        /// <remarks>
        /// <para>
        /// Возвращает <c>C(n, k)</c> сочетаний. Каждое — возрастающий массив длины k.
        /// Имя <c>EnumerateCombinationIndices</c> выбрано, чтобы не конфликтовать с числовым
        /// <see cref="Combinations(int, int)"/>.
        /// </para>
        /// <para>Сложность O(C(n, k) · k) по времени, O(k) по памяти.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach (var idx in EnumerateCombinationIndices(4, 2))
        ///     Console.WriteLine(string.Join(",", idx));
        /// // 0,1
        /// // 0,2
        /// // 0,3
        /// // 1,2
        /// // 1,3
        /// // 2,3
        /// </code>
        /// </example>
        /// <seealso cref="CombinationsOf{T}(IReadOnlyList{T}, int)"/>
        public static IEnumerable<int[]> EnumerateCombinationIndices(int n, int k)
        {
            if (n < 0 || k < 0 || k > n) yield break;
            if (k == 0) { yield return Array.Empty<int>(); yield break; }

            var indices = new int[k];
            for (int i = 0; i < k; i++) indices[i] = i;

            while (true)
            {
                yield return (int[])indices.Clone();

                int i = k - 1;
                while (i >= 0 && indices[i] == n - k + i) i--;
                if (i < 0) yield break;
                indices[i]++;
                for (int j = i + 1; j < k; j++) indices[j] = indices[j - 1] + 1;
            }
        }

        /// <summary>
        /// Перебирает все сочетания элементов коллекции по k.
        /// </summary>
        /// <typeparam name="T">Тип элементов.</typeparam>
        /// <param name="items">Исходная коллекция. Не изменяется.</param>
        /// <param name="k">Размер сочетания, 0 ≤ k ≤ items.Count.</param>
        /// <returns>Ленивая последовательность массивов длины k.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="items"/> равен <c>null</c>.</exception>
        /// <remarks>
        /// Имя <c>CombinationsOf</c> выбрано, чтобы не конфликтовать с числовым
        /// <see cref="Combinations(int, int)"/>, возвращающим <see cref="BigInteger"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach (var combo in CombinationsOf(new[] { "A", "B", "C", "D" }, 2))
        ///     Console.WriteLine(string.Join("", combo));
        /// // AB AC AD BC BD CD
        /// </code>
        /// </example>
        /// <seealso cref="EnumerateCombinationIndices(int, int)"/>
        /// <seealso cref="CombinationsOf(int, int)"/>
        public static IEnumerable<T[]> CombinationsOf<T>(IReadOnlyList<T> items, int k)
        {
            ArgumentNullException.ThrowIfNull(items);
            foreach (var idx in EnumerateCombinationIndices(items.Count, k))
            {
                var result = new T[k];
                for (int i = 0; i < k; i++) result[i] = items[idx[i]];
                yield return result;
            }
        }

        /// <summary>
        /// Перебирает все сочетания по k из диапазона <c>0..n−1</c>.
        /// </summary>
        /// <param name="n">Общее число элементов, n ≥ 0.</param>
        /// <param name="k">Размер сочетания, 0 ≤ k ≤ n.</param>
        /// <returns>Ленивая последовательность массивов длины k с возрастающими числами.</returns>
        /// <remarks>Удобная обёртка над <see cref="CombinationsOf{T}(IReadOnlyList{T}, int)"/>.</remarks>
        /// <example>
        /// <code>
        /// foreach (var combo in CombinationsOf(5, 2))
        ///     Console.WriteLine(string.Join(",", combo));
        /// // 0,1 0,2 0,3 0,4 1,2 1,3 1,4 2,3 2,4 3,4
        /// </code>
        /// </example>
        public static IEnumerable<int[]> CombinationsOf(int n, int k)
            => CombinationsOf(Enumerable.Range(0, n).ToArray(), k);
    }
}