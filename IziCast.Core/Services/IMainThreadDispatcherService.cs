using System;
namespace IziCast.Core.Services
{
    public interface IMainThreadDispatcherService
    {
        void DispatchOnMainThread(Action action);
    }
}
