using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace Combinatorics.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class BasicBenchmarks
{
    [Params(10, 20, 50, 100)]
    public int N;

    [Benchmark]
    public BigInteger Factorial() => Combinatorics.Factorial(N);

    [Benchmark]
    public BigInteger Combinations_HalfN() => Combinatorics.Combinations(N, N / 2);

    [Benchmark]
    public BigInteger Catalan() => Combinatorics.Catalan(N);

    [Benchmark]
    public BigInteger[] PascalRow() => Combinatorics.PascalRow(N);

    [Benchmark]
    public BigInteger PermutationsWithRepetition()
        => Combinatorics.PermutationsWithRepetition(N / 3, N / 3, N / 3);
}