# Functional Programming in C# - Examples

This repository is a collection of C# examples that demonstrate functional programming concepts. If you're coming from an object-oriented background, these examples aim to show you how to write cleaner, more predictable, and more maintainable code by applying functional principles.

## What's in this repository?

The `/Examples` directory contains code for the following concepts:

*   **Function Composition:** Combining simple functions to create more complex ones.
*   **Functional Validation:** A declarative way to validate data.
*   **Memoization:** Caching the results of expensive function calls.
*   **Option/Maybe:** Handling the absence of a value without using `null`.
*   **Partial Application:** Creating new functions by pre-filling some of the arguments of an existing function.
*   **Railway Oriented Programming:** A way to handle errors and validation in a functional style.

Feel free to explore the code and run the examples.

---

## Core Functional Concepts

Here's a quick refresher on some of the key ideas of functional programming that you'll see in the examples.

### 1. Pure Functions

A pure function is a function that, given the same input, will always return the same output and doesn't have any observable side effects. This makes them very predictable.

For example, this `Multiply` function is pure. It doesn't change any state outside of its own scope.

```csharp
int Multiply(int a, int b)
{
    return a * b;
}
```

### 2. Immutability

Immutability means that once an object is created, it cannot be changed. This helps to prevent bugs and makes your code easier to reason about, especially in multi-threaded scenarios.

In C#, you can use the `readonly` modifier to make properties immutable.

```csharp
public class Person
{
    public readonly string Name;

    public Person(string name)
    {
        Name = name;
    }
}
```
Once a `Person` is created, its `Name` can't be changed.

### 3. Higher-Order Functions

A higher-order function is a function that can take another function as an argument, or a function that returns a function. C# has great support for this with LINQ, delegates, and lambda expressions.

A great example is LINQ's `Select` method, which takes a function as an argument to transform a list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
List<int> squaredNumbers = numbers.Select(x => x * x).ToList();
```
Here, `x => x * x` is a lambda expression (an anonymous function) that we pass to `Select`.

### 4. Immutable Data Structures

Functional programming prefers data structures that cannot be changed. The `System.Collections.Immutable` namespace in .NET provides collections like `ImmutableList<T>`. When you "add" an item to an immutable list, it returns a *new* list with the item added, leaving the original list unchanged.

```csharp
ImmutableList<int> numbers = ImmutableList.Create(1, 2, 3, 4, 5);
ImmutableList<int> doubledNumbers = numbers.Select(x => x * 2).ToImmutableList();
```
The original `numbers` list is not modified.

## Functional vs. Imperative Example

Here’s a simple comparison of summing even numbers in a list, written in a functional style and an imperative (non-functional) style.

**Functional Approach (using LINQ):**
```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

int sumOfEvenNumbers = numbers.Where(x => x % 2 == 0).Sum();
Console.WriteLine("Sum of even numbers (functional): " + sumOfEvenNumbers);
```
This code is declarative. It describes *what* you want to do (filter for even numbers, then sum them), not *how* to do it step-by-step.

**Imperative Approach:**
```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

int sumOfEvenNumbers = 0;
foreach (int number in numbers)
{
    if (number % 2 == 0)
    {
        sumOfEvenNumbers += number;
    }
}
Console.WriteLine("Sum of even numbers (non-functional): " + sumOfEvenNumbers);
```
This code is procedural. It gives the computer a sequence of steps to follow, using a loop and a mutable variable (`sumOfEvenNumbers`).

## Conclusion

By using pure functions, immutability, and higher-order functions, you can write C# code that is more robust, easier to debug, and often more concise.

Happy coding!