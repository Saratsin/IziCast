using System;
using IziCast.Core.Models.IsBusyHandler;
using MvvmCross.Navigation;

namespace IziCast.Core.Base
{
    public interface IBaseViewModel
    {
        IMvxNavigationService NavigationService { get; }

        IsBusyHandler Handler { get; }

        string Title { get; set; }
    }
}
