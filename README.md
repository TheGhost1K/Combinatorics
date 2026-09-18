# Combinatorics

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![License](https://img.shields.io/badge/license-MIT-blue)
![Tests](https://img.shields.io/badge/tests-410%20passing-brightgreen)
![NuGet](https://img.shields.io/nuget/v/Combinatorics)

Библиотека комбинаторных чисел на C#. Все методы возвращают `BigInteger` или `BigRational`, чтобы избежать переполнения на больших n. Включает формулы Бине в точной арифметике, асимптотики с оценкой погрешности, ленивые генераторы перестановок и сочетаний.

---

## Оглавление

- [Возможности](#возможности)
- [Установка](#установка)
- [Быстрый старт](#быстрый-старт)
- [Базовые](#базовые)
- [Каталан и родственные](#каталан-и-родственные)
- [Стирлинг, Белл, Эйлер, Лах](#стирлинг-белл-эйлер-лах)
- [Пути: Деланнуа, Шрёдер, Моцкин](#пути-деланнуа-шрёдер-моцкин)
- [Рекуррентные: Фибоначчи, Люка, Пелль](#рекуррентные-фибоначчи-люка-пелль)
- [Числа Бернулли](#числа-бернулли)
- [Формулы Бине в точной арифметике](#формулы-бине-в-точной-арифметике)
- [Асимптотики](#асимптотики)
- [Генераторы](#генераторы)
- [Точные дроби и иррациональности](#точные-дроби-и-иррациональности)
- [API Reference](#api-reference)
- [Требования](#требования)
- [Тестирование и бенчмарки](#тестирование-и-бенчмарки)
- [Лицензия](#лицензия)

---

## Возможности

### Базовые
- Факториал, перестановки (с повторениями), размещения, сочетания (с повторениями).
- Треугольник Паскаля.
- Числа Каталана, последовательность Каталана.

### Классические
- Числа Нараяны.
- Числа Моцкина (обычные, 2-цветные, обобщённые с m цветами).
- Числа Фусса–Каталана (m-арные деревья).

### Разбиения
- Числа Стирлинга 1-го рода (беззнаковые и знаковые).
- Числа Стирлинга 2-го рода.
- Числа Белла.
- Числа Лаха и упорядоченные числа Белла (Fubini).

### Перестановки
- Числа Эйлера (подъёмы и спуски).

### Пути
- Числа Деланнуа (прямоугольные и центральные).
- Большие и малые числа Шрёдера.
- Числа Шрёдера–Каталана.

### Рекуррентные
- Числа Фибоначчи (итеративно, fast doubling, формула Бине).
- Числа Люка.
- Числа Пелля.
- Числа Пелля–Люка.

### Бернулли
- Числа Бернулли как точные дроби `BigRational`.

### Формулы Бине в точной арифметике
- Класс `QuadraticSurd` для чисел вида `(a + b·√d)/c`.
- Точные формулы для Фибоначчи, Люка, Пелля, Пелля–Люка.

### Асимптотики
- Базовые оценки O(1/n) для факториала, Каталана, Белла, сочетаний.
- Расширенные оценки O(1/n⁴) через ряды Стирлинга.
- Функция Ламберта W и гамма Ланцоша.
- Оценка относительной погрешности.

### Генераторы
- Перестановки (Heap's algorithm, ленивые).
- Сочетания по элементам и по индексам.

### Красивый вывод
- Форматирование последовательностей, таблиц, треугольников.
- Компактный формат для больших чисел (K/M/B/e-нотация).
- Красивая печать чисел Бернулли (дробь + десятичное).
- Сравнение точного и приближённого значения с оценкой погрешности.
- Рамки, заголовки, разделители тысяч, работа с колонками.

---

## Установка

### Через NuGet

```bash
dotnet add package Combinatorics
```

### Из исходников

```bash
git clone https://github.com/TheGhost1K/combinatorics.git
cd combinatorics
dotnet build -c Release
dotnet test -c Release
```

---

## Быстрый старт

```csharp
using static Combinatorics.Combinatorics;

// Базовые
var fact    = Factorial(20);          // 2432902008176640000
var hands   = Combinations(52, 5);    // 2598960
var perm    = Arrangements(10, 3);    // 720
var catalan = Catalan(10);            // 16796

// Разбиения
var bell     = Bell(10);                // 115975
var stirling = StirlingSecondKind(5, 2); // 15

// Рекуррентные
var fib100 = FibonacciFast(100);       // 354224848179261915075
var pell   = Pell(10);                 // 2378

// Точные дроби
var b6 = Bernoulli(6);                 // 1/42
Console.WriteLine(b6);                  // "1/42"

// Генераторы
foreach (var combo in CombinationsOf(new[] { "A", "B", "C", "D" }, 2))
    Console.WriteLine(string.Join("", combo));
// AB AC AD BC BD CD
```

---

## Базовые

### Факториал и перестановки

```csharp
Factorial(0);                    // 1
Factorial(5);                    // 120
Factorial(20);                   // 2432902008176640000
Factorial(100);                  // 9.33e157 — огромное число

Permutations(5);                 // 120 = 5!

// Анаграммы слова MISSISSIPPI (M×1, I×4, S×4, P×2)
PermutationsWithRepetition(1, 4, 4, 2); // 34650
```

### Сочетания и размещения

```csharp
Combinations(5, 2);              // 10
Combinations(52, 5);             // 2598960 — покерные руки
Combinations(100, 50);           // 1.0e29

Arrangements(10, 3);             // 720 — призовые места
Arrangements(5, 5);              // 120 = 5!

// Разложить 4 неразличимых шара по 3 ящикам
CombinationsWithRepetition(3, 4); // 15
```

### Треугольник Паскаля

```csharp
var row10 = PascalRow(10);
// 1, 10, 45, 120, 210, 252, 210, 120, 45, 10, 1

// Сумма элементов строки = 2^n
PascalRow(5).Sum();              // 32 = 2^5
```

---

## Каталан и родственные

### Числа Каталана

```csharp
Catalan(0);                       // 1
Catalan(5);                       // 42
Catalan(10);                      // 16796
Catalan(15);                      // 9694845

// Последовательность за O(n)
var catalans = CatalanSequence(11);
// 1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862, 16796
```

**Где встречаются:** правильные скобочные последовательности, бинарные деревья, пути Дика, триангуляции выпуклого (n+2)-угольника.

### Числа Нараяны

```csharp
// Перестановки n пар скобок ровно с k «внешними» парами
Narayana(4, 2);                   // 6
Narayana(6, 3);                   // 50

// Вся строка
var narayana = NarayanaRow(6);
// 0, 1, 15, 50, 50, 15, 1

// Сумма по k = Catalan(n)
```

### Числа Фусса–Каталана

```csharp
// Обобщение Каталана на (m+1)-арные деревья
FussCatalan(5, 1);                // 42    = Catalan(5)
FussCatalan(5, 2);                // 273   — тернарные деревья
FussCatalan(5, 3);                // 969   — четвертичные деревья

var seq2 = FussCatalanSequence(8, 2);
// 1, 1, 3, 12, 55, 273, 1428, 7752
```

---

## Стирлинг, Белл, Эйлер, Лах

### Числа Стирлинга 1-го рода

```csharp
// Перестановки n элементов ровно с k циклами
StirlingFirstKind(5, 2);          // 50
StirlingFirstKind(4, 4);          // 1

// Знаковые (коэффициенты падающего факториала)
SignedStirlingFirstKind(5, 2);    // -50
SignedStirlingFirstKind(5, 3);    // 35

// Вся строка
var row = StirlingFirstKindRow(5);
// 0, 24, 50, 35, 10, 1
// Сумма = 5! = 120
```

### Числа Стирлинга 2-го рода

```csharp
// Разбиения n элементов на k непустых подмножеств
StirlingSecondKind(5, 2);         // 15
StirlingSecondKind(5, 3);         // 25
StirlingSecondKind(10, 5);        // 42525

var row = StirlingSecondKindRow(5);
// 0, 1, 15, 25, 10, 1
// Сумма = Bell(5) = 52
```

### Числа Белла

```csharp
Bell(0);                          // 1
Bell(5);                          // 52
Bell(10);                         // 115975
Bell(20);                         // 51724158235372

var bells = BellSequence(11);
// 1, 1, 2, 5, 15, 52, 203, 877, 4140, 21147, 115975
```

**Где встречаются:** все разбиения множества, моменты распределения Пуассона, кластеризация.

### Числа Эйлера

```csharp
// Перестановки n элементов ровно с k подъёмами
Eulerian(5, 2);                   // 66
Eulerian(4, 1);                   // 11

// Спуски (симметрия: A(n, k) = A(n, n-1-k))
EulerianDescents(5, 2);           // 26

var row = EulerianRow(5);
// 1, 26, 66, 26, 1
// Сумма = 5! = 120
```

### Числа Лаха и Fubini

```csharp
// Разбиения n элементов на k линейно упорядоченных подмножеств
Lah(5, 3);                        // 120
Lah(4, 2);                        // 36

var row = LahRow(5);
// 0, 120, 240, 120, 20, 1

// Упорядоченные числа Белла (Fubini)
OrderedBell(0);                   // 1
OrderedBell(5);                   // 541
OrderedBell(6);                   // 4683
```

---

## Пути: Деланнуа, Шрёдер, Моцкин

### Числа Деланнуа

```csharp
// Пути из (0,0) в (m,n) с шагами (1,0), (0,1), (1,1)
Delannoy(3, 3);                   // 63
Delannoy(5, 5);                   // 1683
Delannoy(2, 3);                   // 25

// Центральные
DelannoyCentral(5);               // 1683

var central = DelannoyCentralSequence(8);
// 1, 3, 13, 63, 321, 1683, 8989, 48639
```

### Числа Шрёдера

```csharp
// Большие: пути из (0,0) в (n,n) без подъёма выше диагонали
SchroederLarge(0);                // 1
SchroederLarge(5);                // 394
SchroederLarge(8);                // 41586

// Малые (в 2 раза меньше для n ≥ 1)
SchroederSmall(5);                // 197
SchroederSmall(8);                // 20793

// Шрёдер–Каталан — синоним малых
SchroederCatalan(7);              // 4279

var large = SchroederLargeSequence(9);
// 1, 2, 6, 22, 90, 394, 1806, 8558, 41586
```

### Числа Моцкина

```csharp
// Пути длины n с шагами (1,0), (1,1), (1,-1), не ниже оси
Motzkin(0);                       // 1
Motzkin(10);                      // 2188
Motzkin(15);                      // 127

var motzkins = MotzkinSequence(11);
// 1, 1, 2, 4, 9, 21, 51, 127, 323, 835, 2188

// Моцкин с 2 цветами горизонтального шага = Catalan(n+1)
MotzkinTwoColored(5);             // 132 = Catalan(6)

// Обобщённый: m цветов горизонтального шага
MotzkinGeneralized(5, 1);         // 21 = Motzkin(5)
MotzkinGeneralized(5, 2);         // 132 = MotzkinTwoColored(5)
MotzkinGeneralized(5, 3);         // 543

var seq3 = MotzkinGeneralizedSequence(8, 3);
// 1, 3, 10, 36, 137, 543, 2219, 9285

// Проверка через закрытую формулу
MotzkinGeneralizedBySum(5, 3);    // 543
```

---

## Рекуррентные: Фибоначчи, Люка, Пелль

### Числа Фибоначчи

```csharp
// Итеративно O(n)
Fibonacci(0);                     // 0
Fibonacci(10);                    // 55
Fibonacci(30);                    // 832040

// Быстрое удвоение O(log n) — предпочтительно для больших n
FibonacciFast(50);                // 12586269025
FibonacciFast(100);               // 354224848179261915075
FibonacciFast(1000);              // 4.35e208

// Пара (F(n), F(n+1))
var (fn, fn1) = FibonacciPair(10);
// (55, 89)

var fibs = FibonacciSequence(16);
// 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610
```

### Числа Люка

```csharp
Lucas(0);                         // 2
Lucas(1);                         // 1
Lucas(10);                        // 123
Lucas(20);                        // 15127

var lucases = LucasSequence(11);
// 2, 1, 3, 4, 7, 11, 18, 29, 47, 76, 123
```

### Числа Пелля

```csharp
Pell(0);                          // 0
Pell(1);                          // 1
Pell(10);                         // 2378
Pell(15);                         // 470832

var pells = PellSequence(11);
// 0, 1, 2, 5, 12, 29, 70, 169, 408, 985, 2378
```

### Числа Пелля–Люка

```csharp
PellLucas(0);                     // 2
PellLucas(10);                    // 6726
PellLucas(20);                    // 15994428

var seq = PellLucasSequence(11);
// 2, 2, 6, 14, 34, 82, 198, 478, 1154, 2786, 6726

// Тождество Q(n)² − 8·P(n)² = 4·(−1)^n
PellLucasIdentity(10);            // true
```

### Тождества Фибоначчи и Люка

```csharp
// Кассини: F(n+1)² − F(n)·F(n+2) = (−1)^n
CassiniIdentity(10);              // true

// Люка: L(n)² − 5·F(n)² = 4·(−1)^n
LucasIdentity(10);                // true

// Формула удвоения: F(2n) = F(n)·L(n)
DoublingFormula(10);              // true

// L(n) = F(n−1) + F(n+1)
LucasFromFibonacci(10);           // true
```

---

## Числа Бернулли

```csharp
// Точные дроби через BigRational
Bernoulli(0);                     // 1
Bernoulli(1);                     // -1/2
Bernoulli(2);                     // 1/6
Bernoulli(3);                     // 0
Bernoulli(4);                     // -1/30
Bernoulli(6);                     // 1/42
Bernoulli(12);                    // -691/2730

// Строковое представление
Console.WriteLine(Bernoulli(6));  // "1/42"

// Десятичное представление для отладки
Bernoulli(6).ToDecimalString();   // "0.023809"
Bernoulli(6).ToDecimalString(2);  // "0.02"

// Последовательность
var seq = BernoulliSequence(10);
// 1, -1/2, 1/6, 0, -1/30, 0, 1/42, 0, -1/30, 0

// Все B_{2k+1} = 0 при k ≥ 1
```

**Где встречаются:** формула Фаульхабера (суммы степеней), дзета-функция Римана при чётных аргументах.

---

## Формулы Бине в точной арифметике

### QuadraticSurd — точные числа вида (a + b·√d) / c

```csharp
// φ = (1 + √5)/2
var phi = Phi;
Console.WriteLine(phi);           // "(1 + √5)/2"

// φ² = (3 + √5)/2
var phi2 = QuadraticSurd.Pow(phi, 2);

// φ·ψ = -1
var product = Phi * Psi;          // -1

// Арифметика в Q(√2)
var x = new QuadraticSurd(1, 1, 1, 2);   // 1 + √2
var y = new QuadraticSurd(1, -1, 1, 2);  // 1 - √2
var prod = x * y;                         // -1

// Норма: N(a + b·√d) = a² - d·b²
x.Norm;                           // -1
```

### Формулы Бине

```csharp
// F(n) = (φ^n − ψ^n) / √5
FibonacciBinet(50);               // 12586269025
FibonacciBinet(100);              // совпадает с FibonacciFast(100)

// L(n) = φ^n + ψ^n
LucasBinet(10);                   // 123

// P(n) = ((1+√2)^n − (1−√2)^n) / (2√2)
PellBinet(10);                    // 2378

// Q(n) = (1+√2)^n + (1−√2)^n
PellLucasBinet(10);               // 6726
```

**Примечание:** для продакшена используйте `FibonacciFast` (работает только с `BigInteger` и в ~18 раз быстрее). Формулы Бине в `QuadraticSurd` — для учебных целей и теоретических выкладок.

---

## Асимптотики

### Базовые (O(1/n))

```csharp
// Факториал: n! ≈ √(2πn) · (n/e)^n
FactorialStirling(100);           // 9.33e157

// Каталан: C(n) ≈ 4^n / (n^(3/2)·√π)
CatalanApprox(1000);              // 2.04e598

// Фибоначчи: F(n) ≈ φ^n / √5
FibonacciApprox(100);             // 3.54e20

// Белл через функцию Ламберта W
BellApprox(50);                   // 1.86e47

// Сочетания через логарифмы
CombinationsApprox(1000, 500);    // 2.70e299
```

### Расширенные (O(1/n⁴))

```csharp
// Факториал с рядом Стирлинга
FactorialStirlingExtended(100);   // точнее базовой на 4 знака

// Уточнённый Стирлинг с 1/(12n)
FactorialStirlingRefined(100);

// Каталан с 3 поправками
CatalanApproxExtended(1000);

// Фибоначчи через полную формулу Бине в double
FibonacciApproxExtended(70);      // машинная точность для F(n) < 2^53

// Белл с поправкой
BellApproxExtended(50);

// Деланнуа и Шрёдер с поправками
DelannoyCentralApproxExtended(100);
SchroederLargeApproxExtended(100);
```

### Оценка точности

```csharp
// Относительная погрешность
var err = RelativeError(Factorial(20), FactorialStirling(20));
// ~0.0004 (0.04%)

var err2 = RelativeError(Factorial(20), FactorialStirlingExtended(20));
// ~1e-10
```

### Специальные функции

```csharp
// Функция Ламберта W(x) — решение W·e^W = x
LambertW(1.0);                    // 0.5671432904
LambertW(Math.E);                 // 1.0
LambertW(1e15);                   // ~26.36

// Гамма-функция через Ланцоша (точность ~15 значащих цифр)
GammaLanczos(5.0);                // 24 = 4!
GammaLanczos(0.5);                // √π ≈ 1.7724

// Логарифм гамма
LogGammaLanczos(100);             // ln(99!) ≈ 359.13

// Логарифм факториала
LogFactorialLanczos(100);         // ln(100!)
```

---

## Генераторы

### Перестановки (Heap's algorithm)

```csharp
// Ленивый перебор всех перестановок
foreach (var p in Permutations(new[] { 1, 2, 3 }))
    Console.WriteLine(string.Join(", ", p));
// 1, 2, 3
// 2, 1, 3
// 3, 1, 2
// 1, 3, 2
// 2, 3, 1
// 3, 2, 1

// Количество перестановок = n!
var count = Permutations(new[] { 1, 2, 3, 4 }).Count(); // 24

// Пустая коллекция даёт одну пустую перестановку
Permutations(Array.Empty<int>()).Count(); // 1

// Одноэлементная — саму себя
Permutations(new[] { 42 }).Single();      // [42]
```

### Сочетания по элементам

```csharp
// Все сочетания элементов
foreach (var c in CombinationsOf(new[] { "A", "B", "C", "D" }, 2))
    Console.WriteLine(string.Join("", c));
// AB AC AD BC BD CD

// Из диапазона 0..n-1
foreach (var c in CombinationsOf(5, 2))
    Console.WriteLine(string.Join(",", c));
// 0,1  0,2  0,3  0,4  1,2  1,3  1,4  2,3  2,4  3,4

// k = 0 даёт одну пустую выборку
CombinationsOf(5, 0).Count();     // 1

// k > n даёт пустую последовательность
CombinationsOf(3, 5);             // пусто
```

### Сочетания по индексам

```csharp
// Только индексы — минимум аллокаций
foreach (var idx in EnumerateCombinationIndices(5, 3))
    Console.WriteLine(string.Join(",", idx));
// 0,1,2  0,1,3  0,1,4  0,2,3  0,2,4  0,3,4  1,2,3  1,2,4  1,3,4  2,3,4

// Лексикографический порядок гарантирован
EnumerateCombinationIndices(4, 2).Select(c => string.Join(",", c));
// ["0,1", "0,2", "0,3", "1,2", "1,3", "2,3"]
```

---

## Красивый вывод

Класс `CombinatoricsFormatter` содержит утилиты для форматирования и печати результатов. Он не влияет на вычисления и не имеет состояния.

```csharp
using static Combinatorics.Combinatorics;
using static Combinatorics.CombinatoricsFormatter;
```

### Форматирование последовательностей

```csharp
// Через запятую
FormatSequence(CatalanSequence(6));
// "1, 1, 2, 5, 14, 42"

// С индексами
FormatSequenceIndexed(BellSequence(5));
// "0:1, 1:1, 2:2, 3:5, 4:15"
```

### Компактный формат для больших чисел

```csharp
FormatCompact(42);                    // "42"
FormatCompact(1500);                  // "1.50K"
FormatCompact(2_500_000);             // "2.50M"
FormatCompact(1_000_000_000);         // "1.00B"
FormatCompact(Factorial(20));         // "2.43e18"
FormatCompact(FibonacciFast(1000));   // "4.35e208"

// Научная нотация с точностью мантиссы
FormatScientific(Factorial(20));      // "2.43e18"
FormatScientific(Factorial(20), 5);   // "2.43290e18"
```

### Форматирование чисел Бернулли

```csharp
var b6 = Bernoulli(6);                         // 1/42

FormatRational(b6);                            // "1/42"
FormatRational(b6, RationalMode.Decimal);      // "0.023810"
FormatRational(b6, RationalMode.Both);         // "1/42 (≈ 0.023810)"
FormatRational(Bernoulli(12), RationalMode.Both, 8);
// "-691/2730 (≈ -0.25311355)"
```

### Таблица «индекс → значение»

```csharp
Console.WriteLine(FormatTable(BellSequence(6)));
// +------+----------------------+
// |    n |             значение |
// +------+----------------------+
// |    0 |                    1 |
// |    1 |                    1 |
// |    2 |                    2 |
// |    3 |                    5 |
// |    4 |                   15 |
// |    5 |                   52 |
// +------+----------------------+
```

### Две колонки рядом (точное vs приближённое)

```csharp
var exact = Enumerable.Range(0, 11).Select(n => Catalan(n)).ToArray();
var approx = Enumerable.Range(0, 11)
    .Select(n => n == 0 ? BigInteger.One : (BigInteger)Math.Round(CatalanApprox(n)))
    .ToArray();

Console.WriteLine(FormatTableDouble(exact, approx, "C(n) точное", "C(n) ≈"));
// +------+---------------------------+---------------------------+
// |    n |            C(n) точное    |              C(n) ≈       |
// +------+---------------------------+---------------------------+
// |    0 |                         1 |                         1 |
// |    1 |                         1 |                         1 |
// |    2 |                         2 |                         1 |
// ...
```

### Треугольник (Паскаль, Стирлинг, Эйлер)

```csharp
var pascal = Enumerable.Range(0, 8).Select(PascalRow);
Console.WriteLine(FormatTriangle(pascal));
//                         1
//                     1       1
//                 1       2       1
//             1       3       3       1
//         1       4       6       4       1
//     1       5      10      10       5       1
// 1       6      15      20      15       6       1

var stirling = Enumerable.Range(0, 7).Select(StirlingSecondKindRow);
Console.WriteLine(FormatTriangle(stirling));
```

### Печать в несколько колонок

```csharp
Console.WriteLine(FormatColumns(MotzkinSequence(20), columns: 5));
// 0:1         4:9         8:323       12:15511    16:142547
// 1:1         5:21        9:835       13:41835    17:747261
// 2:2         6:51        10:2188     14:113634   18:4005811
// 3:4         7:127       11:5798     15:310572   19:21776331
```

### Рамка вокруг текста

```csharp
Console.WriteLine(Box($"Catalan(10) = {Catalan(10)}\n" +
                      $"Bell(10)    = {Bell(10)}\n" +
                      $"Fib(100)    = {FormatScientific(FibonacciFast(100))}"));
// +-------------------------+
// | Catalan(10) = 16796     |
// | Bell(10)    = 115975    |
// | Fib(100)    = 3.54e20   |
// +-------------------------+
```

### Разделители тысяч

```csharp
FormatThousands(Factorial(20));
// "2 432 902 008 176 640 000"

FormatThousands(Bell(20));
// "51 724 158 235 372"
```

### Печать в консоль

```csharp
// Простой вывод "label = value"
PrintResult("Catalan(10)", Catalan(10));
// Catalan(10) = 16796

// Компактно
PrintCompact("20!", Factorial(20));
// 20! = 2.43e18

// Научно
PrintScientific("100!", Factorial(100), 3);
// 100! = 9.333e157

// Рациональное
PrintRational("B(6)", Bernoulli(6));
// B(6) = 1/42 (≈ 0.023810)

// Последовательность
PrintSequence("Bell", BellSequence(11));
// Bell: 1, 1, 2, 5, 15, 52, 203, 877, 4140, 21147, 115975

// Таблица
PrintTable("Числа Белла", BellSequence(8));

// Точное vs приближённое
PrintApproxVsExact("F(50)", Fibonacci(50), FibonacciApprox(50));
// F(50) = 12586269025  (≈ 1.2586e10, погрешность 0.0000%)

// Заголовки
PrintHeader("Combinatorics");
// ============================================================
//   Combinatorics
// ============================================================

PrintSubheader("Асимптотика");
// Асимптотика
// ------------------------------------------------------------
```

### Полный пример

```csharp
using static Combinatorics.Combinatorics;
using static Combinatorics.CombinatoricsFormatter;

PrintHeader("Combinatorics — демонстрация");

PrintResult("Catalan(10)", Catalan(10));
PrintCompact("20!", Factorial(20));
PrintScientific("100!", Factorial(100), 3);

PrintSubheader("Последовательности");
PrintSequence("Catalan", CatalanSequence(11));
PrintSequence("Bell", BellSequence(11));

PrintSubheader("Треугольник Паскаля");
var pascal = Enumerable.Range(0, 8).Select(PascalRow);
Console.WriteLine(FormatTriangle(pascal));

PrintSubheader("Числа Бернулли");
for (int i = 0; i <= 10; i++)
    PrintRational($"B({i})", Bernoulli(i));

PrintSubheader("Асимптотика");
for (int n = 10; n <= 50; n += 10)
    PrintApproxVsExact($"C({n})", Catalan(n), CatalanApprox(n));
```

### Сводная таблица методов форматтера

| Метод | Что делает |
|---|---|
| `FormatSequence` | Последовательность в строку через запятую |
| `FormatSequenceIndexed` | Последовательность с индексами |
| `FormatCompact` | Компактный формат (K/M/B/e) |
| `FormatScientific` | Научная нотация с точностью |
| `FormatSmart` | Синоним `FormatCompact` |
| `FormatRational` | Дробь/десятичное/оба |
| `FormatTable` | Таблица «n → значение» с рамкой |
| `FormatTableDouble` | Две колонки рядом |
| `FormatTriangle` | Треугольник с центрированием |
| `FormatColumns` | Последовательность в несколько колонок |
| `FormatThousands` | Разделители тысяч |
| `FormatDuration` | Человекочитаемая длительность |
| `Box` | Оборачивает текст в рамку |
| `PrintResult` | Печать `label = value` |
| `PrintCompact` | Печать в компактном формате |
| `PrintScientific` | Печать в научной нотации |
| `PrintRational` | Печать дроби |
| `PrintSequence` | Печать последовательности |
| `PrintTable` | Печать таблицы |
| `PrintApproxVsExact` | Точное vs приближённое с погрешностью |
| `PrintHeader` | Заголовок секции |
| `PrintSubheader` | Подзаголовок |
| `RationalMode` (enum) | `Fraction`, `Decimal`, `Both` |

## Точные дроби и иррациональности

### BigRational — точные дроби

```csharp
// Создание и нормализация
var half = new BigRational(1, 2);
var alsoHalf = new BigRational(2, 4);      // нормализуется к 1/2
var third = new BigRational(1, 3);
var minusHalf = new BigRational(1, -2);    // знак переносится в числитель

// Арифметика
half + third;                     // 5/6
half - third;                     // 1/6
half * third;                     // 1/6
half / third;                     // 3/2
-half;                            // -1/2

// Сравнение
third < half;                     // true
half > third;                     // true
half <= half;                     // true
half >= alsoHalf;                 // true

// Сравнение с примитивами
half < 1;                         // true
half > 0;                         // true
half >= BigInteger.One / 2;       // true

// Сортировка
var list = new List<BigRational>
{
    new(1, 2), new(1, 4), new(3, 4), new(1, 3), new(2, 3)
};
list.Sort();
// 1/4, 1/3, 1/2, 2/3, 3/4

// IComparable
((IComparable)third).CompareTo(half); // -1

// Вывод
Console.WriteLine(half);          // "1/2"
Console.WriteLine(new BigRational(5)); // "5"
half.ToDecimalString();           // "0.500000"
half.ToDecimalString(2);          // "0.50"

// Исключения
new BigRational(1, 0);            // DivideByZeroException
```

### QuadraticSurd — точные иррациональности

```csharp
// φ = (1 + √5)/2
var phi = new QuadraticSurd(1, 1, 2, 5);
Console.WriteLine(phi);           // "(1 + √5)/2"

// Нормализация gcd и знака
var x = new QuadraticSurd(2, 2, 4, 5);   // → (1 + √5)/2
var y = new QuadraticSurd(1, 1, -2, 5);  // → (-1 - √5)/2

// Сопряжение
phi.Conjugate;                    // (1 - √5)/2

// Норма
phi.Norm;                         // -1

// Арифметика
phi + phi;                        // 1 + √5
phi * phi.Conjugate;              // -1
phi / phi;                        // 1

// Возведение в степень (O(log n))
QuadraticSurd.Pow(phi, 10);
QuadraticSurd.Pow(phi, -1);       // φ⁻¹ = φ - 1

// Сравнение
var a = QuadraticSurd.Sqrt(5);
var b = QuadraticSurd.Sqrt(5);
a == b;                           // true

// Исключения
QuadraticSurd.Sqrt(5) + QuadraticSurd.Sqrt(7); // ArgumentException: разные d
```

---

## API Reference

Полная документация: **[https://TheGhost1K.github.io/combinatorics/](https://TheGhost1K.github.io/combinatorics/)**

Сгенерирована через DocFX из XML-комментариев в исходниках.

### Сводная таблица методов

| Группа | Методы |
|---|---|
| Базовые | `Factorial`, `Permutations`, `PermutationsWithRepetition`, `Arrangements`, `Combinations`, `CombinationsWithRepetition`, `PascalRow` |
| Каталан | `Catalan`, `CatalanSequence`, `Narayana`, `NarayanaRow`, `FussCatalan`, `FussCatalanSequence` |
| Стирлинг | `StirlingFirstKind`, `SignedStirlingFirstKind`, `StirlingFirstKindRow`, `StirlingSecondKind`, `StirlingSecondKindRow` |
| Белл / Эйлер | `Bell`, `BellSequence`, `Eulerian`, `EulerianRow`, `EulerianDescents` |
| Лах | `Lah`, `LahRow`, `OrderedBell` |
| Пути | `Delannoy`, `DelannoyCentral`, `DelannoyCentralSequence`, `DelannoyByRecurrence`, `SchroederLarge`, `SchroederSmall`, `SchroederCatalan`, `*Sequence` |
| Моцкин | `Motzkin`, `MotzkinSequence`, `MotzkinTwoColored`, `MotzkinTwoColoredSequence`, `MotzkinGeneralized`, `MotzkinGeneralizedSequence`, `MotzkinGeneralizedBySum` |
| Фибоначчи | `Fibonacci`, `FibonacciFast`, `FibonacciPair`, `FibonacciSequence`, `Lucas`, `LucasSequence`, `CassiniIdentity`, `LucasIdentity`, `DoublingFormula`, `LucasFromFibonacci` |
| Пелль | `Pell`, `PellSequence`, `PellLucas`, `PellLucasSequence`, `PellLucasIdentity` |
| Бернулли | `Bernoulli`, `BernoulliSequence` |
| Бине | `FibonacciBinet`, `LucasBinet`, `PellBinet`, `PellLucasBinet` |
| Асимптотики | `FactorialStirling`, `FactorialStirlingRefined`, `FactorialStirlingExtended`, `LogFactorialLanczos`, `CatalanApprox`, `CatalanApproxExtended`, `FibonacciApprox`, `FibonacciApproxExtended`, `LucasApprox`, `LucasApproxExtended`, `PellApprox`, `PellApproxExtended`, `PellLucasApprox`, `PellLucasApproxExtended`, `BellApprox`, `BellApproxExtended`, `CombinationsApprox`, `DelannoyCentralApprox`, `DelannoyCentralApproxExtended`, `SchroederLargeApprox`, `SchroederLargeApproxExtended`, `MotzkinApprox` |
| Спецфункции | `LambertW`, `GammaLanczos`, `LogGammaLanczos`, `RelativeError` |
| Генераторы | `Permutations<T>`, `EnumerateCombinationIndices`, `CombinationsOf<T>`, `CombinationsOf` |
| Форматтер | `FormatSequence`, `FormatSequenceIndexed`, `FormatCompact`, `FormatScientific`, `FormatSmart`, `FormatRational`, `FormatTable`, `FormatTableDouble`, `FormatTriangle`, `FormatColumns`, `FormatThousands`, `FormatDuration`, `Box`, `PrintResult`, `PrintCompact`, `PrintScientific`, `PrintRational`, `PrintSequence`, `PrintTable`, `PrintApproxVsExact`, `PrintHeader`, `PrintSubheader` |

---

## Требования

- **.NET 10.0** или новее.
- **C# 14** (для `readonly struct`, primary constructors и т.д.).

---

## Тестирование и бенчмарки

### Тесты

```bash
dotnet test -c Release
```

**~270 тестов** на xUnit, включая проверку тождеств:

- Кассини: `F(n+1)² − F(n)·F(n+2) = (−1)^n`
- Люка: `L(n)² − 5·F(n)² = 4·(−1)^n`
- Пелля–Люка: `Q(n)² − 8·P(n)² = 4·(−1)^n`
- Формулы удвоения: `F(2n) = F(n)·L(n)`, `P(2n) = P(n)·Q(n)`
- Суммы строк: `Σ_k S(n, k) = B_n`, `Σ_k |s(n, k)| = n!`, `Σ_k A(n, k) = n!`
- Симметрии: `A(n, k) = A(n, n-1-k)`, `N(n, k) = N(n, n+1-k)`
- Согласованность операторов с `CompareTo`
- Граничные случаи и исключения

### Бенчмарки

```bash
cd benchmarks/Combinatorics.Benchmarks
dotnet run -c Release -- --filter *
```

Группы:

- `BasicBenchmarks` — факториал, сочетания, Каталан, Паскаль.
- `StirlingBenchmarks` — Стирлинг 1-го/2-го рода, Белл, Эйлер.
- `GeneratorBenchmarks` — перестановки, сочетания.
- `BigRationalBenchmarks` — числа Бернулли.
- `BinetVsDoublingBenchmarks` — сравнение методов Фибоначчи.
- `QuadraticSurdBenchmarks` — арифметика в Q(√d).
- `AsymptoticsBenchmarks` — базовая и расширенная асимптотика.
- `StirlingEulerBenchmarks` — одиночные vs построение строк.
- `LahNarayanaBenchmarks` — Лах, Нараян, Fubini, Моцкин.
- `SchroederDelannoyBenchmarks` — Шрёдер, Деланнуа.
- `FussCatalanMotzkinBenchmarks` — Фусс–Каталан, Фибоначчи, Пелль.
- `FullSequenceBenchmarks` — сводный прогон по всем семействам.

Выборочный запуск:

```bash
dotnet run -c Release -- --filter *BinetVsDoubling*
dotnet run -c Release -- --filter *FullSequence* --exporters markdown html csv
```

Результаты складываются в `BenchmarkDotNet.Artifacts/results/`.

---

## Архитектура

```
src/Combinatorics/
├── BigRational.cs                      — точные дроби на BigInteger
├── QuadraticSurd.cs                    — точная арифметика в Q(√d)
├── CombinatoricsFormatter.cs           — красивый вывод результатов
├── Combinatorics.Basic.cs              — факториал, сочетания, Каталан, Паскаль
├── Combinatorics.Stirling.cs           — Стирлинг 1-го и 2-го рода
├── Combinatorics.BellEuler.cs          — Белл, Эйлер
├── Combinatorics.Lah.cs                — Лах, Fubini
├── Combinatorics.Narayana.cs           — Нараян
├── Combinatorics.Motzkin.cs            — Моцкин
├── Combinatorics.Bernoulli.cs          — Бернулли
├── Combinatorics.Generators.cs         — перестановки, сочетания
├── Combinatorics.Extra.cs              — Деланнуа, Шрёдер, Фусс–Каталан
├── Combinatorics.PellSchroeder.cs      — Пелль, Пелль–Люка, Шрёдер–Каталан
├── Combinatorics.FibonacciMotzkin.cs   — Фибоначчи, Люка, обобщённый Моцкин
├── Combinatorics.Binet.cs              — формулы Бине
├── Combinatorics.Asymptotics.cs        — базовые асимптотики
└── Combinatorics.AsymptoticsExtended.cs — расширенные асимптотики
```

Все `Combinatorics.*.cs` — это `partial class Combinatorics`, разбитый на логические группы.

---

## Ссылки

- [OEIS](https://oeis.org) — Онлайн-энциклопедия целочисленных последовательностей.
- [DocFX](https://dotnet.github.io/docfx/) — генератор документации.
- [BenchmarkDotNet](https://benchmarkdotnet.org/) — фреймворк бенчмарков.

---

## Лицензия

MIT. См. [LICENSE](LICENSE).

---

## Вклад

Приветствуются issue и pull request'ы. Перед PR:

```bash
dotnet build -c Release
dotnet test -c Release
```

### Идеи для будущего

- Числа Деланнуа в общем виде `D(m, n)` через NTT для больших m, n.
- Кэш (LRU) для часто вызываемых `Combinations`, `Catalan`.
- Публикация в NuGet с автоматическим CI/CD.
- Source generators для треугольников Паскаля/Стирлинга/Эйлера.