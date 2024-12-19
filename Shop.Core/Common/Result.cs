using System.Diagnostics;

namespace Shop.Core;

public static class Result
{
    public static void Switch<TValue, TError>(this Result<TValue, TError> result, Action<TValue> success,
        Action<TError> failure)
    {
        _ = result.Match(val =>
        {
            success(val);
            return Unit.Value;
        }, err =>
        {
            failure(err);
            return Unit.Value;
        });
    }

    public static Result<TValue, TError> Tap<TValue, TError>(
        this Result<TValue, TError> result,
        Action<TValue> tap
    )
    {
        return result.Map(val =>
        {
            tap(val);
            return val;
        });
    }

    public static Result<TValue2, TError> Map<TValue1, TValue2, TError>(
        this Result<TValue1, TError> result,
        Func<TValue1, TValue2> map
    )
    {
        return result.Match<Result<TValue2, TError>>(val => map(val), err => err);
    }


    public static Result<TValue2, TError> Bind<TValue1, TValue2, TError>(
        this Result<TValue1, TError> result,
        Func<TValue1, Result<TValue2, TError>> map
    )
    {
        return result.Match(map, err => err);
    }
}

public abstract class Result<TValue, TError>
{
    private Result(bool ok)
    {
        IsSuccess = ok;
        IsFailure = !ok;
    }

    public bool IsSuccess { get; }
    public bool IsFailure { get; }

    public abstract TMatch Match<TMatch>(Func<TValue, TMatch> succeed, Func<TError, TMatch> fail);

    public static implicit operator Result<TValue, TError>(TValue val)
    {
        return new Success(val);
    }

    public static implicit operator Result<TValue, TError>(TError err)
    {
        return new Failure(err);
    }

    [DebuggerDisplay(nameof(_value))]
    private class Success(TValue value) : Result<TValue, TError>(true)
    {
        private readonly TValue _value = value;

        public override TOut Match<TOut>(Func<TValue, TOut> succeed, Func<TError, TOut> fail)
        {
            return succeed(_value);
        }
    }

    [DebuggerDisplay(nameof(_error))]
    private class Failure(TError error) : Result<TValue, TError>(false)
    {
        private readonly TError _error = error;

        public override TOut Match<TOut>(Func<TValue, TOut> succeed, Func<TError, TOut> fail)
        {
            return fail(_error);
        }
    }
}