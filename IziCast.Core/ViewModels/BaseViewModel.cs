using IziCast.Core.Models.IsBusyHandler;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using MvvmCross;

namespace IziCast.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected IMvxNavigationService NavigationService => Mvx.Resolve<IMvxNavigationService>();

        protected IsBusyHandler Handler { get; } = new IsBusyHandler();
    }
}
