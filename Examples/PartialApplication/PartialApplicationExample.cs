using System;

namespace Functional.Programming.With.CSharp.Examples.PartialApplication;

public static class PartialApplicationExample
{
    public static Func<decimal, decimal> CreateTaxCalculator(decimal taxRate) =>
        amount => amount * (1 + taxRate);

    public static void Run()
    {
        var calculateSalesTax = CreateTaxCalculator(0.08m);
        var calculateVat = CreateTaxCalculator(0.20m);

        const decimal price = 100m;
        Console.WriteLine($"Sales tax total: {calculateSalesTax(price):F2}");
        Console.WriteLine($"VAT total: {calculateVat(price):F2}");
    }
}
