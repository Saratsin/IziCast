using IziCast.Core;
using IziCast.Core.Enums;
using IziCast.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Droid.Support.V7.AppCompat;
using IziCast.Droid.Services;
using MvvmCross.Platforms.Android.Core;
using IziCast.Core.Sevices;
using MvvmCross.WeakSubscription;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Bindings.Target;
using IziCast.Droid.Widgets;

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
