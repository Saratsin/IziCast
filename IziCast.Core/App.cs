using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using IziCast.Core.Sevices;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

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
            var appStartMode = Mvx.Resolve<ILaunchModeService>().LaunchMode;

            var viewModelType = typeof(FirstViewModel);

            if (appStartMode == LaunchMode.Overlay)
                viewModelType = typeof(OverlayChromecastButtonViewModel);
            
            Mvx.Resolve<IMvxNavigationService>().Navigate(viewModelType);
		}
	}
}
