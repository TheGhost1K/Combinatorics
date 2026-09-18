using System.Numerics;

namespace Combinatorics;

public static partial class Combinatorics
{
    // ==================== ФИБОНАЧЧИ / ЛЮКА ЧЕРЕЗ БИНЕ ====================

    /// <summary>
    /// Золотое сечение <c>φ = (1 + √5)/2</c> в точной арифметике поля Q(√5).
    /// </summary>
    /// <remarks>
    /// Используется в формуле Бине для чисел Фибоначчи и Люка.
    /// Удовлетворяет <c>φ² = φ + 1</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// var phi2 = QuadraticSurd.Pow(Phi, 2); // (3 + √5)/2 = φ + 1
    /// </code>
    /// </example>
    /// <seealso cref="Psi"/>
    /// <seealso cref="FibonacciBinet(int)"/>
    public static QuadraticSurd Phi => new(1, 1, 2, 5);

    /// <summary>
    /// Сопряжение золотого сечения: <c>ψ = (1 − √5)/2</c>.
    /// </summary>
    /// <remarks>
    /// <c>ψ = −1/φ</c>, <c>|ψ| &lt; 1</c>. Используется в формуле Бине.
    /// </remarks>
    public static QuadraticSurd Psi => Phi.Conjugate;

    /// <summary>Квадратный корень из 5 в точной арифметике Q(√5).</summary>
    public static QuadraticSurd Sqrt5 => QuadraticSurd.Sqrt(5);

    /// <summary>
    /// n-е число Фибоначчи через формулу Бине: <c>F(n) = (φ^n − ψ^n) / √5</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение F(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <remarks>
    /// <para>Вычисляется за O(log n) операций в поле Q(√5). На практике
    /// <see cref="FibonacciFast(int)"/> быстрее, так как работает только с целыми числами.</para>
    /// <para>Формула Бине полезна для теоретических выкладок и как «эталон» при тестировании.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var f50 = FibonacciBinet(50); // 12586269025
    /// </code>
    /// </example>
    /// <seealso cref="Fibonacci(int)"/>
    /// <seealso cref="FibonacciFast(int)"/>
    public static BigInteger FibonacciBinet(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var phi_n = QuadraticSurd.Pow(Phi, n);
        var psi_n = QuadraticSurd.Pow(Psi, n);
        var result = (phi_n - psi_n) / Sqrt5;
        return result.ToRational().Num;
    }

    /// <summary>
    /// n-е число Люка через формулу Бине: <c>L(n) = φ^n + ψ^n</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение L(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    public static BigInteger LucasBinet(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var phi_n = QuadraticSurd.Pow(Phi, n);
        var psi_n = QuadraticSurd.Pow(Psi, n);
        var result = phi_n + psi_n;
        return result.ToRational().Num;
    }

    /// <summary>Число <c>1 + √2</c> в точной арифметике Q(√2).</summary>
    /// <remarks>«Серебряное сечение». Удовлетворяет <c>(1+√2)² = 3 + 2√2</c>.</remarks>
    public static QuadraticSurd OnePlusSqrt2 => new(1, 1, 1, 2);

    /// <summary>Число <c>1 − √2</c> — сопряжение <see cref="OnePlusSqrt2"/>.</summary>
    /// <remarks><c>|1 − √2| &lt; 1</c>, используется в формуле Бине для Пелля.</remarks>
    public static QuadraticSurd OneMinusSqrt2 => OnePlusSqrt2.Conjugate;

    /// <summary>Число <c>2√2</c> в Q(√2).</summary>
    public static QuadraticSurd TwoSqrt2 => new(0, 2, 1, 2);

    /// <summary>
    /// n-е число Пелля через формулу Бине:
    /// <c>P(n) = ((1+√2)^n − (1−√2)^n) / (2√2)</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение P(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <seealso cref="Pell(int)"/>
    public static BigInteger PellBinet(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var a = QuadraticSurd.Pow(OnePlusSqrt2, n);
        var b = QuadraticSurd.Pow(OneMinusSqrt2, n);
        return ((a - b) / TwoSqrt2).ToRational().Num;
    }

    /// <summary>
    /// n-е число Пелля–Люка через формулу Бине:
    /// <c>Q(n) = (1+√2)^n + (1−√2)^n</c>.
    /// </summary>
    /// <param name="n">Неотрицательное целое.</param>
    /// <returns>Значение Q(n).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="n"/> &lt; 0.</exception>
    /// <seealso cref="PellLucas(int)"/>
    public static BigInteger PellLucasBinet(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var a = QuadraticSurd.Pow(OnePlusSqrt2, n);
        var b = QuadraticSurd.Pow(OneMinusSqrt2, n);
        return (a + b).ToRational().Num;
    }
}