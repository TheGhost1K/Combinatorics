using BenchmarkDotNet.Attributes;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Сравнение асимптотических формул: базовая vs расширенная.
/// Ожидание: расширенные формулы в 2–3 раза дороже по CPU,
/// но точность на порядки выше. Стоит ли овчинка выделки — решает пользователь.
/// </summary>
[MemoryDiagnoser]
public class AsymptoticsBenchmarks
{
    [Params(10, 100, 1000, 10000)]
    public int N;

    // ==================== ФАКТОРИАЛ ====================

    [Benchmark(Baseline = true)]
    public double Factorial_Stirling() => FactorialStirling(N);

    [Benchmark]
    public double Factorial_StirlingRefined() => FactorialStirlingRefined(N);

    [Benchmark]
    public double Factorial_StirlingExtended() => FactorialStirlingExtended(N);

    // ==================== КАТАЛАН ====================

    [Benchmark]
    public double Catalan_Basic() => CatalanApprox(N);

    [Benchmark]
    public double Catalan_Extended() => CatalanApproxExtended(N);

    // ==================== ФИБОНАЧЧИ ====================

    [Benchmark]
    public double Fibonacci_Basic() => FibonacciApprox(N);

    [Benchmark]
    public double Fibonacci_Extended() => FibonacciApproxExtended(N);

    // ==================== БЕЛЛ ====================

    [Benchmark]
    public double Bell_Basic() => BellApprox(N);

    [Benchmark]
    public double Bell_Extended() => BellApproxExtended(N);
}