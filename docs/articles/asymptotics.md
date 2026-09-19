# Асимптотики

Приближённые формулы для больших n — O(1) вместо точных мегабайт.

## Зачем нужны

Для `n = 10 000` точное `n!` занимает ~35 000 знаков. Приближение — те же 8 байт `double`. Если нужна оценка (например, для сравнения или выбора алгоритма), приближение в миллионы раз быстрее.

## Базовая и расширенная

| Метод | Погрешность |
|---|---|
| `FactorialStirling` | O(1/n) |
| `FactorialStirlingRefined` | O(1/n³) |
| `FactorialStirlingExtended` | O(1/n⁴) |
| `LogFactorialLanczos` | ~10⁻¹⁵ (машинная) |
| `CatalanApprox` | O(1/n) |
| `CatalanApproxExtended` | O(1/n⁴) |
| `FibonacciApprox` | O(φ⁻²ⁿ) — очень быстро |
| `FibonacciApproxExtended` | машинная для F(n) < 2⁵³ |
| `BellApprox` | O(1/n) |
| `BellApproxExtended` | O(1/n²) |

## Формулы

### Факториал (Стирлинг)

```
n! ≈ √(2πn) · (n/e)^n · (1 + 1/(12n) + 1/(288n²) − 139/(51840n³) − …)
```

```csharp
FactorialStirling(100);            // 9.33e157 — базовая
FactorialStirlingRefined(100);     // + 1/(12n)
FactorialStirlingExtended(100);    // + 3 члена
LogFactorialLanczos(100);          // ln(100!) с машинной точностью
```

### Каталан

```
C_n ≈ 4^n / (n^{3/2} · √π) · (1 − 9/(8n) + 145/(128n²) − …)
```

```csharp
CatalanApprox(100);            // 8.96e56
CatalanApproxExtended(100);    // точнее
```

### Фибоначчи (Бине)

```
F_n = (φ^n − ψ^n) / √5
```

Так как `|ψ| < 1`, второе слагаемое быстро стремится к нулю:

```csharp
FibonacciApprox(50);            // 1.2586e10
FibonacciApproxExtended(50);    // полная формула в double
LucasApprox(50);
LucasApproxExtended(50);
```

### Белл (через функцию Ламберта W)

```
B_n ≈ n^{−1/2} · (n/W(n))^{n+1/2} · exp(n/W(n) − n − 1)
```

```csharp
BellApprox(50);            // 1.86e47
BellApproxExtended(50);    // + поправка 1/(12W(n))
```

### Сочетания

```
C(n, k) ≈ n^n / (k^k · (n−k)^{n−k}) · √(n / (2π · k · (n−k)))
```

```csharp
CombinationsApprox(1000, 500);   // 2.70e299
```

### Деланнуа, Шрёдер, Моцкин

```csharp
DelannoyCentralApprox(100);              // ~1.5e76
DelannoyCentralApproxExtended(100);      // точнее
SchroederLargeApprox(100);
SchroederLargeApproxExtended(100);
MotzkinApprox(100);
```

## Оценка погрешности

```csharp
var exact = Factorial(20);
var approx = FactorialStirling(20);
var err = RelativeError(exact, approx);
// ~0.0004 (0.04%)

var approxExtended = FactorialStirlingExtended(20);
var errExtended = RelativeError(exact, approxExtended);
// ~1e-10
```

**`RelativeError(exact, approx)`** возвращает `|approx − exact| / exact`.

## Специальные функции

### Функция Ламберта W

Решение уравнения `W·e^W = x`. Используется в асимптотике Белла.

```csharp
LambertW(1.0);       // 0.5671432904
LambertW(Math.E);    // 1.0
LambertW(1e15);      // ~26.36
```

**Метод Галлея** — кубическая сходимость, точность ~10⁻¹⁵.

### Гамма Ланцоша

Аппроксимация гамма-функции с точностью ~10⁻¹⁵.

```csharp
GammaLanczos(5.0);     // 24 = 4!
GammaLanczos(0.5);     // √π ≈ 1.7724
GammaLanczos(2.5);     // 3√π/4 ≈ 1.3293

LogGammaLanczos(100);  // ln(99!) ≈ 359.13
```

## Точность на практике

| n | Stirling | Refined | Extended | Lanczos |
|---|---|---|---|---|
| 10 | 0.8% | 8e-4% | 1e-8% | 1e-14% |
| 20 | 0.4% | 1e-4% | 1e-10% | 1e-15% |
| 50 | 0.2% | 7e-6% | 1e-13% | 1e-15% |
| 100 | 0.1% | 8e-7% | 1e-15% | 1e-15% |

## См. также

- [Числа Каталана](catalan.md)
- [Стирлинг и Белл](stirling-bell.md)
- [Фибоначчи и Люка](fibonacci-lucas.md)
- [Формулы Бине](binet.md)