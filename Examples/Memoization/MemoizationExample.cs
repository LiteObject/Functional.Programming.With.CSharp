using System;
using System.Collections.Generic;

namespace Functional.Programming.With.CSharp.Examples.Memoization;

public static class MemoizationExample
{
    // Wrap a function so results are cached per input value.
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
        where T : notnull
    {
        var cache = new Dictionary<T, TResult>();
        return value =>
        {
            if (cache.TryGetValue(value, out var cached))
            {
                return cached;
            }

            var result = func(value);
            cache[value] = result;
            return result;
        };
    }

    // Memoize the naive recursive Fibonacci to avoid exponential recomputation.
    private static readonly Func<int, long> MemoizedFibonacci = Memoize<int, long>(Fibonacci);

    private static long Fibonacci(int n) =>
        n switch
        {
            0 => 0,
            1 => 1,
            _ => MemoizedFibonacci(n - 1) + MemoizedFibonacci(n - 2)
        };

    public static void Run()
    {
        for (var i = 0; i <= 10; i++)
        {
            Console.WriteLine($"F({i}) = {MemoizedFibonacci(i)}");
        }
    }
}
