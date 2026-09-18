using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests;

public class GeneratorTests
{
    // ==================== ПЕРЕСТАНОВКИ ====================

    [Fact]
    public void Permutations_CountMatchesFactorial()
    {
        for (int n = 0; n <= 7; n++)
        {
            var items = Enumerable.Range(0, n).ToArray();
            long count = Permutations(items).LongCount();
            Assert.Equal((long)BasicFactorial(n), count);
        }
    }

    [Fact]
    public void Permutations_AllDistinct()
    {
        var items = new[] { 1, 2, 3, 4 };
        var seen = new HashSet<string>();
        foreach (var p in Permutations(items))
            Assert.True(seen.Add(string.Join(",", p)));
        Assert.Equal(24, seen.Count);
    }

    [Fact]
    public void Permutations_Empty()
    {
        var result = Permutations(Array.Empty<int>()).ToList();
        var item = Assert.Single(result);
        Assert.Empty(item);
    }

    // ==================== СОЧЕТАНИЯ — ИНДЕКСЫ ====================

    [Fact]
    public void EnumerateCombinationIndices_CountMatchesFormula()
    {
        for (int n = 0; n <= 12; n++)
            for (int k = 0; k <= n; k++)
                Assert.Equal(Combinations(n, k), EnumerateCombinationIndices(n, k).LongCount());
    }

    [Fact]
    public void EnumerateCombinationIndices_AllDistinct()
    {
        var seen = new HashSet<string>();
        foreach (var c in EnumerateCombinationIndices(5, 3))
            Assert.True(seen.Add(string.Join(",", c)));
        Assert.Equal(10, seen.Count);
    }

    [Fact]
    public void EnumerateCombinationIndices_K0_YieldsEmpty()
    {
        var result = EnumerateCombinationIndices(5, 0).ToList();
        var item = Assert.Single(result);
        Assert.Empty(item);
    }

    [Fact]
    public void EnumerateCombinationIndices_KGreaterN_YieldsNothing()
        => Assert.Empty(EnumerateCombinationIndices(3, 5));

    [Fact]
    public void EnumerateCombinationIndices_Lexicographic()
    {
        // Для n=4, k=2 ожидаем строго лексикографический порядок
        var expected = new[] { "0,1", "0,2", "0,3", "1,2", "1,3", "2,3" };
        var actual = EnumerateCombinationIndices(4, 2).Select(c => string.Join(",", c)).ToArray();
        Assert.Equal(expected, actual);
    }

    // ==================== СОЧЕТАНИЯ — ЭЛЕМЕНТЫ ====================

    [Fact]
    public void CombinationsOf_Items_Works()
    {
        var result = CombinationsOf(["A", "B", "C", "D"], 2).ToList();
        Assert.Equal(6, result.Count);
        Assert.Contains(result, c => c[0] == "A" && c[1] == "B");
        Assert.Contains(result, c => c[0] == "C" && c[1] == "D");
    }

    [Fact]
    public void CombinationsOf_IntRange_Works()
    {
        var result = CombinationsOf(5, 2).ToList();
        Assert.Equal(10, result.Count);
        Assert.All(result, c => Assert.Equal(2, c.Length));
        Assert.All(result, c => Assert.True(c[0] < c[1]));
    }

    [Fact]
    public void CombinationsOf_CountMatchesFormula()
    {
        for (int n = 0; n <= 10; n++)
            for (int k = 0; k <= n; k++)
                Assert.Equal(Combinations(n, k), CombinationsOf(n, k).LongCount());
    }

    [Fact]
    public void CombinationsOf_NullItems_Throws()
        => Assert.Throws<ArgumentNullException>(() => CombinationsOf<int>(null!, 2).ToList());

    // ==================== ДИАМЕТРАЛЬНОЕ РАЗЛИЧИЕ ====================

    [Fact]
    public void CombinationsName_NoConflict_BetweenNumericAndGeneric()
    {
        // Числовое Combinations — BigInteger
        BigInteger numeric = Combinations(5, 2);
        Assert.Equal(new BigInteger(10), numeric);

        // Генератор — через CombinationsOf
        var enumerated = CombinationsOf(5, 2).ToList();
        Assert.Equal(10, enumerated.Count);

        // Компилятор не путает эти вызовы
        Assert.Equal((long)numeric, (long)enumerated.Count);
    }

    private static long BasicFactorial(int n)
    {
        long r = 1;
        for (int i = 2; i <= n; i++) r *= i;
        return r;
    }
}