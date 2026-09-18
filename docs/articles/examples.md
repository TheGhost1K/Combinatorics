# Примеры использования

## Покерные руки
var hands = Combinations(52, 5);   // 2 598 960
## Скобочные последовательности
// Сколько правильных скобочных последовательностей из 5 пар?
var catalan = Catalan(5);          // 42
## Формула Бине в точной арифметике
csharp
var phi = Combinatorics.Phi;
var phi2 = QuadraticSurd.Pow(phi, 2);      // (3 + √5)/2
var fib50 = FibonacciBinet(50);            // 12586269025