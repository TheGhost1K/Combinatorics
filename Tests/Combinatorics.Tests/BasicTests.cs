using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;

namespace Combinatorics.Tests
{
    public class BasicTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(5, 120)]
        [InlineData(10, 3628800)]
        [InlineData(20, 2432902008176640000UL)]
        public void Factorial_Works(int n, ulong expected)
            => Assert.Equal(new BigInteger(expected), Factorial(n));

        [Fact]
        public void Factorial_Negative_Throws()
            => Assert.Throws<ArgumentOutOfRangeException>(() => Factorial(-1));

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(4, 2, 6)]
        [InlineData(5, 2, 10)]
        [InlineData(10, 5, 252)]
        [InlineData(52, 5, 2598960)] // покерные руки
        public void Combinations_Works(int n, int k, long expected)
            => Assert.Equal(new BigInteger(expected), Combinations(n, k));

        [Fact]
        public void Combinations_Symmetry()
        {
            for (int n = 0; n <= 30; n++)
                for (int k = 0; k <= n; k++)
                    Assert.Equal(Combinations(n, k), Combinations(n, n - k));
        }

        [Fact]
        public void Combinations_RowSumEquals2PowN()
        {
            for (int n = 0; n <= 20; n++)
            {
                BigInteger sum = 0;
                for (int k = 0; k <= n; k++) sum += Combinations(n, k);
                Assert.Equal(BigInteger.Pow(2, n), sum);
            }
        }

        [Fact]
        public void Combinations_InvalidArgs_Throw()
        {
            Assert.Throws<ArgumentException>(() => Combinations(-1, 0));
            Assert.Throws<ArgumentException>(() => Combinations(5, 6));
            Assert.Throws<ArgumentException>(() => Combinations(5, -1));
        }

        [Theory]
        [InlineData(10, 3, 720)]
        [InlineData(5, 5, 120)]
        [InlineData(5, 0, 1)]
        public void Arrangements_Works(int n, int k, long expected)
            => Assert.Equal(new BigInteger(expected), Arrangements(n, k));

        [Fact]
        public void Arrangements_Identity()
            => Assert.Equal(Factorial(7), Arrangements(7, 7));

        [Theory]
        [InlineData(new[] { 1, 4, 4, 2 }, 34650)] // MISSISSIPPI
        [InlineData(new[] { 2, 2 }, 6)]            // AABB
        [InlineData(new[] { 3 }, 1)]               // AAA
        public void PermutationsWithRepetition_Works(int[] counts, long expected)
            => Assert.Equal(new BigInteger(expected), PermutationsWithRepetition(counts));

        [Fact]
        public void PermutationsWithRepetition_InvalidThrows()
        {
            Assert.Throws<ArgumentException>(() => PermutationsWithRepetition());
            Assert.Throws<ArgumentException>(() => PermutationsWithRepetition(1, -2));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 5)]
        [InlineData(4, 14)]
        [InlineData(5, 42)]
        [InlineData(10, 16796)]
        [InlineData(15, 9694845)]
        public void Catalan_Works(int n, long expected)
            => Assert.Equal(new BigInteger(expected), Catalan(n));

        [Fact]
        public void Catalan_Recurrence()
        {
            for (int n = 0; n <= 15; n++)
            {
                BigInteger sum = 0;
                for (int i = 0; i <= n; i++) sum += Catalan(i) * Catalan(n - i);
                Assert.Equal(Catalan(n + 1), sum);
            }
        }

        // ==================== CATALAN SEQUENCE ====================

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void CatalanSequence_MatchesIndividualCalls(int count)
        {
            var seq = CatalanSequence(count);
            Assert.Equal(count, seq.Length);
            for (int i = 0; i < count; i++)
                Assert.Equal(Catalan(i), seq[i]);
        }

        [Fact]
        public void CatalanSequence_KnownValues()
        {
            // Первые 15 чисел Каталана (OEIS A000108)
            var expected = new BigInteger[]
            {
        1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862,
        16796, 58786, 208012, 742900, 2674440
            };
            var actual = CatalanSequence(expected.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CatalanSequence_Empty_ReturnsEmptyArray()
        {
            var result = CatalanSequence(0);
            Assert.Empty(result);
        }

        [Fact]
        public void CatalanSequence_Single_ReturnsOne()
        {
            var result = CatalanSequence(1);
            Assert.Single(result);
            Assert.Equal(BigInteger.One, result[0]);
        }

        [Fact]
        public void CatalanSequence_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CatalanSequence(-1));
        }

        [Fact]
        public void CatalanSequence_Recurrence()
        {
            // C_{n+1} = C_n * 2*(2n+1) / (n+2) — проверяем на больших n
            const int count = 100;
            var seq = CatalanSequence(count);
            for (int n = 0; n < count - 1; n++)
            {
                BigInteger expected = seq[n] * (2 * (2 * n + 1)) / (n + 2);
                Assert.Equal(expected, seq[n + 1]);
            }
        }

        [Fact]
        public void CatalanSequence_LargeN_Consistency()
        {
            // Проверка на больших n: сравнение с Catalan для выборки индексов
            const int count = 200;
            var seq = CatalanSequence(count);

            // Проверяем ключевые индексы — не все, чтобы не тормозить тест
            foreach (var i in new[] { 0, 1, 2, 5, 10, 50, 100, 150, 199 })
                Assert.Equal(Catalan(i), seq[i]);
        }

        [Fact]
        public void CatalanSequence_AllValuesPositive()
        {
            var seq = CatalanSequence(50);
            foreach (var c in seq)
                Assert.True(c > 0);
        }

        [Fact]
        public void CatalanSequence_MonotonicFromSecond()
        {
            // C_0 = C_1 = 1, дальше строго возрастает
            var seq = CatalanSequence(30);
            for (int i = 2; i < seq.Length; i++)
                Assert.True(seq[i] > seq[i - 1], $"не монотонно на i={i}");
        }


        [Fact]
        public void PascalRow_MatchesCombinations()
        {
            for (int n = 0; n <= 30; n++)
            {
                var row = PascalRow(n);
                for (int k = 0; k <= n; k++)
                    Assert.Equal(Combinations(n, k), row[k]);
            }
        }
    }
}