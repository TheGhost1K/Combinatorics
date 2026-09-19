# Формулы Бине в точной арифметике

Точное вычисление рекуррентных последовательностей через квадратичные иррациональности.

## Идея

Числа вида `a + b·√d` образуют **квадратичное поле** `Q(√d)`. В нём определены сложение, умножение, деление — и формулы Бине становятся точными.

Например, `φ = (1+√5)/2 ∈ Q(√5)`, и `F_n = (φ^n − ψ^n)/√5` — **точное** равенство в этом поле, а не приближённое.

## QuadraticSurd

Класс `QuadraticSurd` представляет числа `(a + b·√d)/c`, где `a, b, c, d ∈ ℤ`, `c > 0`, `d > 0`.

Хранится в нормализованной форме: `gcd(a, b, c) = 1`, знак перенесён в `a` и `b`.

### Создание

```csharp
// φ = (1 + √5)/2
var phi = new QuadraticSurd(1, 1, 2, 5);

// Из целого: 5 + 0·√d
var five = QuadraticSurd.FromInteger(5, 5);

// √d
var sqrt5 = QuadraticSurd.Sqrt(5);

// Предопределённые константы
var phi = Phi;                  // (1 + √5)/2
var psi = Psi;                  // (1 − √5)/2
var onePlusSqrt2 = OnePlusSqrt2; // 1 + √2
```

### Арифметика

```csharp
var x = new QuadraticSurd(1, 1, 1, 2);   // 1 + √2
var y = new QuadraticSurd(1, -1, 1, 2);  // 1 − √2

x + y;   // 2
x - y;   // 2√2
x * y;   // -1 (норма)
x / y;   // -3 − 2√2

-x;      // -1 − √2
```

### Возведение в степень

`QuadraticSurd.Pow(x, n)` — быстрое возведение за O(log n).

```csharp
QuadraticSurd.Pow(phi, 2);   // (3 + √5)/2 = φ + 1
QuadraticSurd.Pow(phi, 10);  // точное значение
QuadraticSurd.Pow(phi, -1);  // φ − 1 = 1/φ
```

### Норма и сопряжение

```csharp
var x = new QuadraticSurd(1, 1, 1, 2);
x.Conjugate;   // 1 − √2
x.Norm;        // -1
```

## Формулы Бине

### Фибоначчи и Люка

```csharp
FibonacciBinet(0);    // 0
FibonacciBinet(50);   // 12586269025
FibonacciBinet(100);  // 354224848179261915075
FibonacciBinet(500);  // точное значение

LucasBinet(0);    // 2
LucasBinet(10);   // 123
LucasBinet(50);   // 28143753123
```

### Пелль и Пелль–Люка

```csharp
PellBinet(0);         // 0
PellBinet(10);        // 2378
PellBinet(50);        // точное

PellLucasBinet(10);   // 6726
PellLucasBinet(50);   // точное
```

## Сравнение методов

| Метод | Сложность | Скорость (n = 1000) |
|---|---|---|
| `Fibonacci(n)` — итеративно | O(n) | 320 μs |
| `FibonacciFast(n)` — fast doubling | O(log n) | 8 μs |
| `FibonacciBinet(n)` — Q(√5) | O(log n) с большей константой | 145 μs |

**Вывод:** для продакшена используйте `FibonacciFast` (работает только с `BigInteger`, в ~18 раз быстрее).

**Формулы Бине** в `QuadraticSurd` — для **учебных целей и теоретических выкладок**: они показывают алгебраическую структуру, лежащую в основе последовательностей.

## Тождества в Q(√5)

```csharp
// φ² = φ + 1
var phi2 = QuadraticSurd.Pow(phi, 2);
var check = phi2 - phi - QuadraticSurd.FromInteger(1, 5);
// check.A = 0, check.B = 0

// φ·ψ = −1
var product = Phi * Psi;   // -1
```

## Тождества в Q(√2)

```csharp
// (1 + √2)(1 − √2) = −1
var product = OnePlusSqrt2 * OneMinusSqrt2;

// (1 + √2)¹⁰ = 3363 + 2378√2
var p10 = QuadraticSurd.Pow(OnePlusSqrt2, 10);
// p10.A = 3363, p10.B = 2378
```

## Вывод в консоль

```csharp
Console.WriteLine(Phi);              // (1 + √5)/2
Console.WriteLine(Psi);              // (1 - √5)/2
Console.WriteLine(Sqrt5);            // √5
Console.WriteLine(QuadraticSurd.FromInteger(5, 5)); // 5
Console.WriteLine(new QuadraticSurd(2, 3, 1, 7));   // (2 + 3√7)
```

## См. также

- [Фибоначчи и Люка](fibonacci-lucas.md)
- [Числа Пелля](pell.md)
- [Асимптотики](asymptotics.md)