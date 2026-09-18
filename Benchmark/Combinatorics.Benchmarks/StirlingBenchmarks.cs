using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace Combinatorics.Benchmarks;

[MemoryDiagnoser]
public class StirlingBenchmarks
{
    [Params(10, 30, 60)]
    public int N;

    [Benchmark]
    public BigInteger StirlingFirstKind()
        => Combinatorics.StirlingFirstKind(N, N / 2);

    [Benchmark]
    public BigInteger[] StirlingFirstKindRow()
        => Combinatorics.StirlingFirstKindRow(N);

    [Benchmark]
    public BigInteger StirlingSecondKind()
        => Combinatorics.StirlingSecondKind(N, N / 2);

    [Benchmark]
    public BigInteger[] StirlingSecondKindRow()
        => Combinatorics.StirlingSecondKindRow(N);

    [Benchmark]
    public BigInteger Bell() => Combinatorics.Bell(N);

    [Benchmark]
    public BigInteger[] BellSequence() => Combinatorics.BellSequence(N);

    [Benchmark]
    public BigInteger Eulerian() => Combinatorics.Eulerian(N, N / 2);

    [Benchmark]
    public BigInteger[] EulerianRow() => Combinatorics.EulerianRow(N);
}