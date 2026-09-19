# Генераторы

Ленивый перебор перестановок и сочетаний.

## Перестановки

**Heap's algorithm** — O(n!) по времени, O(n) по памяти. Ленивый: следующая перестановка вычисляется только при запросе.

```csharp
using static Combinatorics.Combinatorics;

foreach (var p in Permutations(new[] { 1, 2, 3 }))
    Console.WriteLine(string.Join(", ", p));
// 1, 2, 3
// 2, 1, 3
// 3, 1, 2
// 1, 3, 2
// 2, 3, 1
// 3, 2, 1
```

**Граничные случаи:**

```csharp
Permutations(Array.Empty<int>()).Count();   // 1 — одна пустая перестановка
Permutations(new[] { 42 }).Single();         // [42]
```

**Количество перестановок = n!:**

```csharp
Permutations(Enumerable.Range(0, 5)).Count(); // 120
```

## Сочетания по элементам

`CombinationsOf<T>(items, k)` — все сочетания длины k.

**Важно:** имя `CombinationsOf`, а не `Combinations`, чтобы не конфликтовать с числовым `Combinations(n, k)`.

```csharp
foreach (var c in CombinationsOf(new[] { "A", "B", "C", "D" }, 2))
    Console.WriteLine(string.Join("", c));
// AB
// AC
// AD
// BC
// BD
// CD
```

**Из диапазона 0..n−1:**

```csharp
foreach (var c in CombinationsOf(5, 2))
    Console.WriteLine(string.Join(",", c));
// 0,1  0,2  0,3  0,4  1,2  1,3  1,4  2,3  2,4  3,4
```

## Сочетания по индексам

`EnumerateCombinationIndices(n, k)` — только индексы, без копирования элементов. Минимум аллокаций.

**Лексикографический порядок** гарантирован.

```csharp
foreach (var idx in EnumerateCombinationIndices(4, 2))
    Console.WriteLine(string.Join(",", idx));
// 0,1
// 0,2
// 0,3
// 1,2
// 1,3
// 2,3
```

## Сравнение методов

| Метод | Возвращает | Аллокации |
|---|---|---|
| `Permutations<T>` | `IEnumerable<T[]>` | O(n) на перестановку |
| `CombinationsOf<T>` | `IEnumerable<T[]>` | O(k) на сочетание |
| `EnumerateCombinationIndices` | `IEnumerable<int[]>` | O(k) на сочетание |

## Граничные случаи

```csharp
// k = 0 — одна пустая выборка
CombinationsOf(5, 0).Count();     // 1

// k > n — пустая последовательность
CombinationsOf(3, 5);             // пусто

// k = n — одна полная выборка
CombinationsOf(3, 3).Count();     // 1

// n = 0, k = 0 — одна пустая
CombinationsOf(0, 0).Count();     // 1
```

## Ленивость

Генераторы **не вычисляют всю последовательность заранее**. Можно прервать цикл:

```csharp
// Взять только первые 5 сочетаний из 20 по 10
var first5 = CombinationsOf(20, 10).Take(5).ToList();
```

**Важно:** даже для `C(20, 10) = 184756` сочетаний генератор не строит их все — только запрошенные.

## Производительность

- **Перестановки:** O(n!) — Heap's algorithm минимально возможен.
- **Сочетания:** O(C(n, k) · k) — каждое сочетание копируется один раз.
- **Индексы:** O(C(n, k) · k) без копирования элементов.

Подробнее — см. бенчмарки `GeneratorBenchmarks` и `FullSequenceBenchmarks`.

## См. также

- [Базовые вычисления](basic.md)