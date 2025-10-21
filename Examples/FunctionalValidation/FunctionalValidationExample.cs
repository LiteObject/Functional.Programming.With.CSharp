using System;
using System.Linq;

namespace Functional.Programming.With.CSharp.Examples.FunctionalValidation;

public static class FunctionalValidationExample
{
    public delegate (bool isValid, string? error) ValidationRule<in T>(T input);

    public static ValidationRule<T> Combine<T>(params ValidationRule<T>[] rules) => input =>
    {
        var errors = rules
            .Select(rule => rule(input))
            .Where(result => !result.isValid)
            .Select(result => result.error)
            .Where(message => message is not null)
            .Select(message => message!)
            .ToList();

        return errors.Any()
            ? (false, string.Join("; ", errors))
            : (true, null);
    };

    public static ValidationRule<string> NotEmpty => input =>
        string.IsNullOrWhiteSpace(input)
            ? (false, "Value cannot be empty")
            : (true, null);

    public static ValidationRule<string> MinLength(int min) => input =>
        input?.Length < min
            ? (false, $"Minimum length is {min}")
            : (true, null);

    public static void Run()
    {
        var validator = Combine(NotEmpty, MinLength(3));

        var samples = new[] { "", "ab", "abcd" };
        foreach (var sample in samples)
        {
            var (isValid, error) = validator(sample);
            Console.WriteLine(isValid
                ? $"'{sample}' is valid"
                : $"'{sample}' is invalid: {error}");
        }
    }
}
