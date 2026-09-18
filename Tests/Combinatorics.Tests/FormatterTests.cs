using System.Numerics;
using Xunit;
using static Combinatorics.Combinatorics;
using static Combinatorics.CombinatoricsFormatter;

namespace Combinatorics.Tests
{
    public class FormatterTests
    {
        [Fact]
        public void FormatSequence_Works()
    => Assert.Equal("1, 1, 2, 5", FormatSequence(CatalanSequence(4)));

        [Fact]
        public void FormatCompact_Scales()
        {
            Assert.Equal("42", FormatCompact(42));
            Assert.Equal("1.50K", FormatCompact(1500));
            Assert.Equal("2.50M", FormatCompact(2_500_000));
            Assert.Equal("1.00B", FormatCompact(1_000_000_000));
        }

        [Fact]
        public void FormatScientific_Works()
        {
            Assert.Equal("0", FormatScientific(BigInteger.Zero));
            Assert.StartsWith("2.43e18", FormatScientific(Factorial(20)));
        }

        [Fact]
        public void FormatRational_Modes()
        {
            var b6 = Bernoulli(6);
            Assert.Equal("1/42", FormatRational(b6, RationalMode.Fraction));
            Assert.StartsWith("0.023", FormatRational(b6, RationalMode.Decimal));
            Assert.Contains("1/42", FormatRational(b6, RationalMode.Both));
        }

        [Fact]
        public void FormatThousands_Works()
        {
            Assert.Equal("42", FormatThousands(42));
            Assert.Equal("1 234", FormatThousands(1234));
            Assert.Equal("-1 234 567", FormatThousands(-1234567));
        }

        [Fact]
        public void Box_Works()
        {
            var box = Box("hi");
            Assert.Contains("| hi |", box);
            Assert.Contains("+----+", box);
        }

        [Fact]
        public void FormatTriangle_NoException()
        {
            var pascal = Enumerable.Range(0, 5).Select(PascalRow);
            var result = FormatTriangle(pascal);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void FormatColumns_Works()
        {
            var result = FormatColumns(Enumerable.Range(0, 8), columns: 4);
            Assert.Contains("0:", result);
            Assert.Contains("7:", result);
        }
    }
}