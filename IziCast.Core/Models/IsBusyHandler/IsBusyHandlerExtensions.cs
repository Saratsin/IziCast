using System;
using System.Threading.Tasks;

namespace IziCast.Core.Models.IsBusyHandler
{
    public static class IsBusyHandlerExtensions
    {
        private const int OperationDefaultDelayInMilliseconds = 300;

        #region Handle with delay
        public static Task HandleWithDelay(this IsBusyHandler handler, Action operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.Handle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                operation.Invoke();

                await delayTask.ConfigureAwait(false);
            });
        }

        public static Task<TResult> HandleWithDelay<TResult>(this IsBusyHandler handler, Func<TResult> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.Handle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationResult = operation.Invoke();

                await delayTask.ConfigureAwait(false);

                return operationResult;
            });
        }

        public static Task HandleWithDelay(this IsBusyHandler handler, Func<Task> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.Handle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);
            });
        }

        public static Task<TResult> HandleWithDelay<TResult>(this IsBusyHandler handler, Func<Task<TResult>> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.Handle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);

                return operationTask.Result;
            });
        }
        #endregion

        #region Silent handle with delay
        public static Task SilentHandleWithDelay(this IsBusyHandler handler, Action operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.SilentHandle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                operation.Invoke();

                await delayTask.ConfigureAwait(false);
            });
        }

        public static Task<TResult> SilentHandleWithDelay<TResult>(this IsBusyHandler handler, Func<TResult> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.SilentHandle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationResult = operation.Invoke();

                await delayTask.ConfigureAwait(false);

                return operationResult;
            });
        }

        public static Task SilentHandleWithDelay(this IsBusyHandler handler, Func<Task> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.SilentHandle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);
            });
        }

        public static Task<TResult> SilentHandleWithDelay<TResult>(this IsBusyHandler handler, Func<Task<TResult>> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return handler.SilentHandle(async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);

                return operationTask.Result;
            });
        }
        #endregion

        #region TryResult handles
        public static async Task<TryResult<TResult>> Handle<TResult>(this IsBusyHandler handler, Func<Task<TryResult<TResult>>> operation)
        {
            var result = await handler.Handle(operation).ConfigureAwait(false);

            if (result == default(TryResult<TResult>))
                result = TryResult.Unsucceed<TResult>();

            return result;
        }

        public static async Task<TryResult<TResult>> SilentHandle<TResult>(this IsBusyHandler handler, Func<Task<TryResult<TResult>>> operation)
        {
            var result = await handler.SilentHandle(operation).ConfigureAwait(false);

            if (result == default(TryResult<TResult>))
                result = TryResult.Unsucceed<TResult>();

            return result;
        }

        public static Task<TryResult<TResult>> HandleWithDelay<TResult>(this IsBusyHandler handler, Func<Task<TryResult<TResult>>> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return Handle(handler, async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);

                return operationTask.Result;
            });
        }

        public static Task<TryResult<TResult>> SilentHandleWithDelay<TResult>(this IsBusyHandler handler, Func<Task<TryResult<TResult>>> operation, int delayInMilliseconds = OperationDefaultDelayInMilliseconds)
        {
            return SilentHandle(handler, async () =>
            {
                var delayTask = Task.Delay(delayInMilliseconds);
                var operationTask = operation.Invoke();

                await Task.WhenAll(operationTask, delayTask).ConfigureAwait(false);

                return operationTask.Result;
            });
        }
        #endregion
    }
}
