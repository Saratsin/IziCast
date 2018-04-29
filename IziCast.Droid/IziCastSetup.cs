using IziCast.Core;
using IziCast.Core.Services;
using IziCast.Core.Sevices;
using IziCast.Droid.Services;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;

namespace IziCast.Droid
{
    public class IziCastSetup : MvxAppCompatSetup<App>
    {

		protected override void InitializePlatformServices()
		{
            Mvx.LazyConstructAndRegisterSingleton<ILaunchModeService, LaunchModeService>();
			Mvx.LazyConstructAndRegisterSingleton<ILocalCastChromecastClient, LocalCastChromecastClient>();

            base.InitializePlatformServices();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new IziCastAndroidViewPresenter(AndroidViewAssemblies);
        }

		protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
		{
            base.FillTargetFactories(registry);
		}
	}
}
