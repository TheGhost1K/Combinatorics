using System.Numerics;

namespace Combinatorics;

/// <summary>
/// Несократимая рациональная дробь на основе <see cref="BigInteger"/>.
/// Поддерживает арифметику (+, −, ×, ÷), сравнение (==, !=, &lt;, &gt;, &lt;=, &gt;=)
/// и неявные преобразования из <see cref="long"/> и <see cref="BigInteger"/>.
/// </summary>
/// <remarks>
/// <para>
/// Дробь всегда хранится в нормализованной форме:
/// знаменатель положительный, числитель и знаменатель взаимно просты.
/// </para>
/// <para>
/// Структура неизменяемая (<c>readonly struct</c>) — все операции возвращают новый экземпляр.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var half = new BigRational(1, 2);
/// var third = new BigRational(1, 3);
/// var sum = half + third;       // 5/6
/// var cmp = half &gt; third;      // true
/// var b = Combinatorics.Combinatorics.Bernoulli(6); // 1/42
/// </code>
/// </example>
public readonly struct BigRational
    : IEquatable<BigRational>,
      IComparable<BigRational>,
      IComparable
{
    /// <summary>Числитель дроби. Может быть отрицательным.</summary>
    /// <remarks>Знак дроби определяется знаком числителя, так как знаменатель всегда положительный.</remarks>
    public BigInteger Num { get; }

    /// <summary>Знаменатель дроби. Всегда строго положительный.</summary>
    /// <remarks>Никогда не равен нулю. В нормализованной форме взаимно прост с <see cref="Num"/>.</remarks>
    public BigInteger Den { get; }

    /// <summary>Ноль: 0/1.</summary>
    public static readonly BigRational Zero = new(BigInteger.Zero, BigInteger.One);

    /// <summary>Единица: 1/1.</summary>
    public static readonly BigRational One = new(BigInteger.One, BigInteger.One);

    /// <summary>
    /// Создаёт дробь <paramref name="num"/>/<paramref name="den"/> и приводит её к несократимому виду.
    /// </summary>
    /// <param name="num">Числитель. Может быть любым целым.</param>
    /// <param name="den">Знаменатель. Не может быть нулём.</param>
    /// <exception cref="DivideByZeroException">Если <paramref name="den"/> равен нулю.</exception>
    /// <remarks>
    /// <para>
    /// Если знаменатель отрицательный, знак переносится в числитель:
    /// <c>new BigRational(1, -2)</c> даёт <c>-1/2</c>.
    /// </para>
    /// <para>
    /// Дробь сокращается делением на НОД числителя и знаменателя:
    /// <c>new BigRational(4, 8)</c> даёт <c>1/2</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var r1 = new BigRational(4, 8);    // 1/2
    /// var r2 = new BigRational(1, -2);   // -1/2
    /// var r3 = new BigRational(0, 5);    // 0/1
    /// </code>
    /// </example>
    public BigRational(BigInteger num, BigInteger den)
    {
        if (den.IsZero) throw new DivideByZeroException("Знаменатель не может быть нулём");
        if (den.Sign < 0) { num = -num; den = -den; }
        var g = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
        if (g > 1) { num /= g; den /= g; }
        Num = num; Den = den;
    }

    /// <summary>
    /// Создаёт целое число как рациональное: <paramref name="num"/>/1.
    /// </summary>
    /// <param name="num">Целое значение.</param>
    /// <example>
    /// <code>
    /// var five = new BigRational(5); // 5/1
    /// </code>
    /// </example>
    public BigRational(long num) : this(num, 1) { }

    // ==================== АРИФМЕТИКА ====================

    /// <summary>Сложение двух дробей.</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns>Сумма в несократимой форме.</returns>
    /// <example>
    /// <code>
    /// var sum = new BigRational(1, 3) + new BigRational(1, 6); // 1/2
    /// </code>
    /// </example>
    public static BigRational operator +(BigRational a, BigRational b)
    {
        if (a.Den == b.Den)
            return new BigRational(a.Num + b.Num, a.Den);
        return new BigRational(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
    }

    /// <summary>Вычитание двух дробей.</summary>
    /// <param name="a">Уменьшаемое.</param>
    /// <param name="b">Вычитаемое.</param>
    /// <returns>Разность в несократимой форме.</returns>
    public static BigRational operator -(BigRational a, BigRational b)
    {
        if (a.Den == b.Den)
            return new BigRational(a.Num - b.Num, a.Den);
        return new BigRational(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);
    }

    /// <summary>Умножение двух дробей.</summary>
    /// <param name="a">Первый множитель.</param>
    /// <param name="b">Второй множитель.</param>
    /// <returns>Произведение в несократимой форме.</returns>
    public static BigRational operator *(BigRational a, BigRational b)
        => new(a.Num * b.Num, a.Den * b.Den);

    /// <summary>Деление двух дробей.</summary>
    /// <param name="a">Делимое.</param>
    /// <param name="b">Делитель.</param>
    /// <returns>Частное в несократимой форме.</returns>
    /// <exception cref="DivideByZeroException">Если <paramref name="b"/> равен нулю (числитель b равен 0).</exception>
    public static BigRational operator /(BigRational a, BigRational b)
        => new(a.Num * b.Den, a.Den * b.Num);

    /// <summary>Унарный минус: меняет знак дроби.</summary>
    /// <param name="a">Дробь.</param>
    /// <returns>Дробь с противоположным знаком.</returns>
    public static BigRational operator -(BigRational a)
        => new(-a.Num, a.Den);

    /// <summary>Унарный плюс: возвращает дробь без изменений.</summary>
    /// <param name="a">Дробь.</param>
    /// <returns>Та же дробь.</returns>
    public static BigRational operator +(BigRational a) => a;

    // ==================== СРАВНЕНИЕ — ОПЕРАТОРЫ ====================

    /// <summary>Оператор «больше».</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> строго больше <paramref name="b"/>.</returns>
    public static bool operator <(BigRational a, BigRational b)
        => a.Num * b.Den < b.Num * a.Den;

    /// <summary>Оператор «больше».</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> строго больше <paramref name="b"/>.</returns>
    public static bool operator >(BigRational a, BigRational b)
        => a.Num * b.Den > b.Num * a.Den;
    
    /// <summary>Оператор «меньше или равно».</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≤ <paramref name="b"/>.</returns>
    public static bool operator <=(BigRational a, BigRational b)
        => a.Num * b.Den <= b.Num * a.Den;

    /// <summary>Оператор «больше или равно».</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≥ <paramref name="b"/>.</returns>
    public static bool operator >=(BigRational a, BigRational b)
        => a.Num * b.Den >= b.Num * a.Den;

    /// <summary>
    /// Оператор равенства: дроби равны, если совпадают в нормализованной форме.
    /// </summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если дроби равны.</returns>
    /// <remarks>Так как дроби хранятся в несократимой форме, достаточно сравнить числители и знаменатели.</remarks>
    public static bool operator ==(BigRational a, BigRational b)
        => a.Num == b.Num && a.Den == b.Den;

    /// <summary>Оператор неравенства.</summary>
    /// <param name="a">Левая дробь.</param>
    /// <param name="b">Правая дробь.</param>
    /// <returns><c>true</c>, если дроби не равны.</returns>
    public static bool operator !=(BigRational a, BigRational b)
        => !(a == b);

    // ==================== IComparable ====================

    /// <summary>
    /// Сравнивает две дроби.
    /// </summary>
    /// <param name="other">Дробь для сравнения.</param>
    /// <returns>
    /// −1, если текущая меньше <paramref name="other"/>; 0, если равны; +1, если больше.
    /// </returns>
    /// <remarks>
    /// Оптимизировано: использует перекрёстное умножение без создания промежуточных <see cref="BigRational"/>.
    /// </remarks>
    public int CompareTo(BigRational other)
        => (Num * other.Den).CompareTo(other.Num * Den);

    /// <summary>
    /// Не-типизированное сравнение — для совместимости с не-generic коллекциями.
    /// </summary>
    /// <param name="obj">Объект для сравнения; должен быть <see cref="BigRational"/> или <c>null</c>.</param>
    /// <returns>
    /// +1, если <paramref name="obj"/> равен <c>null</c>; иначе результат <see cref="CompareTo(BigRational)"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Если <paramref name="obj"/> не является <see cref="BigRational"/>.</exception>
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is BigRational r) return CompareTo(r);
        throw new ArgumentException($"Объект должен быть {nameof(BigRational)}", nameof(obj));
    }

    // ==================== IEquatable ====================

    /// <summary>
    /// Проверяет равенство с другой дробью.
    /// </summary>
    /// <param name="other">Дробь для сравнения.</param>
    /// <returns><c>true</c>, если дроби равны.</returns>
    public bool Equals(BigRational other) => Num == other.Num && Den == other.Den;

    /// <summary>
    /// Проверяет равенство с произвольным объектом.
    /// </summary>
    /// <param name="o">Объект для сравнения.</param>
    /// <returns><c>true</c>, если <paramref name="o"/> — <see cref="BigRational"/> и равен текущей дроби.</returns>
    public override bool Equals(object? o) => o is BigRational r && Equals(r);

    /// <summary>
    /// Возвращает хеш-код, согласованный с <see cref="Equals(BigRational)"/>.
    /// </summary>
    /// <returns>Хеш-код на основе числителя и знаменателя.</returns>
    public override int GetHashCode() => HashCode.Combine(Num, Den);

    // ==================== ПРЕОБРАЗОВАНИЯ ====================

    /// <summary>Неявное преобразование из <see cref="long"/> в рациональное.</summary>
    /// <param name="v">Целое значение.</param>
    /// <returns>Дробь <c>v/1</c>.</returns>
    public static implicit operator BigRational(long v) => new(v, 1);

    /// <summary>Неявное преобразование из <see cref="BigInteger"/> в рациональное.</summary>
    /// <param name="v">Целое значение.</param>
    /// <returns>Дробь <c>v/1</c>.</returns>
    public static implicit operator BigRational(BigInteger v) => new(v, 1);

    // ==================== ОПЕРАТОРЫ С ПРИМИТИВАМИ ====================

    /// <summary>Сравнение дроби с <see cref="long"/>: меньше.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &lt; <paramref name="b"/>.</returns>
    public static bool operator <(BigRational a, long b) => a < new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="long"/>: больше.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &gt; <paramref name="b"/>.</returns>
    public static bool operator >(BigRational a, long b) => a > new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="long"/>: меньше или равно.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≤ <paramref name="b"/>.</returns>
    public static bool operator <=(BigRational a, long b) => a <= new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="long"/>: больше или равно.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≥ <paramref name="b"/>.</returns>
    public static bool operator >=(BigRational a, long b) => a >= new BigRational(b, 1);

    /// <summary>Сравнение <see cref="long"/> с дробью: меньше.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &lt; <paramref name="b"/>.</returns>
    public static bool operator <(long a, BigRational b) => new BigRational(a, 1) < b;

    /// <summary>Сравнение <see cref="long"/> с дробью: больше.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &gt; <paramref name="b"/>.</returns>
    public static bool operator >(long a, BigRational b) => new BigRational(a, 1) > b;

    /// <summary>Сравнение <see cref="long"/> с дробью: меньше или равно.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≤ <paramref name="b"/>.</returns>
    public static bool operator <=(long a, BigRational b) => new BigRational(a, 1) <= b;

    /// <summary>Сравнение <see cref="long"/> с дробью: больше или равно.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≥ <paramref name="b"/>.</returns>
    public static bool operator >=(long a, BigRational b) => new BigRational(a, 1) >= b;

    /// <summary>Сравнение дроби с <see cref="BigInteger"/>: меньше.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &lt; <paramref name="b"/>.</returns>
    public static bool operator <(BigRational a, BigInteger b) => a < new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="BigInteger"/>: больше.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &gt; <paramref name="b"/>.</returns>
    public static bool operator >(BigRational a, BigInteger b) => a > new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="BigInteger"/>: меньше или равно.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≤ <paramref name="b"/>.</returns>
    public static bool operator <=(BigRational a, BigInteger b) => a <= new BigRational(b, 1);

    /// <summary>Сравнение дроби с <see cref="BigInteger"/>: больше или равно.</summary>
    /// <param name="a">Дробь.</param>
    /// <param name="b">Целое.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≥ <paramref name="b"/>.</returns>
    public static bool operator >=(BigRational a, BigInteger b) => a >= new BigRational(b, 1);

    /// <summary>Сравнение <see cref="BigInteger"/> с дробью: меньше.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &lt; <paramref name="b"/>.</returns>
    public static bool operator <(BigInteger a, BigRational b) => new BigRational(a, 1) < b;

    /// <summary>Сравнение <see cref="BigInteger"/> с дробью: больше.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> &gt; <paramref name="b"/>.</returns>
    public static bool operator >(BigInteger a, BigRational b) => new BigRational(a, 1) > b;

    /// <summary>Сравнение <see cref="BigInteger"/> с дробью: меньше или равно.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≤ <paramref name="b"/>.</returns>
    public static bool operator <=(BigInteger a, BigRational b) => new BigRational(a, 1) <= b;

    /// <summary>Сравнение <see cref="BigInteger"/> с дробью: больше или равно.</summary>
    /// <param name="a">Целое.</param>
    /// <param name="b">Дробь.</param>
    /// <returns><c>true</c>, если <paramref name="a"/> ≥ <paramref name="b"/>.</returns>
    public static bool operator >=(BigInteger a, BigRational b) => new BigRational(a, 1) >= b;

    // ==================== ВЫВОД ====================

    /// <summary>
    /// Возвращает строковое представление дроби.
    /// </summary>
    /// <returns>
    /// Целое число как строку, если знаменатель равен 1 (например, <c>"5"</c>);
    /// иначе — <c>"числитель/знаменатель"</c> (например, <c>"1/2"</c>).
    /// </returns>
    /// <example>
    /// <code>
    /// new BigRational(3, 1).ToString(); // "3"
    /// new BigRational(1, 2).ToString(); // "1/2"
    /// </code>
    /// </example>
    public override string ToString()
        => Den.IsOne ? Num.ToString() : $"{Num}/{Den}";

    /// <summary>
    /// Возвращает десятичное представление с заданной точностью (для отладки/логов).
    /// </summary>
    /// <param name="digits">Число знаков после запятой, ≥ 0.</param>
    /// <returns>Строка вида <c>"1.500000"</c> или <c>"-0.333333"</c>.</returns>
    /// <remarks>
    /// Округление — вниз (отбрасывание), не по правилам. Для точных значений используйте <see cref="ToString"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// new BigRational(3, 2).ToDecimalString(6);   // "1.500000"
    /// new BigRational(1, 3).ToDecimalString(6);   // "0.333333"
    /// new BigRational(-1, 2).ToDecimalString(2);  // "-0.50"
    /// </code>
    /// </example>
    public string ToDecimalString(int digits = 6)
    {
        if (digits < 0) throw new ArgumentOutOfRangeException(nameof(digits), "digits ≥ 0");
        if (Den.IsOne) return Num.ToString();

        var sign = (Num.Sign < 0) ? "-" : "";
        var absNum = BigInteger.Abs(Num);
        var scale = BigInteger.Pow(10, digits);
        var scaled = absNum * scale / Den;
        var intPart = scaled / scale;
        var fracPart = scaled % scale;
        return $"{sign}{intPart}.{fracPart.ToString().PadLeft(digits, '0')}";
    }
}