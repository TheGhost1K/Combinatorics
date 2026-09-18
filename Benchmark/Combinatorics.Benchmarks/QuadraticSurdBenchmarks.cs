using BenchmarkDotNet.Attributes;

namespace Combinatorics.Benchmarks;

/// <summary>
/// Микробенчмарки операций в Q(√d): что дороже всего?
/// Ожидание: НОД-нормализация — самая дорогая операция.
/// </summary>
[MemoryDiagnoser]
public class QuadraticSurdBenchmarks
{
    [Params(5, 20, 100)]
    public int Exp;

    private QuadraticSurd _phi;
    private QuadraticSurd _onePlusSqrt2;

    [GlobalSetup]
    public void Setup()
    {
        _phi = new QuadraticSurd(1, 1, 2, 5);
        _onePlusSqrt2 = new QuadraticSurd(1, 1, 1, 2);
    }

    [Benchmark]
    public QuadraticSurd Phi_Pow() => QuadraticSurd.Pow(_phi, Exp);

    [Benchmark]
    public QuadraticSurd OnePlusSqrt2_Pow() => QuadraticSurd.Pow(_onePlusSqrt2, Exp);

    [Benchmark]
    public QuadraticSurd Multiplication()
    {
        var x = _phi;
        var result = x;
        for (int i = 1; i < Exp; i++) result *= x;
        return result;
    }

    [Benchmark]
    public QuadraticSurd Addition()
    {
        var x = _phi;
        var result = x;
        for (int i = 1; i < Exp; i++) result += x;
        return result;
    }
}