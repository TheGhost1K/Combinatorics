using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Фусс–Каталан для разных m и Фибоначчи/Люка/Пелль — разные подходы.
/// </summary>
[MemoryDiagnoser]
public class FussCatalanMotzkinBenchmarks
{
    [Params(10, 30, 60, 100)]
    public int N;

    // ==================== ФУСС–КАТАЛАН ====================

    [Benchmark(Baseline = true)]
    public BigInteger FussCatalan_M2() => FussCatalan(N, 2);

    [Benchmark]
    public BigInteger FussCatalan_M3() => FussCatalan(N, 3);

    [Benchmark]
    public BigInteger FussCatalan_M5() => FussCatalan(N, 5);

    [Benchmark]
    public BigInteger[] FussCatalan_M2_Sequence() => FussCatalanSequence(N, 2);

    // ==================== ФИБОНАЧЧИ / ЛЮКА ====================

    [Benchmark]
    public BigInteger Fibonacci_Iterative() => Fibonacci(N);

    [Benchmark]
    public BigInteger Fibonacci_FastDoubling() => FibonacciFast(N);

    [Benchmark]
    public BigInteger Fibonacci_Binet() => FibonacciBinet(N);

    [Benchmark]
    public BigInteger Lucas_Iterative() => Lucas(N);

    [Benchmark]
    public BigInteger Lucas_Binet() => LucasBinet(N);

    // ==================== ПЕЛЛЬ ====================

    [Benchmark]
    public BigInteger Pell_Iterative() => Pell(N);

    [Benchmark]
    public BigInteger Pell_Binet() => PellBinet(N);

    [Benchmark]
    public BigInteger PellLucas_Iterative() => PellLucas(N);

    [Benchmark]
    public BigInteger PellLucas_Binet() => PellLucasBinet(N);
}