using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace IziCast.Droid.Base.Views
{
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
