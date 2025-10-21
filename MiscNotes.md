# Notes on Functional Programming Approach in C#

## Introduction
- **Predictability:** A core goal is to make code more predictable. The easiest way to do that is to ensure that for the same input, a function always returns the same output.
- **No Side Effects:** Functions should not modify any state outside their own scope (e.g., no modifying global variables, no writing to disk or the console).
- **Self-documenting Signatures:** A method's signature should tell you everything you need to know about what it does. If it takes a `string` and returns an `int`, it should *only* be doing a conversion. If it needs to do more, that should be reflected in the types it uses.

## Refactoring to an Immutable Architecture
- **Start with `readonly`:** The simplest step is to make class properties `readonly` wherever possible. This prevents objects from being changed after they are created.
- **"With" methods:** Instead of modifying an object, create a method that returns a *new* object with the updated values. For example, instead of `person.SetName("new name")`, use `var newPerson = person.WithName("new name")`. C# records (`with` keyword) make this much easier.
- **Immutable Collections:** Use `System.Collections.Immutable` collections (`ImmutableList<T>`, `ImmutableDictionary<TKey, TValue>`, etc.). When you "change" them, you get a new collection back, leaving the original untouched. This is great for concurrency.

## Refactoring Away from Exceptions
- **Exceptions are for exceptional situations:** Don't use exceptions for predictable error conditions, like user input validation. Exceptions are side effects that make code harder to follow.
- **Return a result type:** Instead of throwing an exception, have your function return a type that represents both success and failure. A common pattern is a `Result<TSuccess, TError>` object. The caller is then forced to handle both cases. This is the core idea of Railway Oriented Programming.

## Avoiding Primitive Obsession
- **What it is:** "Primitive Obsession" is using primitive types (like `string`, `int`, `decimal`) to represent domain concepts. For example, using a `string` for an email address or a `decimal` for money.
- **Why it's bad:** You can't enforce validation at the type level. Any string can be passed as an email, even if it's not a valid one.
- **The fix:** Create simple, immutable value-object types. For example, an `EmailAddress` class that validates the string in its constructor. This way, if you have an `EmailAddress` object, you *know* it's valid. This makes your method signatures much more honest.

## Avoiding Null with the Maybe/Option Type
- **`null` is the "billion-dollar mistake":** `NullReferenceException` is a common bug. The problem with `null` is that you have to remember to check for it.
- **The `Maybe<T>` (or `Option<T>`) type:** This is a type that explicitly represents the possibility of a missing value. It has two states: `Some(T)` (a value is present) or `None` (no value).
- **How it helps:** When a function returns a `Maybe<T>`, the caller is forced to handle both the `Some` and `None` cases, making it impossible to forget a "null check". It makes the possibility of an absent value explicit in the function's signature.

## Handling Failure and Input Errors in a Functional Way
- **Combine `Maybe` and `Result`:** Use `Maybe<T>` for values that are optional, and `Result<TSuccess, TError>` for operations that can fail.
- **Validation as a function:** Create small, reusable validation functions. For example, `IsNotEmpty(string)`, `IsValidEmail(string)`.
- **Chain or compose validations:** You can chain these validation functions together. If any one of them fails, the chain is short-circuited and returns a failure result. This is another application of Railway Oriented Programming. This keeps your validation logic clean and separate from your business logic.