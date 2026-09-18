using BenchmarkDotNet.Attributes;

namespace Combinatorics.Benchmarks;

[MemoryDiagnoser]
public class BigRationalBenchmarks
{
    [Params(10, 30, 60)]
    public int N;

    [Benchmark]
    public BigRational Bernoulli_Single()
        => Combinatorics.Bernoulli(N);

    [Benchmark]
    public BigRational[] Bernoulli_Sequence()
        => Combinatorics.BernoulliSequence(N);
}