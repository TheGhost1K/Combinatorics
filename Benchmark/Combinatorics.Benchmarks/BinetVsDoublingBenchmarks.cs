using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Сравнение точных методов вычисления чисел Фибоначчи, Люка, Пелля.
/// Гипотеза: fast doubling в Z быстрее, чем Бине в Q(√d),
/// так как последний работает с четвёрками (a,b,c,d) и делает НОД на каждом шаге.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class BinetVsDoublingBenchmarks
{
    [Params(10, 50, 100, 500, 1000, 5000)]
    public int N;

    // ==================== ФИБОНАЧЧИ ====================

    [Benchmark(Baseline = true)]
    public BigInteger Fibonacci_Iterative() => Fibonacci(N);

    [Benchmark]
    public BigInteger Fibonacci_FastDoubling() => FibonacciFast(N);

    [Benchmark]
    public BigInteger Fibonacci_Binet() => FibonacciBinet(N);

    // ==================== ЛЮКА ====================

    [Benchmark]
    public BigInteger Lucas_Iterative() => Lucas(N);

    [Benchmark]
    public BigInteger Lucas_Binet() => LucasBinet(N);

    // ==================== ПЕЛЛЬ ====================

    [Benchmark]
    public BigInteger Pell_Iterative() => Pell(N);

    [Benchmark]
    public BigInteger Pell_Binet() => PellBinet(N);

    // ==================== ПЕЛЛЬ–ЛЮКА ====================

    [Benchmark]
    public BigInteger PellLucas_Iterative() => PellLucas(N);

    [Benchmark]
    public BigInteger PellLucas_Binet() => PellLucasBinet(N);
}