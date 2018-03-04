using System;
using System.Threading.Tasks;
using IziCast.Core.Services;
using MvvmCross.Platform;

namespace IziCast.Core.Models.IsBusyHandler
{
    public class IsBusyHandler
    {
        private readonly IMainThreadDispatcherService _mainThreadService = Mvx.Resolve<IMainThreadDispatcherService>();

        public event EventHandler<bool> IsBusyChanged;

        public bool IsBusy { get; private set; }

        public async Task Handle(Func<Task> operation)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                FireIsBusyChangedEvent(true);

                await operation.Invoke().ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
                FireIsBusyChangedEvent(false);
            }
        }

        public async Task<TResult> Handle<TResult>(Func<Task<TResult>> operation)
        {
            if (IsBusy)
                return default(TResult);

			var operationResult = default(TResult);
   
            try
            {
                IsBusy = true;
                FireIsBusyChangedEvent(true);

                operationResult = await operation.Invoke().ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
                FireIsBusyChangedEvent(false);
            }

            return operationResult;
        }

        public async Task SilentHandle(Func<Task> operation)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                await operation.Invoke().ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
    
        public async Task<TResult> SilentHandle<TResult>(Func<Task<TResult>> operation)
        {
            if (IsBusy)
                return default(TResult);

            var operationResult = default(TResult);

            try
            {
                IsBusy = true;

                operationResult = await operation.Invoke().ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }

            return operationResult;
        }

        private void FireIsBusyChangedEvent(bool value)
        {
            var isBusyChanged = IsBusyChanged;

            if (isBusyChanged == null)
                return;

            _mainThreadService.DispatchOnMainThread(() =>
            {
				isBusyChanged.Invoke(this, value);
            });
        }
    }
}
