using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace Combinatorics.Benchmarks;

[MemoryDiagnoser]
public class GeneratorBenchmarks
{
    [Params(5, 7, 9)]
    public int N;

    private int[] _items = [];

    [GlobalSetup]
    public void Setup() => _items = [.. Enumerable.Range(0, N)];

    [Benchmark]
    public int Permutations_Heap_Count()
    {
        int c = 0;
        foreach (var _ in Combinatorics.Permutations(_items)) c++;
        return c;
    }

    [Benchmark]
    public int Permutations_ToList_Count()
        => Combinatorics.Permutations(_items).Count();

    [Benchmark]
    public int Combinations_Count()
    {
        int c = 0;
        foreach (var _ in Combinatorics.CombinationsOf(_items, N / 2)) c++;
        return c;
    }

    // ==================== СОЧЕТАНИЯ — ЧЕРЕЗ ИНДЕКСЫ ====================

    /// <summary>
    /// Сочетания индексов через EnumerateCombinationIndices — минимальные аллокации,
    /// возвращает int[] индексов без копирования элементов.
    /// </summary>
    [Benchmark]
    public int EnumerateCombinationIndices_Count()
    {
        int c = 0;
        foreach (var _ in Combinatorics.EnumerateCombinationIndices(N, N / 2)) c++;
        return c;
    }

    // ==================== СОЧЕТАНИЯ — ЧИСЛО (без перебора) ====================

    /// <summary>
    /// Число сочетаний через формулу — без генерации, для сравнения с перебором.
    /// </summary>
    [Benchmark]
    public BigInteger Combinations_Formula()
        => Combinatorics.Combinations(N, N / 2);
}