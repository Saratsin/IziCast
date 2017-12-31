using Android.Content;
using IziCast.Core;
using IziCast.Core.Enums;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Platform;

namespace IziCast.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            if (AppLaunchMode.GetMode() != LaunchMode.Overlay)
                return base.CreateViewPresenter();

            return new OverlayMvxAndroidViewPresenter(ApplicationContext);
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
