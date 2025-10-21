using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional.Programming.With.CSharp.Examples.LazySequences;

public static class LazySequencesExample
{
    // Yield an unbounded arithmetic progression starting at the given value.
    public static IEnumerable<int> InfiniteSequence(int start = 0)
    {
        var current = start;
        while (true)
        {
            yield return current++;
        }
    }

    // Generate Fibonacci numbers lazily so callers can Take as many as they need.
    public static IEnumerable<int> FibonacciSequence()
    {
        var current = 0;
        var next = 1;
        while (true)
        {
            yield return current;
            (current, next) = (next, current + next);
        }
    }

    private static bool IsPrime(int value)
    {
        if (value < 2)
        {
            return false;
        }

        var limit = (int)Math.Sqrt(value);
        for (var i = 2; i <= limit; i++)
        {
            if (value % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    public static void Run()
    {
        // Lazy pipelines only evaluate the numbers requested by Take.
        var firstTenPrimes = InfiniteSequence(2)
            .Where(IsPrime)
            .Take(10)
            .ToArray();

        Console.WriteLine("First ten primes: " + string.Join(", ", firstTenPrimes));

        var firstTenFibonacci = FibonacciSequence()
            .Take(10)
            .ToArray();

        Console.WriteLine("First ten fibonacci numbers: " + string.Join(", ", firstTenFibonacci));
    }
}
