# Архитектура

## Общая структура

```
combinatorics/
├── src/Combinatorics/                    — библиотека
├── tests/Combinatorics.Tests/            — xUnit-тесты
├── benchmarks/Combinatorics.Benchmarks/  — бенчмарки BenchmarkDotNet
├── docs/                                 — документация DocFX
└── README.md
```

## Файлы библиотеки

```
src/Combinatorics/
├── BigRational.cs                        — точные дроби на BigInteger
├── QuadraticSurd.cs                      — точная арифметика в Q(√d)
├── CombinatoricsFormatter.cs             — красивый вывод результатов
├── Combinatorics.Basic.cs                — факториал, сочетания, Каталан, Паскаль
├── Combinatorics.Stirling.cs             — Стирлинг 1-го и 2-го рода
├── Combinatorics.BellEuler.cs            — Белл, Эйлер
├── Combinatorics.Lah.cs                  — Лах, Fubini
├── Combinatorics.Narayana.cs             — Нараян
├── Combinatorics.Motzkin.cs              — Моцкин
├── Combinatorics.Bernoulli.cs            — Бернулли
├── Combinatorics.Generators.cs           — перестановки, сочетания
├── Combinatorics.Extra.cs                — Деланнуа, Шрёдер, Фусс–Каталан
├── Combinatorics.PellSchroeder.cs        — Пелль, Пелль–Люка, Шрёдер–Каталан
├── Combinatorics.FibonacciMotzkin.cs     — Фибоначчи, Люка, обобщённый Моцкин
├── Combinatorics.Binet.cs                — формулы Бине
├── Combinatorics.Asymptotics.cs          — базовые асимптотики
└── Combinatorics.AsymptoticsExtended.cs  — расширенные асимптотики
```

Все файлы `Combinatorics.*.cs` — это `partial class Combinatorics`,
разбитый на логические группы. Компилятор собирает их в один статический класс.

## Слои

**Уровень 1 — базовые типы:**
- `BigRational` — точные дроби.
- `QuadraticSurd` — точная арифметика в квадратичных полях.
- `CombinatoricsFormatter` — утилиты вывода.

**Уровень 2 — вычисления:**
- Факториал, сочетания, размещения.
- Числа Каталана и родственные.
- Разбиения и перестановки.
- Рекуррентные последовательности.

**Уровень 3 — производные:**
- Формулы Бине (используют `QuadraticSurd`).
- Асимптотики (используют `double` и специальные функции).

## Соглашения

### Возвращаемые типы

- **Целые числа** → `BigInteger` (переполнение невозможно).
- **Дроби** → `BigRational` (точное представление).
- **Приближения** → `double` (быстрые, но с потерей точности).
- **Последовательности** → массивы (`BigInteger[]`, `BigRational[]`).
- **Генераторы** → `IEnumerable<T>` (ленивые).

### Валидация аргументов

Все публичные методы проверяют входные данные:

```csharp
if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
if (k > n) throw new ArgumentException("Требуется 0 ≤ k ≤ n");
```

Исключения типизированы:
- `ArgumentOutOfRangeException` — один аргумент вне диапазона.
- `ArgumentException` — нарушены соотношения между аргументами.
- `DivideByZeroException` — деление на ноль.
- `InvalidOperationException` — недопустимая операция.

### Сложность

Все методы используют оптимальные алгоритмы:

| Метод | Сложность |
|---|---|
| `Factorial` | O(n log² n) через divide & conquer |
| `Combinations` | O(min(k, n−k)) |
| `Catalan` | O(n) |
| `CatalanSequence` | O(n) через рекуррентность |
| `NarayanaRow` | O(n²) через одну `PascalRow` |
| `StirlingFirstKind` | O(n·k) |
| `Bell` | O(n²) |
| `FibonacciFast` | O(log n) через fast doubling |
| `SchroederLargeSequence` | O(n) через рекуррентность |
| `Delannoy` | автовыбор: формула или рекуррентность |

### Документация

Все публичные члены имеют XML-комментарии:

- `<summary>` — краткое описание.
- `<param>` — каждый параметр.
- `<returns>` — что возвращает.
- `<exception>` — какие исключения.
- `<remarks>` — формулы, сложность, ссылки.
- `<example>` — пример кода.
- `<seealso>` — ссылки на OEIS и другие методы.

DocFX собирает из этих комментариев API Reference.

## Тестирование

**~270 тестов** на xUnit, покрывают:

- **Значения** — сверка с известными последовательностями OEIS.
- **Тождества** — Кассини, Люка, Пелля–Люка, формулы удвоения.
- **Суммы** — `Σ_k S(n, k) = B_n`, `Σ_k |s(n, k)| = n!`, `Σ_k A(n, k) = n!`.
- **Симметрии** — `A(n, k) = A(n, n−1−k)`, `N(n, k) = N(n, n+1−k)`.
- **Согласованность** — операторы сравнения и `CompareTo`.
- **Граничные случаи** — n = 0, k = 0, k = n, отрицательные аргументы.
- **Исключения** — что бросается и когда.

## Бенчмарки

12 групп бенчмарков BenchmarkDotNet в `benchmarks/Combinatorics.Benchmarks/`:

- `BasicBenchmarks` — факториал, сочетания, Каталан, Паскаль.
- `StirlingBenchmarks` — Стирлинг, Белл, Эйлер.
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

Запуск:

```bash
cd benchmarks/Combinatorics.Benchmarks
dotnet run -c Release -- --filter *BinetVsDoubling*
```

## Документация

- **README.md** — визитная карточка, в корне репозитория.
- **docs/** — сайт DocFX, собирается в `docs/_site/`.
- **XML-комментарии** — в исходниках, DocFX читает их через `Combinatorics.xml`.

Публикация: GitHub Actions (`.github/workflows/docs.yml`) собирает DocFX и деплоит на GitHub Pages.

## Зависимости

**Библиотека:** ноль внешних зависимостей. Только `System.Numerics.BigInteger` из BCL.

**Тесты:** xUnit, Microsoft.NET.Test.Sdk.

**Бенчмарки:** BenchmarkDotNet.

**Документация:** DocFX.

## Расширение

### Добавление нового семейства чисел

1. Создайте `Combinatorics.Новое.cs`.
2. Объявите `public static partial class Combinatorics`.
3. Реализуйте методы с XML-комментариями.
4. Добавьте тесты в `tests/Combinatorics.Tests/`.
5. Добавьте бенчмарки в `benchmarks/Combinatorics.Benchmarks/`.
6. Обновите README и `docs/articles/examples.md`.

### Добавление нового форматтера

1. Расширьте `CombinatoricsFormatter.cs`.
2. Используйте `StringBuilder` для сборки строк.
3. Проверьте граничные случаи.
4. Добавьте тесты в `tests/Combinatorics.Tests/FormatterTests.cs`.

## Ссылки

- [README.md](https://github.com/TheGhost1K/combinatorics/blob/main/README.md)
- [Примеры](examples.md)