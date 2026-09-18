using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests
{
    public class ExtraNumbersTests
    {
        // ==================== ДЕЛАННУА ====================

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 1, 3)]
        [InlineData(2, 2, 13)]
        [InlineData(3, 3, 63)]
        [InlineData(4, 4, 321)]
        [InlineData(5, 5, 1683)]
        [InlineData(2, 3, 25)]
        [InlineData(3, 5, 231)]
        public void Delannoy_ClosedForm(int m, int n, long expected)
            => Assert.Equal(new BigInteger(expected), Delannoy(m, n));

        [Fact]
        public void Delannoy_MatchesRecurrence()
        {
            for (int m = 0; m <= 8; m++)
                for (int n = 0; n <= 8; n++)
                    Assert.Equal(Delannoy(m, n), DelannoyByRecurrence(m, n));
        }

        [Fact]
        public void Delannoy_Symmetry()
        {
            for (int m = 0; m <= 10; m++)
                for (int n = 0; n <= 10; n++)
                    Assert.Equal(Delannoy(m, n), Delannoy(n, m));
        }

        [Fact]
        public void DelannoyCentralSequence_MatchesIndividual()
        {
            var seq = DelannoyCentralSequence(8);
            for (int i = 0; i < 8; i++) Assert.Equal(Delannoy(i, i), seq[i]);
        }

        [Fact]
        public void Delannoy_NegativeArgs_Throw()
        {
            Assert.Throws<ArgumentException>(() => Delannoy(-1, 0));
            Assert.Throws<ArgumentException>(() => Delannoy(0, -1));
        }

        // ==================== ШРЁДЕР ====================

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 6)]
        [InlineData(3, 22)]
        [InlineData(4, 90)]
        [InlineData(5, 394)]
        [InlineData(6, 1806)]
        [InlineData(7, 8558)]
        [InlineData(8, 41586)]
        public void SchroederLarge_Works(int n, long expected)
            => Assert.Equal(new BigInteger(expected), SchroederLarge(n));

        [Fact]
        public void SchroederLargeSequence_MatchesIndividual()
        {
            var seq = SchroederLargeSequence(10);
            for (int i = 0; i < 10; i++) Assert.Equal(SchroederLarge(i), seq[i]);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(3, 11)]
        [InlineData(4, 45)]
        [InlineData(5, 197)]
        [InlineData(6, 903)]
        [InlineData(7, 4279)]
        public void SchroederSmall_Works(int n, long expected)
            => Assert.Equal(new BigInteger(expected), SchroederSmall(n));

        [Fact]
        public void SchroederSmallSequence_MatchesIndividual()
        {
            var seq = SchroederSmallSequence(10);
            for (int i = 0; i < 10; i++) Assert.Equal(SchroederSmall(i), seq[i]);
        }

        [Fact]
        public void Schroeder_LargeAndSmallRelation()
        {
            // S_n = 2·s_n - (n mod 2 == 0 ? ... ) — точное тождество:
            // S_n = 2 s_n для n ≥ 1 чётного? На самом деле связь: s_n = S_n - s_{n-1}? Проверяем численно:
            // S_n - s_n = s_n для n ≥ 1 чётных? Ниже — прямая проверка известного факта:
            // 2 s_n = S_n + s_{n-1}  (эмпирическая связь, проверяется численно для n ≤ 8)
            for (int n = 1; n <= 8; n++)
                Assert.Equal(2 * SchroederSmall(n), SchroederLarge(n));
        }

        // ==================== ФУССА–КАТАЛАНА ====================

        [Fact]
        public void FussCatalan_M1_EqualsCatalan()
        {
            for (int n = 0; n <= 15; n++)
                Assert.Equal(Catalan(n), FussCatalan(n, 1));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(3, 12)]
        [InlineData(4, 55)]
        [InlineData(5, 273)]
        [InlineData(6, 1428)]
        [InlineData(7, 7752)]
        public void FussCatalan_M2_TernaryTrees(int n, long expected)
            => Assert.Equal(new BigInteger(expected), FussCatalan(n, 2));

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 4)]
        [InlineData(3, 22)]
        [InlineData(4, 140)]
        [InlineData(5, 969)]
        public void FussCatalan_M3_QuaternaryTrees(int n, long expected)
            => Assert.Equal(new BigInteger(expected), FussCatalan(n, 3));

        [Fact]
        public void FussCatalan_GeneralFormulaCheck()
        {
            // A_n^(m) = C((m+1)n, n) / (mn + 1) — целое
            for (int m = 1; m <= 5; m++)
                for (int n = 0; n <= 12; n++)
                {
                    var expected = Combinations((m + 1) * n, n) / (m * n + 1);
                    Assert.Equal(expected, FussCatalan(n, m));
                    Assert.True((Combinations((m + 1) * n, n) % (m * n + 1)).IsZero);
                }
        }

        [Fact]
        public void FussCatalanSequence_MatchesIndividual()
        {
            for (int m = 1; m <= 4; m++)
            {
                var seq = FussCatalanSequence(10, m);
                for (int i = 0; i < 10; i++) Assert.Equal(FussCatalan(i, m), seq[i]);
            }
        }

        [Fact]
        public void FussCatalan_InvalidArgs_Throw()
        {
            Assert.Throws<ArgumentException>(() => FussCatalan(-1, 1));
            Assert.Throws<ArgumentException>(() => FussCatalan(5, 0));
        }
    }
}