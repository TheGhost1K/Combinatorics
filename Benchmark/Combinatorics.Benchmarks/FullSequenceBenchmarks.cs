using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Сводный прогон по всем семействам с одинаковым n.
/// Позволяет увидеть, какие последовательности самые «дорогие».
/// </summary>
/// <remarks>
/// Имена методов бенчмарков намеренно НЕ совпадают с именами методов
/// <see cref="Combinatorics"/>: из-за <c>using static</c> одинаковые имена
/// приводят к неоднозначности при разрешении перегрузки.
/// </remarks>
[MemoryDiagnoser]
public class FullSequenceBenchmarks
{
    [Params(20, 50, 100)]
    public int N;

    // ==================== БАЗОВЫЕ ====================

    [Benchmark]
    public BigInteger Bench_Factorial() => Factorial(N);

    [Benchmark]
    public BigInteger Bench_Catalan() => Catalan(N);

    [Benchmark]
    public BigInteger Bench_Combinations() => Combinations(N, N / 2);

    // ==================== МОЦКИН / ШРЁДЕР ====================

    [Benchmark]
    public BigInteger Bench_Motzkin() => Motzkin(N);

    [Benchmark]
    public BigInteger Bench_MotzkinTwoColored() => MotzkinTwoColored(N);

    [Benchmark]
    public BigInteger Bench_MotzkinGeneralized2() => MotzkinGeneralized(N, 2);

    [Benchmark]
    public BigInteger Bench_SchroederLarge() => SchroederLarge(N);

    [Benchmark]
    public BigInteger Bench_SchroederSmall() => SchroederSmall(N);

    [Benchmark]
    public BigInteger Bench_SchroederCatalan() => SchroederCatalan(N);

    // ==================== БЕЛЛ / ЭЙЛЕР ====================

    [Benchmark]
    public BigInteger Bench_Bell() => Bell(N);

    [Benchmark]
    public BigInteger Bench_OrderedBell() => OrderedBell(N);

    [Benchmark]
    public BigInteger Bench_Eulerian() => Eulerian(N, N / 2);

    // ==================== СТИРЛИНГ / ЛАХ / НАРАЯН ====================

    [Benchmark]
    public BigInteger Bench_StirlingFirstKind() => StirlingFirstKind(N, N / 2);

    [Benchmark]
    public BigInteger Bench_StirlingSecondKind() => StirlingSecondKind(N, N / 2);

    [Benchmark]
    public BigInteger Bench_Lah() => Lah(N, N / 2);

    [Benchmark]
    public BigInteger Bench_Narayana() => Narayana(N, N / 2);

    // ==================== ДЕЛАННУА / ФУСС–КАТАЛАН ====================

    [Benchmark]
    public BigInteger Bench_DelannoyCentral() => DelannoyCentral(N);

    [Benchmark]
    public BigInteger Bench_FussCatalan2() => FussCatalan(N, 2);

    // ==================== РЕКУРРЕНТНЫЕ ====================

    [Benchmark]
    public BigInteger Bench_Fibonacci() => Fibonacci(N);

    [Benchmark]
    public BigInteger Bench_FibonacciFast() => FibonacciFast(N);

    [Benchmark]
    public BigInteger Bench_Lucas() => Lucas(N);

    [Benchmark]
    public BigInteger Bench_Pell() => Pell(N);

    [Benchmark]
    public BigInteger Bench_PellLucas() => PellLucas(N);

    // ==================== БЕРНУЛЛИ ====================

    /// <summary>
    /// Возвращает <see cref="BigRational"/>, а не <see cref="BigInteger"/>,
    /// поэтому сигнатура отличается.
    /// </summary>
    [Benchmark]
    public BigRational Bench_Bernoulli() => Bernoulli(N);
}