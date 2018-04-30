using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Core.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace IziCast.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            #if DEBUG
            AppCenter.LogLevel = LogLevel.Verbose;
            #endif

            AppCenter.Start("de4a737b-6ba2-4692-a3e7-bcaa691eafeb", typeof(Crashes), typeof(Analytics));

            CreatableTypes().EndingWith("Service")
                            .AsInterfaces()
                            .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IChromecastClient>(new SharpcasterChromecastClient());

            RegisterCustomAppStart<AppStart>();
        }

		public override void Startup(object hint)
		{
            var appStartMode = hint as LaunchMode? ?? LaunchMode.Default;

            var viewModelType = typeof(FirstViewModel);

            if (appStartMode == LaunchMode.Overlay)
                viewModelType = typeof(ChromecastButtonOverlayViewModel);
            
            Mvx.Resolve<IMvxNavigationService>().Navigate(viewModelType);
		}
	}
}
