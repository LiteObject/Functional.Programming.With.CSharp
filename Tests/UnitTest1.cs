using System;
using System.IO;
using System.Linq;
using Functional.Programming.With.CSharp.Examples.FunctionComposition;
using Functional.Programming.With.CSharp.Examples.FunctionalValidation;
using Functional.Programming.With.CSharp.Examples.LazySequences;
using Functional.Programming.With.CSharp.Examples.Memoization;
using Functional.Programming.With.CSharp.Examples.OptionMaybe;
using Functional.Programming.With.CSharp.Examples.PartialApplication;
using Functional.Programming.With.CSharp.Examples.RailwayOriented;
using Xunit;

namespace Functional.Programming.With.CSharp.Tests;

public class FunctionCompositionExampleTests
{
    [Fact]
    public void Run_WritesExpectedTransformation()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            FunctionCompositionExample.Run();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = writer.ToString();
        Assert.Contains("Original: functional programming", output);
        Assert.Contains("Transformed: GNIMMARGORPLANOITCNUF", output);
    }
}

public class PartialApplicationExampleTests
{
    [Fact]
    public void CreateTaxCalculator_ReturnsExpectedAmounts()
    {
        var salesTax = PartialApplicationExample.CreateTaxCalculator(0.08m);
        var vat = PartialApplicationExample.CreateTaxCalculator(0.20m);

        Assert.Equal(108m, salesTax(100m));
        Assert.Equal(120m, vat(100m));
    }
}

public class OptionMaybeExampleTests
{
    [Fact]
    public void ParseInt_ReturnsSomeForValidInput()
    {
        var result = OptionMaybeExample.ParseInt("42");

        var value = result.Match(
            some: x => x,
            none: () => -1);

        Assert.Equal(42, value);
    }

    [Fact]
    public void ParseInt_ReturnsNoneForInvalidInput()
    {
        var result = OptionMaybeExample.ParseInt("invalid");

        var wasSome = result.Match(
            some: _ => true,
            none: () => false);

        Assert.False(wasSome);
    }
}

public class MemoizationExampleTests
{
    [Fact]
    public void Memoize_CachesFunctionResults()
    {
        var callCount = 0;

        int Square(int value)
        {
            callCount++;
            return value * value;
        }

        var memoized = MemoizationExample.Memoize<int, int>(Square);

        var first = memoized(5);
        var second = memoized(5);

        Assert.Equal(25, first);
        Assert.Equal(25, second);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void Run_PrintsFibonacciSequence()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            MemoizationExample.Run();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = writer.ToString();
        Assert.Contains("F(10) = 55", output);
    }
}

public class RailwayOrientedExampleTests
{
    [Fact]
    public void Run_PrintsSuccessAndFailureMessages()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            RailwayOrientedExample.Run();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = writer.ToString();
        Assert.Contains("Success result: 10", output);
        Assert.Contains("Failure reason: Division by zero", output);
    }
}

public class FunctionalValidationExampleTests
{
    [Fact]
    public void Validator_ReturnsErrorsWhenInvalid()
    {
        var validator = FunctionalValidationExample.Combine(
            FunctionalValidationExample.NotEmpty,
            FunctionalValidationExample.MinLength(3));

        var (isValid, error) = validator("ab");

        Assert.False(isValid);
        Assert.Equal("Minimum length is 3", error);
    }

    [Fact]
    public void Validator_PassesWhenValid()
    {
        var validator = FunctionalValidationExample.Combine(
            FunctionalValidationExample.NotEmpty,
            FunctionalValidationExample.MinLength(3));

        var (isValid, error) = validator("abcd");

        Assert.True(isValid);
        Assert.Null(error);
    }
}

public class LazySequencesExampleTests
{
    [Fact]
    public void InfiniteSequence_YieldsIncreasingNumbers()
    {
        var values = LazySequencesExample.InfiniteSequence()
            .Take(5)
            .ToArray();

        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, values);
    }

    [Fact]
    public void FibonacciSequence_YieldsExpectedValues()
    {
        var values = LazySequencesExample.FibonacciSequence()
            .Take(8)
            .ToArray();

        Assert.Equal(new[] { 0, 1, 1, 2, 3, 5, 8, 13 }, values);
    }
}
