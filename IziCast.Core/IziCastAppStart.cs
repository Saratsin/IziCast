using IziCast.Core.Enums;
using IziCast.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace IziCast.Core
{
    public class IziCastAppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            var appStartMode = hint as LaunchMode?;

            if (appStartMode == LaunchMode.Overlay)
                ShowViewModel<OverlayChromecastButtonViewModel>();
            else
                ShowViewModel<FirstViewModel>();
        }
    }
}
