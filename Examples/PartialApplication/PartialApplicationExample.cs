using System;

namespace Functional.Programming.With.CSharp.Examples.PartialApplication;

public static class PartialApplicationExample
{
    // Fix the tax rate to produce a single-argument calculator (partial application).
    public static Func<decimal, decimal> CreateTaxCalculator(decimal taxRate) =>
        amount => amount * (1 + taxRate);

    public static void Run()
    {
        // Precompute two calculators with different rates for reuse.
        var calculateSalesTax = CreateTaxCalculator(0.08m);
        var calculateVat = CreateTaxCalculator(0.20m);

        const decimal price = 100m;
        Console.WriteLine($"Sales tax total: {calculateSalesTax(price):F2}");
        Console.WriteLine($"VAT total: {calculateVat(price):F2}");
    }
}
