# Примеры использования

Практические примеры работы с библиотекой.

## Покерные руки

Сколько различных пятикарточных комбинаций можно собрать из стандартной колоды в 52 карты?

```csharp
using static Combinatorics.Combinatorics;

var hands = Combinations(52, 5);
Console.WriteLine(hands); // 2598960
```

## Скобочные последовательности

Сколько правильных скобочных последовательностей можно составить из 5 пар скобок?

```csharp
var catalan = Catalan(5);
Console.WriteLine(catalan); // 42
```

Полный список из первых 11 чисел Каталана:

```csharp
var sequence = CatalanSequence(11);
// 1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862, 16796
```

## Разбиения множества

Сколько способов разбить множество из 6 элементов ровно на 3 непустых подмножества?

```csharp
var stirling = StirlingSecondKind(6, 3);
Console.WriteLine(stirling); // 90
```

Сколько всего разбиений множества из 6 элементов?

```csharp
var bell = Bell(6);
Console.WriteLine(bell); // 203
```

## Перестановки с повторениями

Сколько различных анаграмм у слова «MISSISSIPPI»?
Буквы: M×1, I×4, S×4, P×2, всего 11.

```csharp
var anagrams = PermutationsWithRepetition(1, 4, 4, 2);
Console.WriteLine(anagrams); // 34650
```

## Числа Фибоначчи

Первые 100 чисел Фибоначчи вычисляются мгновенно через быстрое удвоение:

```csharp
var fib100 = FibonacciFast(100);
Console.WriteLine(fib100); // 354224848179261915075

var fib100Binet = FibonacciBinet(100);
// Результат совпадает
```

## Точные дроби: числа Бернулли

Числа Бернулли хранятся как точные дроби `BigRational`.

```csharp
var b6 = Bernoulli(6);
Console.WriteLine(b6);                    // 1/42
Console.WriteLine(b6.ToDecimalString(6)); // 0.023809

var b12 = Bernoulli(12);
Console.WriteLine(b12);                   // -691/2730
```

## Асимптотики для больших n

Для очень больших n точное значение занимает мегабайты, а приближение — несколько байт.

```csharp
var exact  = Fibonacci(1000);    // точное
var approx = FibonacciApprox(1000);  // приближение за O(1)
var err    = RelativeError(exact, approx); // ~1e-15
```

## Формулы Бине в точной арифметике

Поле Q(√5) позволяет вычислять числа Фибоначчи по замкнутой формуле.

```csharp
var phi  = Phi;                        // (1 + √5)/2
var phi2 = QuadraticSurd.Pow(phi, 2);  // (3 + √5)/2

// Проверка: φ² = φ + 1
var check = phi2 - phi - QuadraticSurd.FromInteger(1, 5);

var fib50 = FibonacciBinet(50);
Console.WriteLine(fib50); // 12586269025
```

## Генераторы

Ленивый перебор всех сочетаний из 4 элементов по 2:

```csharp
foreach (var combo in CombinationsOf(new[] { "A", "B", "C", "D" }, 2))
    Console.WriteLine(string.Join("", combo));
// AB
// AC
// AD
// BC
// BD
// CD
```

Перебор всех перестановок трёх элементов:

```csharp
foreach (var p in Permutations(new[] { 1, 2, 3 }))
    Console.WriteLine(string.Join(", ", p));
// 1, 2, 3
// 2, 1, 3
// 3, 1, 2
// 1, 3, 2
// 2, 3, 1
// 3, 2, 1
```

## Красивый вывод

Форматтер упрощает отображение результатов.

```csharp
using static Combinatorics.Combinatorics;
using static Combinatorics.CombinatoricsFormatter;

PrintResult("Catalan(10)", Catalan(10));
// Catalan(10) = 16796

PrintSequence("Bell", BellSequence(11));
// Bell: 1, 1, 2, 5, 15, 52, 203, 877, 4140, 21147, 115975

PrintCompact("20!", Factorial(20));
// 20! = 2.43e18

PrintRational("B(6)", Bernoulli(6));
// B(6) = 1/42 (≈ 0.023810)

Console.WriteLine(FormatTriangle(
    Enumerable.Range(0, 6).Select(PascalRow)
));
//            1
//          1   1
//        1   2   1
//      1   3   3   1
//    1   4   6   4   1
//  1   5  10  10   5   1
```

## Треугольник Стирлинга

```csharp
var stirling = Enumerable.Range(0, 7).Select(StirlingSecondKindRow);
Console.WriteLine(FormatTriangle(stirling, width: 8));
//                 1
//               1   1
//             1   3   1
//           1   7   6   1
//         1  15  25  10   1
//       1  31  90  65  15   1
//     1  63 301 350 140  21   1
```

## Сравнение точного и приближённого значения

```csharp
for (int n = 10; n <= 50; n += 10)
{
    PrintApproxVsExact($"C({n})", Catalan(n), CatalanApprox(n));
}
// C(10) = 16796    (≈ 1.6796e4, погрешность 0.0000%)
// C(20) = 6564120420 (≈ 6.5641e9, погрешность 0.0000%)
// C(30) = ...
// C(40) = ...
// C(50) = ...
```

## Что дальше

- [Архитектура](architecture.md) — структура проекта.