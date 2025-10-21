# Design Principles: Functional Programming in C#

## Core Philosophy
### Predictability Over Cleverness
The primary goal is writing code that behaves predictably. When you call a function with specific inputs, you should know exactly what will happen—no surprises, no hidden state changes, no mysterious side effects.

### Honest Function Signatures
A function's signature should be a contract that tells the complete truth:
- If it returns `int`, it returns an integer—not sometimes null, not sometimes throws.
- If it can fail, that should be in the return type (`Result<T, TError>`).
- If a value might be missing, use `Option<T>` instead of null.

### Immutability by Default
Treat mutability as a last resort optimization, not the default. Immutable code is thread-safe without locks, easier to reason about, and impossible to corrupt through aliasing.

---
## Refactoring Strategies

### 1. Making Code Immutable
#### Start Small with `readonly`
```csharp
// Mutable - can be changed after creation
public class Account
{
	public decimal Balance { get; set; }
	public string Owner { get; set; }
}

// Immutable - properties cannot change
public class Account
{
	public decimal Balance { get; }
	public string Owner { get; }

	public Account(decimal balance, string owner)
	{
		Balance = balance;
		Owner = owner;
	}
}
```

#### Use "With" Methods for Updates
```csharp
public class Account
{
	public decimal Balance { get; }
	public string Owner { get; }

	public Account WithBalance(decimal newBalance) =>
		new Account(newBalance, Owner);

	public Account Deposit(decimal amount) =>
		WithBalance(Balance + amount);
}
```

#### Leverage C# Records
```csharp
public record Account(decimal Balance, string Owner)
{
	public Account Deposit(decimal amount) =>
		this with { Balance = Balance + amount };
}
```

#### Immutable Collections
```csharp
// Mutable collection - can be modified by anyone with a reference
List<Transaction> transactions = new();

// Immutable - operations return new collections
ImmutableList<Transaction> transactions = ImmutableList<Transaction>.Empty;
transactions = transactions.Add(newTransaction); // Returns new list
```

---

### 2. Replacing Exceptions with Result Types

#### The Problem with Exceptions
```csharp
// Exceptions hide failure modes
public int Divide(int numerator, int denominator)
{
	if (denominator == 0)
		throw new DivideByZeroException();
	return numerator / denominator;
}

var result = Divide(10, userInput); // Runtime explosion possible
```

#### The Result Pattern
```csharp
public Result<int, string> Divide(int numerator, int denominator)
{
	if (denominator == 0)
		return Result<int, string>.Failure("Cannot divide by zero");

	return Result<int, string>.Success(numerator / denominator);
}

var result = Divide(10, userInput)
	.Match(
		success: value => $"Result: {value}",
		failure: error => $"Error: {error}"
	);
```

#### When to Use Exceptions vs Results
- Use exceptions for programming errors (null arguments, index out of range).
- Use results for expected failures (validation, parsing, business rules).

---

### 3. Eliminating Primitive Obsession with Value Objects

#### The Problem
```csharp
public class User
{
	public string Email { get; set; }
	public decimal AccountBalance { get; set; }
	public int Age { get; set; }
}

public void SendEmail(string email, string subject, string body)
{
	// Is 'email' valid? Hard to know without repeating validation.
}
```

#### The Solution: Value Objects
```csharp
public record EmailAddress
{
	public string Value { get; }

	private EmailAddress(string value) => Value = value;

	public static Result<EmailAddress, string> Create(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			return Result<EmailAddress, string>.Failure("Email cannot be empty");

		if (!input.Contains("@"))
			return Result<EmailAddress, string>.Failure("Invalid email format");

		return Result<EmailAddress, string>.Success(new EmailAddress(input));
	}
}

public record Age
{
	public int Value { get; }

	private Age(int value) => Value = value;

	public static Result<Age, string> Create(int input)
	{
		if (input < 0 || input > 150)
			return Result<Age, string>.Failure("Age must be between 0 and 150");

		return Result<Age, string>.Success(new Age(input));
	}
}

public void SendEmail(EmailAddress to, Subject subject, Body body)
{
	// 'to' is guaranteed to be valid.
}
```

---

### 4. Handling Missing Values with Option/Maybe

#### The Null Problem
```csharp
public User FindUser(int id)
{
	return database.Users.FirstOrDefault(u => u.Id == id); // Might return null
}

var user = FindUser(123);
Console.WriteLine(user.Name); // Possible null reference
```

#### The Option Solution
```csharp
public Option<User> FindUser(int id)
{
	var user = database.Users.FirstOrDefault(u => u.Id == id);
	return user != null
		? Option<User>.Some(user)
		: Option<User>.None();
}

var message = FindUser(123)
	.Map(u => u.Name)
	.Match(
		some: name => $"Hello, {name}!",
		none: () => "User not found"
	);
```

---

### 5. Composing Operations with Railway-Oriented Programming

#### Chaining Operations That Can Fail
```csharp
public Result<Order, string> ProcessOrder(string userEmail, string productId, int quantity)
{
	return EmailAddress.Create(userEmail)
		.Bind(email => FindUser(email))
		.Bind(user => FindProduct(productId)
			.Bind(product => CheckStock(product, quantity)
				.Bind(stock => CreateOrder(user, product, quantity))));
}
```

#### Combining Validations
```csharp
public static class Validator
{
	public static Func<T, Result<T, string>> All<T>(params Func<T, Result<T, string>>[] validators)
	{
		return input =>
		{
			foreach (var validator in validators)
			{
				var result = validator(input);
				if (!result.IsSuccess)
					return result;
			}

			return Result<T, string>.Success(input);
		};
	}
}

var validateUser = Validator.All<User>(
	ValidateAge,
	ValidateEmail,
	ValidateName
);
```

---

## Best Practices Summary

### Do
- Make illegal states unrepresentable through types.
- Use immutable data structures by default.
- Return explicit error types instead of throwing exceptions.
- Validate data at system boundaries.
- Compose small, pure functions into larger operations.

### Do Not
- Use null to represent missing values (use `Option<T>`).
- Throw exceptions for expected failures (use `Result<T, TError>`).
- Mutate state unless absolutely necessary for performance.
- Use primitive types for domain concepts (create value objects).
- Hide side effects in functions that appear pure.

### Remember
The goal is to be practical. Use these patterns where they make code clearer, safer, and more maintainable. C# is a multi-paradigm language—choose the approach that best fits the problem.