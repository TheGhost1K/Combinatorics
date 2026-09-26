# Красивый вывод

Класс `CombinatoricsFormatter` — утилиты для форматирования и печати результатов.

## Подключение

```csharp
using static Combinatorics.Combinatorics;
using static Combinatorics.CombinatoricsFormatter;
```

## Форматирование последовательностей

```csharp
FormatSequence(CatalanSequence(6));
// "1, 1, 2, 5, 14, 42"

FormatSequenceIndexed(BellSequence(5));
// "0:1, 1:1, 2:2, 3:5, 4:15"

FormatSequence(PellSequence(11), separator: " | ");
// "0 | 1 | 2 | 5 | 12 | 29 | 70 | 169 | 408 | 985 | 2378"
```

## Компактный формат для больших чисел

```csharp
FormatCompact(42);                    // "42"
FormatCompact(1500);                  // "1.50K"
FormatCompact(2_500_000);             // "2.50M"
FormatCompact(1_000_000_000);         // "1.00B"
FormatCompact(Factorial(20));         // "2.43e18"
FormatCompact(FibonacciFast(1000));   // "4.35e208"

FormatScientific(Factorial(20));      // "2.43e18"
FormatScientific(Factorial(20), 5);   // "2.43290e18"
```

## Рациональные числа

```csharp
var b6 = Bernoulli(6);                         // 1/42

FormatRational(b6);                            // "1/42"
FormatRational(b6, RationalMode.Decimal);      // "0.023810"
FormatRational(b6, RationalMode.Both);         // "1/42 (≈ 0.023810)"
FormatRational(Bernoulli(12), RationalMode.Both, 8);
// "-691/2730 (≈ -0.25311355)"
```

**`RationalMode`:** `Fraction`, `Decimal`, `Both`.

## Таблицы

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

### Две колонки рядом

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
// |    2 |                         2 |                         2 |
// ...
```

## Треугольники

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
```

## Колонки

```csharp
Console.WriteLine(FormatColumns(MotzkinSequence(20), columns: 5));
// 0:1         4:9         8:323       12:15511    16:142547
// 1:1         5:21        9:835       13:41835    17:747261
// 2:2         6:51        10:2188     14:113634   18:4005811
// 3:4         7:127       11:5798     15:310572   19:21776331
```

## Рамка

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

## Разделители тысяч

```csharp
FormatThousands(Factorial(20));
// "2 432 902 008 176 640 000"

FormatThousands(Bell(20));
// "51 724 158 235 372"
```

## Печать в консоль

```csharp
// Простой вывод
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

// Сравнение с асимптотикой
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

## Полный пример

```csharp
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

## Сводная таблица методов

| Метод | Что делает |
|---|---|
| `FormatSequence` | Последовательность через запятую |
| `FormatSequenceIndexed` | С индексами |
| `FormatCompact` | K/M/B/e-нотация |
| `FormatScientific` | Научная нотация |
| `FormatRational` | Дробь/десятичное/оба |
| `FormatTable` | Таблица «n → значение» |
| `FormatTableDouble` | Две колонки |
| `FormatTriangle` | Треугольник |
| `FormatColumns` | В несколько колонок |
| `FormatThousands` | Разделители тысяч |
| `Box` | Рамка вокруг текста |
| `PrintResult` | `label = value` |
| `PrintCompact` | Компактно |
| `PrintScientific` | Научно |
| `PrintRational` | Дробь |
| `PrintSequence` | Последовательность |
| `PrintTable` | Таблица |
| `PrintApproxVsExact` | Точное vs приближённое |
| `PrintHeader` | Заголовок |
| `PrintSubheader` | Подзаголовок |

## Зачем это нужно

Числа вроде `2432902008176640000` или `9.33e157` тяжело воспринимать глазами. Форматтер превращает их в читаемые строки: `2.43e18`, `2 432 902 008 176 640 000`, компактные таблицы, треугольники. Это полезно при отладке, в демонстрациях, в Jupyter-ноутбуках и в образовательных материалах.

## Как это устроено

Все методы форматтера — **чистые функции**: получают значения, возвращают строку. Никакого состояния, никаких сайд-эффектов. Методы `Print*` пишут в `Console` — тоже без состояния.

Внутри используется `StringBuilder` для эффективной сборки строк и LINQ для краткости.

### Компактный формат

`FormatCompact` анализирует величину числа через `BigInteger.ToString().Length` — это даёт число цифр (порядок величины). Далее выбирается формат: K/M/B/e-нотация.

```csharp
1_000        → "1.00K"
1_000_000    → "1.00M"
1_000_000_000 → "1.00B"
10^12        → "1.00e12"
```

### Формат таблицы

Ширина колонок рассчитывается по максимальному значению среди элементов. Рамка рисуется через `+`, `-`, `|`. Заголовки центрируются по правому краю.

### Формат треугольника

Каждая строка сдвигается вправо на `(maxCols − row.Length) · (width/2)` пробелов — это центрирует треугольник.

## Где это использовать

| Сценарий | Что использовать |
|---|---|
| Отладка в IDE | `PrintResult`, `PrintCompact` |
| Демонстрация в консоли | `PrintHeader`, `PrintSequence`, `FormatTable` |
| Jupyter-ноутбук | `FormatTriangle`, `FormatTable` |
| Логи | `FormatCompact`, `FormatScientific` |
| Образовательные материалы | `FormatTriangle`, `Box` |
| Тесты (проверка значений) | `FormatSequence` |

## Примеры

### Демонстрация

```csharp
PrintHeader("Числа Каталана");
PrintSequence("C(n)", CatalanSequence(11));
PrintSequence("Narayana(6)", NarayanaRow(6).Skip(1));
```

### Отладка

```csharp
var huge = Factorial(100);
Console.WriteLine($"100! = {FormatCompact(huge)}");  // 9.33e157
Console.WriteLine($"100! = {FormatScientific(huge, 5)}");  // 9.33262e157
```

### Логи

```csharp
void Log(string label, BigInteger value)
{
    Console.WriteLine($"[LOG] {label} = {FormatCompact(value)}");
}
```

### Jupyter

```csharp
// Возвращаем строку для отображения в ячейке
FormatTriangle(Enumerable.Range(0, 10).Select(PascalRow))
```

## Производительность

Форматтер работает с `StringBuilder` и LINQ — быстро, но **не в горячем пути**. Если вы печатаете миллионы строк в цикле, это будет медленнее, чем `Console.WriteLine(value)`. Для обычных сценариев разница незаметна.

## Типичные ошибки

**1. Использовать форматтер для точных вычислений.**

Форматтер **не изменяет значение**, только представляет. Если нужно сравнить два BigInteger, используйте сами BigInteger, а не их строковые представления.

**2. Забыть `using static Combinatorics.CombinatoricsFormatter;`.**

Без этого придётся писать `CombinatoricsFormatter.FormatSequence(...)`. Со `using static` — просто `FormatSequence(...)`.

**3. Ожидать, что `FormatCompact` даёт точное значение.**

`FormatCompact(1234567890)` возвращает `"1.23B"` — точность до 3 знаков. Для точного вывода используйте `FormatThousands` или `value.ToString()`.

**4. Использовать `RationalMode.Decimal` для огромных дробей.**

`Bernoulli(100).ToDecimalString(20)` может занять много времени — знаменатели у чисел Бернулли огромные. Для отображения обычно хватает `Fraction`.

**5. Пытаться форматировать `null`.**

Методы бросают `ArgumentNullException` на `null`. Проверяйте входные данные, если они могут быть `null`.

## См. также

- [Примеры](examples.md)