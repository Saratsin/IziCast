using IziCast.Core;
using IziCast.Core.Services;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Plugin.Overlay.Platforms.Android.Presenters;

namespace IziCast.Droid
{
    public class IziCastSetup : MvxAppCompatSetup<App>
    {
		protected override void InitializePlatformServices()
		{
			Mvx.LazyConstructAndRegisterSingleton<IChromecastClient, LocalCastChromecastClient>();

            base.InitializePlatformServices();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxOverlayAppCompatViewPresenter(AndroidViewAssemblies);
        }
	}
}
