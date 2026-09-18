using System.Numerics;

namespace Combinatorics;

/// <summary>
/// Точное число вида <c>(a + b·√d) / c</c>, где a, b, c, d — целые, c &gt; 0, d &gt; 0.
/// Используется для формул Бине в точной арифметике.
/// </summary>
/// <remarks>
/// <para>
/// Хранится в нормализованной форме: <c>gcd(a, b, c) = 1</c>, <c>c &gt; 0</c>.
/// </para>
/// <para>
/// Поддерживает сложение, вычитание, умножение, деление, возведение в степень
/// и сопряжение. Все операции возвращают нормализованный результат.
/// </para>
/// <para>
/// Структура неизменяемая (<c>readonly struct</c>) — все операции возвращают новый экземпляр.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var phi = new QuadraticSurd(1, 1, 2, 5);          // (1 + √5)/2
/// var phi2 = QuadraticSurd.Pow(phi, 2);             // (3 + √5)/2
/// var product = phi * phi.Conjugate;                // -1
/// </code>
/// </example>
public readonly struct QuadraticSurd : IEquatable<QuadraticSurd>
{
    /// <summary>Числитель рациональной части (коэффициент при 1).</summary>
    /// <remarks>Может быть отрицательным. В нормализованной форме взаимно прост с <see cref="B"/> и <see cref="C"/>.</remarks>
    public BigInteger A { get; }

    /// <summary>Числитель иррациональной части (коэффициент при √d).</summary>
    /// <remarks>Может быть отрицательным. Равен нулю, если число рационально — см. <see cref="IsRational"/>.</remarks>
    public BigInteger B { get; }

    /// <summary>Знаменатель. Всегда строго положительный.</summary>
    /// <remarks>Никогда не равен нулю. В нормализованной форме взаимно прост с <see cref="A"/> и <see cref="B"/>.</remarks>
    public BigInteger C { get; }

    /// <summary>Подкоренное выражение. Всегда строго положительное.</summary>
    /// <remarks>
    /// Обычно свободно от квадратов (2, 3, 5, 6, 7, ...), но это не проверяется автоматически.
    /// Все арифметические операции требуют совпадения <c>D</c> у операндов.
    /// </remarks>
    public BigInteger D { get; }

    /// <summary>
    /// Создаёт число <c>(a + b·√d) / c</c> и приводит его к нормализованной форме.
    /// </summary>
    /// <param name="a">Числитель рациональной части.</param>
    /// <param name="b">Числитель иррациональной части.</param>
    /// <param name="c">Знаменатель. Не может быть нулём.</param>
    /// <param name="d">Подкоренное выражение. Должно быть положительным.</param>
    /// <exception cref="DivideByZeroException">Если <paramref name="c"/> равен нулю.</exception>
    /// <exception cref="ArgumentException">Если <paramref name="d"/> ≤ 0.</exception>
    /// <remarks>
    /// <para>
    /// Если <paramref name="c"/> отрицательный, все три числа (a, b, c) меняют знак.
    /// </para>
    /// <para>
    /// Затем все три числа делятся на их НОД, чтобы получить несократимую форму.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var x = new QuadraticSurd(2, 2, 4, 5);   // нормализуется к (1 + √5)/2
    /// var y = new QuadraticSurd(1, 1, -2, 5);  // нормализуется к (-1 - √5)/2
    /// </code>
    /// </example>
    public QuadraticSurd(BigInteger a, BigInteger b, BigInteger c, BigInteger d)
    {
        if (c.IsZero) throw new DivideByZeroException("c ≠ 0");
        if (d.Sign <= 0) throw new ArgumentException("d > 0");
        if (c.Sign < 0) { a = -a; b = -b; c = -c; }

        // Нормализация: делим на gcd(a, b, c)
        var g = BigInteger.GreatestCommonDivisor(
            BigInteger.GreatestCommonDivisor(BigInteger.Abs(a), BigInteger.Abs(b)), c);
        if (g > 1) { a /= g; b /= g; c /= g; }

        A = a; B = b; C = c; D = d;
    }

    /// <summary>Простой конструктор для целых чисел: a + 0·√d.</summary>
    public static QuadraticSurd FromInteger(BigInteger value, BigInteger d)
        => new(value, BigInteger.Zero, BigInteger.One, d);

    /// <summary>Конструктор √d: 0 + 1·√d.</summary>
    public static QuadraticSurd Sqrt(BigInteger d)
        => new(BigInteger.Zero, BigInteger.One, BigInteger.One, d);

    /// <summary>Проверка: является ли число рациональным (b = 0).</summary>
    public bool IsRational => B.IsZero;

    /// <summary>Если рациональное — вернуть как BigRational. Иначе бросить.</summary>
    public BigRational ToRational()
    {
        if (!IsRational) throw new InvalidOperationException("Число не рационально");
        return new BigRational(A, C);
    }

    /// <summary>Сопряжённое: a − b·√d (с тем же знаменателем).</summary>
    public QuadraticSurd Conjugate => new(A, -B, C, D);

    /// <summary>Норма: a² − d·b², делённая на c². Возвращает (N, c²) как BigRational.</summary>
    public BigRational Norm
    {
        get
        {
            BigInteger n = A * A - D * B * B;
            return new BigRational(n, C * C);
        }
    }

    // ==================== АРИФМЕТИКА ====================

    /// <summary>Сложение двух чисел с одинаковым подкоренным выражением.</summary>
    /// <param name="x">Левое слагаемое.</param>
    /// <param name="y">Правое слагаемое.</param>
    /// <returns>Сумма в нормализованной форме.</returns>
    /// <exception cref="ArgumentException">Если <c>x.D ≠ y.D</c>.</exception>
    public static QuadraticSurd operator +(QuadraticSurd x, QuadraticSurd y)
    {
        CheckSameD(x, y);
        return new QuadraticSurd(
            x.A * y.C + y.A * x.C,
            x.B * y.C + y.B * x.C,
            x.C * y.C,
            x.D);
    }

    /// <summary>Вычитание двух чисел с одинаковым подкоренным выражением.</summary>
    /// <param name="x">Уменьшаемое.</param>
    /// <param name="y">Вычитаемое.</param>
    /// <returns>Разность в нормализованной форме.</returns>
    /// <exception cref="ArgumentException">Если <c>x.D ≠ y.D</c>.</exception>
    public static QuadraticSurd operator -(QuadraticSurd x, QuadraticSurd y)
    {
        CheckSameD(x, y);
        return new QuadraticSurd(
            x.A * y.C - y.A * x.C,
            x.B * y.C - y.B * x.C,
            x.C * y.C,
            x.D);
    }

    /// <summary>
    /// Умножение двух чисел с одинаковым подкоренным выражением.
    /// </summary>
    /// <param name="x">Первый множитель.</param>
    /// <param name="y">Второй множитель.</param>
    /// <returns>Произведение в нормализованной форме.</returns>
    /// <exception cref="ArgumentException">Если <c>x.D ≠ y.D</c>.</exception>
    /// <remarks>
    /// Использует формулу <c>(a₁ + b₁√d)(a₂ + b₂√d) = (a₁a₂ + b₁b₂·d) + (a₁b₂ + a₂b₁)√d</c>.
    /// </remarks>
    public static QuadraticSurd operator *(QuadraticSurd x, QuadraticSurd y)
    {
        CheckSameD(x, y);
        return new QuadraticSurd(
            x.A * y.A + x.B * y.B * x.D,
            x.A * y.B + x.B * y.A,
            x.C * y.C,
            x.D);
    }

    /// <summary>
    /// Деление двух чисел с одинаковым подкоренным выражением.
    /// </summary>
    /// <param name="x">Делимое.</param>
    /// <param name="y">Делитель.</param>
    /// <returns>Частное в нормализованной форме.</returns>
    /// <exception cref="ArgumentException">Если <c>x.D ≠ y.D</c>.</exception>
    /// <exception cref="DivideByZeroException">Если норма делителя равна нулю.</exception>
    /// <remarks>
    /// Использует умножение на сопряжённое: <c>x/y = x·conj(y) / N(y)</c>.
    /// </remarks>
    public static QuadraticSurd operator /(QuadraticSurd x, QuadraticSurd y)
    {
        CheckSameD(x, y);
        var num = x * y.Conjugate;
        BigInteger normNum = y.A * y.A - y.D * y.B * y.B;
        if (normNum.IsZero) throw new DivideByZeroException("Деление на элемент с нулевой нормой");
        BigInteger newC = num.C * normNum;
        return new QuadraticSurd(num.A, num.B, newC, x.D);
    }

    /// <summary>Унарный минус: меняет знаки рациональной и иррациональной частей.</summary>
    /// <param name="x">Число.</param>
    /// <returns>Число с противоположным знаком.</returns>
    public static QuadraticSurd operator -(QuadraticSurd x) => new(-x.A, -x.B, x.C, x.D);

    /// <summary>Унарный плюс: возвращает число без изменений.</summary>
    /// <param name="x">Число.</param>
    /// <returns>То же число.</returns>
    public static QuadraticSurd operator +(QuadraticSurd x) => x;

    /// <summary>
    /// Возводит число в степень <paramref name="n"/> за O(log |n|) умножений.
    /// </summary>
    /// <param name="x">Основание.</param>
    /// <param name="n">Показатель степени. Может быть отрицательным.</param>
    /// <returns><c>x^n</c> в нормализованной форме.</returns>
    /// <remarks>
    /// Для отрицательных <paramref name="n"/> вычисляется <c>1 / x^|n|</c>.
    /// Требует, чтобы норма <paramref name="x"/> была ненулевой.
    /// </remarks>
    /// <example>
    /// <code>
    /// var phi = new QuadraticSurd(1, 1, 2, 5);
    /// var phi10 = QuadraticSurd.Pow(phi, 10);
    /// var phiInv = QuadraticSurd.Pow(phi, -1); // = φ - 1
    /// </code>
    /// </example>
    public static QuadraticSurd Pow(QuadraticSurd x, int n)
    {
        if (n < 0)
            return FromInteger(BigInteger.One, x.D) / Pow(x, -n);
        var result = FromInteger(BigInteger.One, x.D);
        var baseX = x;
        while (n > 0)
        {
            if ((n & 1) == 1) result *= baseX;
            baseX *= baseX;
            n >>= 1;
        }
        return result;
    }

    // ==================== СРАВНЕНИЕ ====================

    /// <summary>
    /// Проверяет равенство с другим числом.
    /// </summary>
    /// <param name="other">Число для сравнения.</param>
    /// <returns><c>true</c>, если все четыре компоненты (A, B, C, D) совпадают.</returns>
    public bool Equals(QuadraticSurd other)
        => A == other.A && B == other.B && C == other.C && D == other.D;

    /// <summary>Проверяет равенство с произвольным объектом.</summary>
    /// <param name="o">Объект для сравнения.</param>
    /// <returns><c>true</c>, если <paramref name="o"/> — <see cref="QuadraticSurd"/> и равен текущему.</returns>
    public override bool Equals(object? o) => o is QuadraticSurd q && Equals(q);

    /// <summary>Возвращает хеш-код на основе всех четырёх компонент.</summary>
    /// <returns>Хеш-код.</returns>
    public override int GetHashCode() => HashCode.Combine(A, B, C, D);

    /// <summary>Оператор равенства.</summary>
    /// <param name="x">Левое число.</param>
    /// <param name="y">Правое число.</param>
    /// <returns><c>true</c>, если числа равны.</returns>
    public static bool operator ==(QuadraticSurd x, QuadraticSurd y) => x.Equals(y);

    /// <summary>Оператор неравенства.</summary>
    /// <param name="x">Левое число.</param>
    /// <param name="y">Правое число.</param>
    /// <returns><c>true</c>, если числа не равны.</returns>
    public static bool operator !=(QuadraticSurd x, QuadraticSurd y) => !x.Equals(y);

    // ==================== ВЫВОД ====================

    /// <summary>
    /// Возвращает строковое представление числа.
    /// </summary>
    /// <returns>
    /// Для рационального числа — <c>"5"</c> или <c>"1/2"</c>.
    /// Для чистой иррациональности — <c>"√5"</c> или <c>"2√5/3"</c>.
    /// Для смешанного — <c>"(1 + √5)/2"</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// QuadraticSurd.FromInteger(5, 5).ToString();       // "5"
    /// QuadraticSurd.Sqrt(5).ToString();                 // "√5"
    /// new QuadraticSurd(1, 1, 2, 5).ToString();         // "(1 + √5)/2"
    /// new QuadraticSurd(1, -1, 2, 5).ToString();        // "(1 - √5)/2"
    /// </code>
    /// </example>
    public override string ToString()
    {
        if (B.IsZero) return C.IsOne ? A.ToString() : $"{A}/{C}";
        if (A.IsZero)
        {
            // √d, 2√d, -√d, ... — не пишем «1√d»
            string bPart = B.IsOne ? "" : B == -BigInteger.One ? "-" : B.ToString();
            return C.IsOne ? $"{bPart}√{D}" : $"{bPart}√{D}/{C}";
        }
        string sign = B.Sign >= 0 ? "+" : "-";
        string absB = BigInteger.Abs(B).IsOne ? "" : BigInteger.Abs(B).ToString();
        string core = C.IsOne ? $"({A} {sign} {absB}√{D})" : $"({A} {sign} {absB}√{D})/{C}";
        return core;
    }

    /// <summary>
    /// Проверяет, что у двух чисел одинаковое подкоренное выражение.
    /// </summary>
    /// <param name="x">Первое число.</param>
    /// <param name="y">Второе число.</param>
    /// <exception cref="ArgumentException">Если <c>x.D ≠ y.D</c>.</exception>
    private static void CheckSameD(QuadraticSurd x, QuadraticSurd y)
    {
        if (x.D != y.D)
            throw new ArgumentException($"Разные подкоренные: {x.D} ≠ {y.D}");
    }
}