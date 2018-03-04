using System;
namespace IziCast.Core.Models
{
    public class TryResult<TResult> : Try
    {
        public TryResult(bool operationSucceeded, TResult result = default(TResult)) : base(operationSucceeded)
        {
            Result = result;
        }

        public TResult Result { get; }
    }

    public class TryResult
    {
        public static TryResult<TResult> Create<TResult>(bool operationSucceeded, TResult result) => new TryResult<TResult>(operationSucceeded, result);

        public static TryResult<TResult> Succeed<TResult>(TResult result) => new TryResult<TResult>(true, result);

        public static TryResult<TResult> Unsucceed<TResult>() => new TryResult<TResult>(false);
    }
}
