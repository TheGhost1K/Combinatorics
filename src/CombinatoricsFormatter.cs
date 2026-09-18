using System.Globalization;
using System.Numerics;
using System.Text;

namespace Combinatorics;

/// <summary>
/// Утилиты для красивого форматирования и печати комбинаторных результатов.
/// </summary>
/// <remarks>
/// <para>Все методы возвращают строку или пишут в <see cref="Console"/>. Класс не
/// влияет на основные вычисления и не имеет состояния.</para>
/// <para>Форматирование рассчитано на консольный вывод шириной до 120 символов,
/// но большинство методов адаптируются под содержимое.</para>
/// </remarks>
/// <example>
/// <code>
/// using static Combinatorics.Combinatorics;
/// using static Combinatorics.CombinatoricsFormatter;
///
/// PrintResult("Catalan(10)", Catalan(10));
/// // Catalan(10) = 16796
///
/// Console.WriteLine(FormatSequence(CatalanSequence(10)));
/// // 1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862
///
/// PrintApproxVsExact("F(50)", Fibonacci(50), FibonacciApprox(50));
/// // F(50) = 12586269025  (≈ 1.2586e10, погрешность 0.000%)
/// </code>
/// </example>
public static class CombinatoricsFormatter
{
    // ==================== ПРОСТЫЕ ФОРМАТЫ ====================

    /// <summary>
    /// Форматирует последовательность в строку через запятую.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="sequence">Последовательность чисел.</param>
    /// <param name="separator">Разделитель (по умолчанию <c>", "</c>).</param>
    /// <returns>Строка вида <c>1, 1, 2, 5, 14</c>.</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="sequence"/> равен <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// FormatSequence(CatalanSequence(6));
    /// // "1, 1, 2, 5, 14, 42"
    /// </code>
    /// </example>
    public static string FormatSequence<T>(IEnumerable<T> sequence, string separator = ", ")
    {
        ArgumentNullException.ThrowIfNull(sequence);
        return string.Join(separator, sequence);
    }

    /// <summary>
    /// Форматирует последовательность с индексами: <c>0:1, 1:1, 2:2, …</c>.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="sequence">Последовательность.</param>
    /// <param name="separator">Разделитель между парами.</param>
    /// <returns>Строка с индексами.</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="sequence"/> равен <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// FormatSequenceIndexed(BellSequence(5));
    /// // "0:1, 1:1, 2:2, 3:5, 4:15"
    /// </code>
    /// </example>
    public static string FormatSequenceIndexed<T>(IEnumerable<T> sequence, string separator = ", ")
    {
        ArgumentNullException.ThrowIfNull(sequence);
        var parts = sequence.Select((v, i) => $"{i}:{v}");
        return string.Join(separator, parts);
    }

    // ==================== КОМПАКТНЫЙ ВЫВОД ДЛЯ БОЛЬШИХ ЧИСЕЛ ====================

    /// <summary>
    /// Компактный формат для больших чисел: тысячи, миллионы, миллиарды.
    /// </summary>
    /// <param name="value">Целое число.</param>
    /// <returns>Строка вида <c>"1.23K"</c>, <c>"4.56M"</c>, <c>"7.89B"</c>.</returns>
    /// <remarks>
    /// Для чисел меньше 1000 — обычное представление.
    /// Для очень больших (&gt; 10¹²) — научная нотация.
    /// </remarks>
    /// <example>
    /// <code>
    /// FormatCompact(42);         // "42"
    /// FormatCompact(1500);       // "1.50K"
    /// FormatCompact(2_500_000);  // "2.50M"
    /// FormatCompact(Factorial(20)); // "2.43e18"
    /// </code>
    /// </example>
    public static string FormatCompact(BigInteger value)
    {
        var abs = BigInteger.Abs(value);
        var sign = value.Sign < 0 ? "-" : "";
        var inv = CultureInfo.InvariantCulture;

        if (abs < 1000)
            return value.ToString(inv);
        if (abs < 1_000_000)
            return $"{sign}{((double)abs / 1_000).ToString("F2", inv)}K";
        if (abs < 1_000_000_000)
            return $"{sign}{((double)abs / 1_000_000).ToString("F2", inv)}M";
        if (abs < 1_000_000_000_000)
            return $"{sign}{((double)abs / 1_000_000_000).ToString("F2", inv)}B";

        return FormatScientific(value, 2);
    }

