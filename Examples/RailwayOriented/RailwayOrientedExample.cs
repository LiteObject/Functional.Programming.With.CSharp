using System;

namespace Functional.Programming.With.CSharp.Examples.RailwayOriented;

public readonly struct Result<TSuccess, TError>
{
    private readonly TSuccess _success;
    private readonly TError _error;
    public bool IsSuccess { get; }

    private Result(TSuccess success, TError error, bool isSuccess)
    {
        _success = success;
        _error = error;
        IsSuccess = isSuccess;
    }

    public static Result<TSuccess, TError> Success(TSuccess value) => new(value, default!, true);

    public static Result<TSuccess, TError> Failure(TError error) => new(default!, error, false);

    public Result<TNewSuccess, TError> Bind<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TError>> next) =>
        IsSuccess ? next(_success) : Result<TNewSuccess, TError>.Failure(_error);

    public TResult Match<TResult>(Func<TSuccess, TResult> success, Func<TError, TResult> failure) =>
        IsSuccess ? success(_success) : failure(_error);
}

public static class RailwayOrientedExample
{
    private static Result<int, string> Divide(int left, int right) =>
        right == 0
            ? Result<int, string>.Failure("Division by zero")
            : Result<int, string>.Success(left / right);

    public static void Run()
    {
        var goodResult = Result<int, string>.Success(100)
            .Bind(x => Divide(x, 2))
            .Bind(x => Divide(x, 5))
            .Match(
                success: value => $"Success result: {value}",
                failure: error => $"Failure reason: {error}");

        Console.WriteLine(goodResult);

        var badResult = Result<int, string>.Success(10)
            .Bind(x => Divide(x, 0))
            .Match(
                success: value => $"Unexpected success: {value}",
                failure: error => $"Failure reason: {error}");

        Console.WriteLine(badResult);
    }
}
