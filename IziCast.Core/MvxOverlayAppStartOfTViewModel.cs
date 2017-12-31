using MvvmCross.Core.ViewModels;

namespace IziCast.Core
{
    public class MvxOverlayAppStart<TViewModel> : MvxNavigatingObject, IMvxAppStart where TViewModel : IMvxViewModel
    {
        public void Start(object hint = null)
        {
            ShowViewModel<TViewModel>();
        }
    }
}