    /// <summary>
    /// Научная нотация с заданной точностью мантиссы.
    /// </summary>
    /// <param name="value">Целое число.</param>
    /// <param name="digits">Число знаков после запятой.</param>
    /// <returns>Строка вида <c>"2.43e18"</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="digits"/> &lt; 0.</exception>
    /// <example>
    /// <code>
    /// FormatScientific(Factorial(20));     // "2.43e18"
    /// FormatScientific(Factorial(20), 5);  // "2.43290e18"
    /// </code>
    /// </example>
    public static string FormatScientific(BigInteger value, int digits = 2)
    {
        if (digits < 0) throw new ArgumentOutOfRangeException(nameof(digits), "digits ≥ 0");
        if (value.IsZero) return "0";
        var inv = CultureInfo.InvariantCulture;

        var sign = value.Sign < 0 ? "-" : "";
        var abs = BigInteger.Abs(value);
        int exponent = abs.ToString(inv).Length - 1;
        double mantissa = (double)abs / Math.Pow(10, exponent);
        return $"{sign}{mantissa.ToString($"F{digits}", inv)}e{exponent.ToString(inv)}";
    }

    /// <summary>
    /// «Умный» формат: компактный для средних, научный для очень больших.
    /// </summary>
    /// <param name="value">Целое число.</param>
    /// <returns>Строка.</returns>
    /// <example>
    /// <code>
    /// FormatSmart(42);            // "42"
    /// FormatSmart(1500);          // "1.50K"
    /// FormatSmart(Factorial(50)); // "3.04e64"
    /// </code>
    /// </example>
    public static string FormatSmart(BigInteger value) => FormatCompact(value);

    // ==================== ДРОБИ ====================

    /// <summary>
    /// Форматирует <see cref="BigRational"/> как дробь и/или десятичное.
    /// </summary>
    /// <param name="value">Рациональное число.</param>
    /// <param name="mode">Режим вывода: <c>Fraction</c>, <c>Decimal</c>, <c>Both</c>.</param>
    /// <param name="digits">Знаков после запятой для десятичного.</param>
    /// <returns>Строка.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="digits"/> &lt; 0.</exception>
    /// <example>
    /// <code>
    /// var b6 = Bernoulli(6); // 1/42
    /// FormatRational(b6);                        // "1/42"
    /// FormatRational(b6, RationalMode.Decimal);  // "0.023810"
    /// FormatRational(b6, RationalMode.Both);     // "1/42 (≈ 0.023810)"
    /// </code>
    /// </example>
    public static string FormatRational(
        BigRational value,
        RationalMode mode = RationalMode.Fraction,
        int digits = 6)
    {
        if (digits < 0) throw new ArgumentOutOfRangeException(nameof(digits), "digits ≥ 0");

        return mode switch
        {
            RationalMode.Fraction => value.ToString(),
            RationalMode.Decimal => value.ToDecimalString(digits),
            RationalMode.Both => $"{value} (≈ {value.ToDecimalString(digits)})",
            _ => value.ToString()
        };
    }

    // ==================== ТАБЛИЦА ====================

    /// <summary>
    /// Форматирует таблицу «индекс → значение» с выравниванием по правому краю.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="sequence">Последовательность.</param>
    /// <param name="valueFormatter">Функция форматирования значения (по умолчанию <c>ToString</c>).</param>
    /// <param name="columnWidth">Ширина колонки значений.</param>
    /// <returns>Многострочная строка.</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="sequence"/> равен <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// Console.WriteLine(FormatTable(BellSequence(6)));
    /// // +------+-----------------+
    /// // |  n   |     B(n)        |
    /// // +------+-----------------+
    /// // |   0  |               1 |
    /// // |   1  |               1 |
    /// // ...
    /// </code>
    /// </example>
    public static string FormatTable<T>(
        IEnumerable<T> sequence,
        Func<T, string>? valueFormatter = null,
        int columnWidth = 20)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        valueFormatter ??= v => v?.ToString() ?? "";

        var items = sequence.ToList();
        var sb = new StringBuilder();
        int idxWidth = Math.Max(2, items.Count.ToString().Length);

        var border = "+" + new string('-', idxWidth + 2)
                   + "+" + new string('-', columnWidth + 2) + "+";

