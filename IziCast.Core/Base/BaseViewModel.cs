using IziCast.Core.Models.IsBusyHandler;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace IziCast.Core.Base
{
    public abstract class BaseViewModel : MvxViewModel, IBaseViewModel
    {
        public IMvxNavigationService NavigationService => Mvx.Resolve<IMvxNavigationService>();

        public IsBusyHandler Handler { get; } = new IsBusyHandler();

		public virtual string Title { get; set; }
    }
}
