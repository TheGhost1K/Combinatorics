# Архитектура

## Слои

- `BigRational.cs` — точная рациональная арифметика
- `QuadraticSurd.cs` — точная арифметика в Q(√d)
- `Combinatorics.*.cs` — partial-класс с методами для каждой группы чисел

Все файлы `Combinatorics.*.cs` — это `partial class Combinatorics`,
разбитый на логические группы.