        sb.AppendLine(border);
        sb.AppendLine("| " + "n".PadRight(idxWidth) + " | " + "значение".PadRight(columnWidth) + " |");
        sb.AppendLine(border);

        for (int i = 0; i < items.Count; i++)
        {
            var val = valueFormatter(items[i]);
            sb.AppendLine("| " + i.ToString().PadLeft(idxWidth)
                        + " | " + val.PadRight(columnWidth) + " |");
        }

        sb.AppendLine(border);
        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Форматирует таблицу с двумя колонками значений (например, точное и приближённое).
    /// </summary>
    /// <typeparam name="T1">Тип первого значения.</typeparam>
    /// <typeparam name="T2">Тип второго значения.</typeparam>
    /// <param name="first">Первая последовательность (точная).</param>
    /// <param name="second">Вторая последовательность (приближение).</param>
    /// <param name="headerFirst">Заголовок первой колонки.</param>
    /// <param name="headerSecond">Заголовок второй колонки.</param>
    /// <param name="columnWidth">Ширина каждой колонки.</param>
    /// <returns>Многострочная строка.</returns>
    /// <example>
    /// <code>
    /// var exact = Enumerable.Range(0, 10).Select(Catalan).ToArray();
    /// var approx = Enumerable.Range(0, 10).Select(n => (BigInteger)Math.Round(CatalanApprox(n))).ToArray();
    /// Console.WriteLine(FormatTableDouble(exact, approx, "C(n) точное", "C(n) ≈"));
    /// </code>
    /// </example>
    public static string FormatTableDouble<T1, T2>(
        IEnumerable<T1> first,
        IEnumerable<T2> second,
        string headerFirst = "точное",
        string headerSecond = "приближённое",
        int columnWidth = 25)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        var list1 = first.ToList();
        var list2 = second.ToList();
        int rows = Math.Min(list1.Count, list2.Count);

        var sb = new StringBuilder();
        int idxWidth = Math.Max(2, rows.ToString().Length);

        var border = "+" + new string('-', idxWidth + 2)
                   + "+" + new string('-', columnWidth + 2)
                   + "+" + new string('-', columnWidth + 2) + "+";

        sb.AppendLine(border);
        sb.AppendLine("| " + "n".PadRight(idxWidth)
                    + " | " + headerFirst.PadRight(columnWidth)
                    + " | " + headerSecond.PadRight(columnWidth) + " |");
        sb.AppendLine(border);

