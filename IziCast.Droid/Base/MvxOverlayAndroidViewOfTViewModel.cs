using Android.Content;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace IziCast.Droid.Base
{
    [MvxOverlayPresentation]
    public abstract class MvxOverlayAndroidView<TViewModel> : MvxOverlayAndroidView, IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxOverlayAndroidView(Context context) : base(context)
        {
        }

        public new TViewModel ViewModel
        {
            get => base.ViewModel as TViewModel;
            set => base.ViewModel = value;
        }
    }
}
