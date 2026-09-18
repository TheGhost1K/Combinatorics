using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Сравнение одиночного вычисления и построения всей строки.
/// Гипотеза: построение строки в O(n·k) быстрее, чем n отдельных вызовов,
/// так как исключает повторное построение промежуточных состояний.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class StirlingEulerBenchmarks
{
    [Params(10, 30, 60, 100)]
    public int N;

    // ==================== СТИРЛИНГ 1-ГО РОДА ====================

    [Benchmark(Baseline = true)]
    public BigInteger StirlingFirst_Single() => StirlingFirstKind(N, N / 2);

    [Benchmark]
    public BigInteger[] StirlingFirst_Row() => StirlingFirstKindRow(N);

    [Benchmark]
    public BigInteger StirlingFirst_SumViaRow()
    {
        var row = StirlingFirstKindRow(N);
        BigInteger s = 0;
        foreach (var v in row) s += v;
        return s;
    }

    // ==================== СТИРЛИНГ 2-ГО РОДА ====================

    [Benchmark]
    public BigInteger StirlingSecond_Single() => StirlingSecondKind(N, N / 2);

    [Benchmark]
    public BigInteger[] StirlingSecond_Row() => StirlingSecondKindRow(N);

    [Benchmark]
    public BigInteger Bell_ViaRow()
    {
        var row = StirlingSecondKindRow(N);
        BigInteger s = 0;
        foreach (var v in row) s += v;
        return s;
    }

    // ==================== БЕЛЛ ====================

    [Benchmark]
    public BigInteger Bell_Direct() => Bell(N);

    [Benchmark]
    public BigInteger[] Bell_Sequence() => BellSequence(N);

    // ==================== ЭЙЛЕРА ====================

    [Benchmark]
    public BigInteger Eulerian_Single() => Eulerian(N, N / 2);

    [Benchmark]
    public BigInteger[] Eulerian_Row() => EulerianRow(N);
}