using BenchmarkDotNet.Attributes;
using System.Numerics;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Лах, Нараян, Fubini, Моцкин (обычный и обобщённый).
/// Проверяем, как масштабируются разные подходы (рекуррентность vs сумма vs формула).
/// </summary>
[MemoryDiagnoser]
public class LahNarayanaBenchmarks
{
    [Params(10, 30, 60, 100)]
    public int N;

    // ==================== ЛАХ ====================

    [Benchmark(Baseline = true)]
    public BigInteger Lah_Single() => Lah(N, N / 2);

    [Benchmark]
    public BigInteger[] Lah_Row() => LahRow(N);

    [Benchmark]
    public BigInteger OrderedBell_Direct() => OrderedBell(N);

    [Benchmark]
    public BigInteger OrderedBell_ViaLahRow()
    {
        var row = LahRow(N);
        BigInteger s = 0;
        foreach (var v in row) s += v;
        return s;
    }

    // ==================== НАРАЯН ====================

    [Benchmark]
    public BigInteger Narayana_Single() => Narayana(N, N / 2);

    [Benchmark]
    public BigInteger[] Narayana_Row() => NarayanaRow(N);

    // ==================== МОЦКИН ====================

    [Benchmark]
    public BigInteger Motzkin_Formula() => Motzkin(N);

    [Benchmark]
    public BigInteger[] Motzkin_Sequence() => MotzkinSequence(N);

    [Benchmark]
    public BigInteger MotzkinGeneralized_Rec() => MotzkinGeneralized(N, 1);

    [Benchmark]
    public BigInteger MotzkinGeneralized_Sum() => MotzkinGeneralizedBySum(N, 1);

    [Benchmark]
    public BigInteger MotzkinGeneralized2_Rec() => MotzkinGeneralized(N, 2);

    [Benchmark]
    public BigInteger MotzkinGeneralized2_Sum() => MotzkinGeneralizedBySum(N, 2);
}