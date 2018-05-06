using System;
using IziCast.Core.Models.IsBusyHandler;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
namespace IziCast.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter> : MvxViewModel<TParameter>
    {
        protected IMvxNavigationService NavigationService => Mvx.Resolve<IMvxNavigationService>();

        protected IsBusyHandler Handler { get; } = new IsBusyHandler();

        public virtual string Title { get; set; }
    }
}