        for (int i = 0; i < rows; i++)
        {
            string v1 = list1[i]?.ToString() ?? "";
            string v2 = list2[i]?.ToString() ?? "";
            sb.AppendLine("| " + i.ToString().PadLeft(idxWidth)
                        + " | " + v1.PadRight(columnWidth)
                        + " | " + v2.PadRight(columnWidth) + " |");
        }
        sb.AppendLine(border);
        return sb.ToString().TrimEnd();
    }

    // ==================== ТРЕУГОЛЬНИК ====================

    /// <summary>
    /// Форматирует треугольную таблицу (Паскаль, Стирлинг, Эйлер) с центрированием.
    /// </summary>
    /// <param name="rows">Строки треугольника.</param>
    /// <param name="width">Ширина колонки (по умолчанию 6).</param>
    /// <returns>Многострочная строка.</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="rows"/> равен <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// var triangle = new[]
    /// {
    ///     PascalRow(0), PascalRow(1), PascalRow(2), PascalRow(3)
    /// };
    /// Console.WriteLine(FormatTriangle(triangle));
    /// //             1
    /// //           1   1
    /// //         1   2   1
    /// //       1   3   3   1
    /// </code>
    /// </example>
    public static string FormatTriangle(
        IEnumerable<IEnumerable<BigInteger>> rows,
        int width = 6)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var list = rows.Select(r => r.ToArray()).ToArray();
        if (list.Length == 0) return "";

        int maxCols = list.Max(r => r.Length);
        var sb = new StringBuilder();

        for (int i = 0; i < list.Length; i++)
        {
            int leadingSpaces = (maxCols - list[i].Length) * (width / 2);
            sb.Append(new string(' ', leadingSpaces));
            for (int j = 0; j < list[i].Length; j++)
            {
                sb.Append(list[i][j].ToString().PadLeft(width));
            }
            sb.AppendLine();
        }
        return sb.ToString().TrimEnd();
    }

    // ==================== КОЛОНКИ ====================

    /// <summary>
    /// Печатает последовательность в несколько колонок.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="sequence">Последовательность.</param>
    /// <param name="columns">Число колонок (по умолчанию 4).</param>
    /// <param name="columnWidth">Ширина колонки (по умолчанию 20).</param>
    /// <returns>Многострочная строка.</returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="sequence"/> равен <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="columns"/> &lt; 1.</exception>
    public static string FormatColumns<T>(
        IEnumerable<T> sequence,
        int columns = 4,
        int columnWidth = 20)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        if (columns < 1) throw new ArgumentOutOfRangeException(nameof(columns), "columns ≥ 1");

        var items = sequence.ToList();
        int rows = (int)Math.Ceiling((double)items.Count / columns);
        var sb = new StringBuilder();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                int idx = r + c * rows;
                if (idx < items.Count)
                {
                    sb.Append($"{idx}:{items[idx]}".PadRight(columnWidth));
                }
            }
            sb.AppendLine();
        }
        return sb.ToString().TrimEnd();
    }

    // ==================== РАМКА ====================

    /// <summary>
    /// Оборачивает текст в рамку.
    /// </summary>
    /// <param name="text">Текст (может быть многострочным).</param>
    /// <param name="padding">Отступ внутри рамки.</param>
    /// <returns>Многострочная строка с рамкой.</returns>
    /// <example>
    /// <code>
    /// Console.WriteLine(Box("Catalan(10) = 16796"));
    /// // +---------------------+
    /// // | Catalan(10) = 16796 |
    /// // +---------------------+
    /// </code>
    /// </example>
    public static string Box(string text, int padding = 1)
    {
        ArgumentNullException.ThrowIfNull(text);
        if (padding < 0) throw new ArgumentOutOfRangeException(nameof(padding), "padding ≥ 0");

        var lines = text.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();
        int width = lines.Max(l => l.Length) + 2 * padding;
        var pad = new string(' ', padding);
        var border = "+" + new string('-', width) + "+";

        var sb = new StringBuilder();
        sb.AppendLine(border);
        foreach (var line in lines)
        {
            sb.AppendLine($"|{pad}{line.PadRight(width - 2 * padding)}{pad}|");
        }
        sb.AppendLine(border);
        return sb.ToString().TrimEnd();
    }

    // ==================== ПЕЧАТЬ В КОНСОЛЬ ====================

    /// <summary>
    /// Печатает результат в формате <c>label = value</c>.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="label">Метка (например, <c>"Catalan(10)"</c>).</param>
    /// <param name="value">Значение.</param>
    /// <example>
    /// <code>
    /// PrintResult("Catalan(10)", Catalan(10));
    /// // Catalan(10) = 16796
    /// </code>
    /// </example>
    public static void PrintResult<T>(string label, T value)
        => Console.WriteLine($"{label} = {value}");

    /// <summary>
    /// Печатает результат в компактном формате.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="value">Значение.</param>
    /// <example>
    /// <code>
    /// PrintCompact("20!", Factorial(20));
    /// // 20! = 2.43e18
    /// </code>
    /// </example>
    public static void PrintCompact(string label, BigInteger value)
        => Console.WriteLine($"{label} = {FormatCompact(value)}");

    /// <summary>
    /// Печатает результат в научной нотации.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="value">Значение.</param>
    /// <param name="digits">Точность мантиссы.</param>
    public static void PrintScientific(string label, BigInteger value, int digits = 2)
        => Console.WriteLine($"{label} = {FormatScientific(value, digits)}");

    /// <summary>
    /// Печатает рациональное число как дробь и десятичное.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="value">Значение.</param>
    /// <param name="mode">Режим вывода.</param>
    /// <param name="digits">Точность десятичного.</param>
    /// <example>
    /// <code>
    /// PrintRational("B(6)", Bernoulli(6), RationalMode.Both);
    /// // B(6) = 1/42 (≈ 0.023810)
    /// </code>
    /// </example>
    public static void PrintRational(
        string label,
        BigRational value,
        RationalMode mode = RationalMode.Both,
        int digits = 6)
        => Console.WriteLine($"{label} = {FormatRational(value, mode, digits)}");

    /// <summary>
    /// Печатает последовательность.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="label">Метка.</param>
    /// <param name="sequence">Последовательность.</param>
    /// <param name="separator">Разделитель.</param>
    /// <example>
    /// <code>
    /// PrintSequence("Bell", BellSequence(11));
    /// // Bell: 1, 1, 2, 5, 15, 52, 203, 877, 4140, 21147, 115975
    /// </code>
    /// </example>
    public static void PrintSequence<T>(string label, IEnumerable<T> sequence, string separator = ", ")
        => Console.WriteLine($"{label}: {FormatSequence(sequence, separator)}");

    /// <summary>
    /// Печатает таблицу «n → значение».
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="label">Заголовок.</param>
    /// <param name="sequence">Последовательность.</param>
    public static void PrintTable<T>(string label, IEnumerable<T> sequence)
    {
        Console.WriteLine(label);
        Console.WriteLine(FormatTable(sequence));
    }

    /// <summary>
    /// Печатает сравнение точного и приближённого значения с оценкой погрешности.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="exact">Точное значение.</param>
    /// <param name="approx">Приближённое значение.</param>
    /// <param name="precision">Знаков после запятой в выводе приближения.</param>
    /// <example>
    /// <code>
    /// PrintApproxVsExact("F(50)", Fibonacci(50), FibonacciApprox(50));
    /// // F(50) = 12586269025  (≈ 1.2586e10, погрешность 0.000%)
    /// </code>
    /// </example>
    public static void PrintApproxVsExact(
        string label,
        BigInteger exact,
        double approx,
        int precision = 4)
    {
        double err = Combinatorics.RelativeError(exact, approx);
        string approxStr = exact > 1_000_000
            ? FormatScientific(exact, precision)
            : approx.ToString($"N{precision}");

        Console.WriteLine($"{label} = {exact}  (≈ {approxStr}, погрешность {err:P4})");
    }

    /// <summary>
    /// Печатает заголовок секции.
    /// </summary>
    /// <param name="title">Заголовок.</param>
    /// <param name="width">Ширина линии (по умолчанию 60).</param>
    public static void PrintHeader(string title, int width = 60)
    {
        var line = new string('=', width);
        Console.WriteLine();
        Console.WriteLine(line);
        Console.WriteLine($"  {title}");
        Console.WriteLine(line);
    }

    /// <summary>
    /// Печатает подзаголовок.
    /// </summary>
    /// <param name="title">Подзаголовок.</param>
    /// <param name="width">Ширина линии (по умолчанию 60).</param>
    public static void PrintSubheader(string title, int width = 60)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine(new string('-', width));
    }

    // ==================== ВСПОМОГАТЕЛЬНОЕ ====================

    /// <summary>
    /// Форматирует целое число с разделителями тысяч: <c>1 234 567</c>.
    /// </summary>
    /// <param name="value">Целое число.</param>
    /// <param name="separator">Разделитель (по умолчанию пробел).</param>
    /// <returns>Строка.</returns>
    /// <example>
    /// <code>
    /// FormatThousands(1234567); // "1 234 567"
    /// FormatThousands(42);      // "42"
    /// </code>
    /// </example>
    public static string FormatThousands(BigInteger value, string separator = " ")
    {
        var s = BigInteger.Abs(value).ToString();
        var sign = value.Sign < 0 ? "-" : "";
        var sb = new StringBuilder();

        for (int i = 0; i < s.Length; i++)
        {
            if (i > 0 && (s.Length - i) % 3 == 0) sb.Append(separator);
            sb.Append(s[i]);
        }
        return sign + sb.ToString();
    }

    /// <summary>
    /// Форматирует длительность в человекочитаемый вид.
    /// </summary>
    /// <param name="duration">Длительность.</param>
    /// <returns>Строка вида <c>"1.23 ms"</c>.</returns>
    public static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalMilliseconds < 1) return $"{duration.TotalMicroseconds:F1} µs";
        if (duration.TotalSeconds < 1) return $"{duration.TotalMilliseconds:F2} ms";
        if (duration.TotalMinutes < 1) return $"{duration.TotalSeconds:F2} s";
        return $"{duration.TotalMinutes:F2} min";
    }
}

/// <summary>
/// Режим вывода рационального числа.
/// </summary>
public enum RationalMode
{
    /// <summary>Только как дробь: <c>1/42</c>.</summary>
    Fraction,

    /// <summary>Только как десятичное: <c>0.023810</c>.</summary>
    Decimal,

    /// <summary>Дробь и десятичное: <c>1/42 (≈ 0.023810)</c>.</summary>
    Both
}