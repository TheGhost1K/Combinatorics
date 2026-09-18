using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Шрёдер (большие и малые) и Деланнуа (закрытая формула vs рекуррентность).
/// Показываем, что рекуррентность для Деланнуа быстрее закрытой формулы,
/// так как не требует вызова Combinations на каждом шаге.
/// </summary>
[MemoryDiagnoser]
public class SchroederDelannoyBenchmarks
{
    [Params(10, 30, 60, 100)]
    public int N;

    // ==================== ШРЁДЕР ====================

    [Benchmark(Baseline = true)]
    public BigInteger SchroederLarge_Single() => SchroederLarge(N);

    [Benchmark]
    public BigInteger[] SchroederLarge_Sequence() => SchroederLargeSequence(N);

    [Benchmark]
    public BigInteger SchroederSmall_Single() => SchroederSmall(N);

    [Benchmark]
    public BigInteger[] SchroederSmall_Sequence() => SchroederSmallSequence(N);

    [Benchmark]
    public BigInteger SchroederCatalan_Single() => SchroederCatalan(N);

    // ==================== ДЕЛАННУА ====================

    [Benchmark]
    public BigInteger Delannoy_ClosedForm() => Delannoy(N / 2, N / 2);

    [Benchmark]
    public BigInteger Delannoy_Recurrence() => DelannoyByRecurrence(N / 2, N / 2);

    [Benchmark]
    public BigInteger DelannoyCentral_ClosedForm() => DelannoyCentral(N);

    [Benchmark]
    public BigInteger[] DelannoyCentral_Sequence() => DelannoyCentralSequence(N);
}