using System;
using System.Collections.Generic;

namespace Functional.Programming.With.CSharp.Examples.Memoization;

public static class MemoizationExample
{
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
