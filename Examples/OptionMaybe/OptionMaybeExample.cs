using System;

namespace Functional.Programming.With.CSharp.Examples.OptionMaybe;

public readonly struct Option<T>
{
    private readonly T _value;
    public bool HasValue { get; }

    private Option(T value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    public static Option<T> Some(T value) => new(value, true);

    public static Option<T> None() => new(default!, false);

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) =>
        HasValue ? some(_value) : none();

    public Option<TResult> Map<TResult>(Func<T, TResult> mapper) =>
        HasValue ? Option<TResult>.Some(mapper(_value)) : Option<TResult>.None();
}

public static class OptionMaybeExample
{
    public static Option<int> ParseInt(string input) =>
        int.TryParse(input, out var result)
            ? Option<int>.Some(result)
            : Option<int>.None();

    public static void Run()
    {
        var values = new[] { "100", "abc", "250" };

        foreach (var value in values)
        {
            var output = ParseInt(value)
                .Map(x => x * 2)
                .Match(
                    some: x => $"Parsed {value} -> {x}",
                    none: () => $"Failed to parse {value}");
            Console.WriteLine(output);
        }
    }
}
