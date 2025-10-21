using System;
using System.Linq;

namespace Functional.Programming.With.CSharp.Examples.FunctionComposition;

public static class FunctionCompositionExample
{
    private static string RemoveSpaces(string input) => input.Replace(" ", string.Empty);

    private static string ToUpperCase(string input) => input.ToUpperInvariant();

    private static string Reverse(string input) => new(input.Reverse().ToArray());

    // Compose two functions so the output of the first feeds the second.
    private static Func<T, TResult> Compose<T, TResult, TIntermediate>(
        Func<T, TIntermediate> first,
        Func<TIntermediate, TResult> second) => value => second(first(value));

    public static void Run()
    {
        // Build a transformation pipeline by chaining three small functions.
        var pipeline = Compose(
            Compose<string, string, string>(RemoveSpaces, ToUpperCase),
            Reverse);

        const string original = "functional programming";
        var transformed = pipeline(original);

        Console.WriteLine($"Original: {original}");
        Console.WriteLine($"Transformed: {transformed}");
    }
}
