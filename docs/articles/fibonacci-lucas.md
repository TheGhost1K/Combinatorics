# Числа Фибоначчи и Люка

Классика рекуррентных последовательностей.

## Числа Фибоначчи

**Определение:** `F_0 = 0`, `F_1 = 1`, `F_n = F_{n−1} + F_{n−2}`.

**Формула Бине:** `F_n = (φ^n − ψ^n) / √5`, где `φ = (1+√5)/2`, `ψ = (1−√5)/2`.

**OEIS:** [A000045](https://oeis.org/A000045).

**Первые значения:** 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, …

```csharp
Fibonacci(0);        // 0
Fibonacci(10);       // 55
Fibonacci(30);       // 832040
Fibonacci(50);       // 12586269025

FibonacciSequence(16);
// 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610
```

### Быстрое удвоение O(log n)

```csharp
FibonacciFast(50);    // 12586269025
FibonacciFast(100);   // 354224848179261915075
FibonacciFast(1000);  // 4.35e208

// Пара (F(n), F(n+1))
var (fn, fn1) = FibonacciPair(10);  // (55, 89)
```

## Числа Люка

**Определение:** `L_0 = 2`, `L_1 = 1`, `L_n = L_{n−1} + L_{n−2}`.

**Формула Бине:** `L_n = φ^n + ψ^n`.

**OEIS:** [A000032](https://oeis.org/A000032).

**Первые значения:** 2, 1, 3, 4, 7, 11, 18, 29, 47, 76, 123, …

```csharp
Lucas(0);           // 2
Lucas(10);          // 123
LucasSequence(11);  // 2, 1, 3, 4, 7, 11, 18, 29, 47, 76, 123
```

## Тождества

### Кассини

`F(n+1)² − F(n)·F(n+2) = (−1)^n`.

```csharp
CassiniIdentity(10);   // true
```

### Люка

`L(n)² − 5·F(n)² = 4·(−1)^n`.

```csharp
LucasIdentity(10);   // true
```

### Формула удвоения

`F(2n) = F(n)·L(n)`.

```csharp
DoublingFormula(10);   // true
```

### Связь Люка с Фибоначчи

`L(n) = F(n−1) + F(n+1)`.

```csharp
LucasFromFibonacci(10);   // true
```

### gcd-свойство

`gcd(F(m), F(n)) = F(gcd(m, n))`.

## Асимптотики

```csharp
FibonacciApprox(50);            // 1.2586e10
FibonacciApproxExtended(70);    // машинная точность для F(n) < 2^53
LucasApprox(50);                // 2.8e10
LucasApproxExtended(50);
```

## Формулы Бине в точной арифметике

Поле Q(√5) позволяет вычислять точно:

```csharp
FibonacciBinet(50);   // 12586269025
LucasBinet(10);       // 123
```

## См. также

- [Числа Пелля](pell.md)
- [Формулы Бине](binet.md)
- [Асимптотики](asymptotics.md)
- [OEIS A000045](https://oeis.org/A000045)
- [OEIS A000032](https://oeis.org/A000032)