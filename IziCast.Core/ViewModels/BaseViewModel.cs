using IziCast.Core.Models.IsBusyHandler;
using MvvmCross.Core.ViewModels;

namespace IziCast.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected IsBusyHandler Handler { get; } = new IsBusyHandler();
    }
}
