using IziCast.Core;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Plugin.Overlay.Platforms.Android.Presenters;
using MvvmCross.IoC;
using System.Linq;
using Android.App;

namespace IziCast.Droid
{
	public class IziCastSetup : MvxAppCompatSetup<App>
    {
		protected override void InitializePlatformServices()
		{
            base.InitializePlatformServices();

            CreatableTypes().EndingWith("Service")
                            .Where(x => !x.IsSubclassOf(typeof(Service)))
                            .AsInterfaces()
                            .RegisterAsLazySingleton();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxOverlayAppCompatViewPresenter(AndroidViewAssemblies);
        }
	}
}
