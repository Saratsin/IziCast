using System;
using System.Threading;
using Android.App;
using IziCast.Core.Services;

namespace IziCast.Droid.Services
{
    public class MainThreadDispatcherService : IMainThreadDispatcherService
    {
        public void DispatchOnMainThread(Action action)
        {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
            {
                action.Invoke();
            }
            else
            {
                Application.SynchronizationContext.Post(ignored => action.Invoke(), null);
            }
        }
    }
}
