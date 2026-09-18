namespace Combinatorics
{
    /// <summary>
    /// Приближённые (double) математические константы.
    /// Вынесены отдельно, чтобы не конфликтовать с точными QuadraticSurd-свойствами
    /// Phi, Psi, Sqrt5 из <c>Combinatorics.Binet.cs</c>.
    /// </summary>
    internal static class MathConstants
    {
        /// <summary>Золотое сечение <c>φ = (1 + √5)/2 ≈ 1.618…</c>.</summary>
        internal static readonly double Phi = (1.0 + Math.Sqrt(5.0)) / 2.0;

        /// <summary>Сопряжение <c>ψ = (1 − √5)/2 ≈ −0.618…</c>.</summary>
        internal static readonly double Psi = (1.0 - Math.Sqrt(5.0)) / 2.0;

        /// <summary>Квадратный корень из 2.</summary>
        internal static readonly double Sqrt2 = Math.Sqrt(2.0);

        /// <summary>Квадратный корень из 5.</summary>
        internal static readonly double Sqrt5 = Math.Sqrt(5.0);

        /// <summary>Квадратный корень из π.</summary>
        internal static readonly double SqrtPi = Math.Sqrt(Math.PI);

        /// <summary>Основание натурального логарифма e.</summary>
        internal static readonly double E = Math.E;

        /// <summary>Серебряное сечение <c>1 + √2 ≈ 2.414…</c>.</summary>
        internal static readonly double SilverRatio = 1.0 + Sqrt2;

        /// <summary>Константа <c>3 + 2√2 ≈ 5.828…</c>, асимптотическая база Шрёдера и Деланнуа.</summary>
        internal static readonly double CatalConst = 3.0 + 2.0 * Sqrt2;
    }
}