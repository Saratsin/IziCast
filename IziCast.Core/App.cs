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
            //Sharpcaster.Logging.LogProvider.SetCurrentLogProvider(SharpCasterLogProvider.Instance);
            #endif
            AppCenter.Start("de4a737b-6ba2-4692-a3e7-bcaa691eafeb", typeof(Crashes), typeof(Analytics));

            CreatableTypes().EndingWith("Service")
                            .AsInterfaces()
                            .RegisterAsLazySingleton();

			Mvx.RegisterSingleton(new GoogleCastVideoSender());
   
            RegisterCustomAppStart<AppStart>();
        }

		public override void Startup(object hint)
		{
            var launchData = hint as LaunchData;

            var viewModelType = typeof(MainViewModel);

            if (launchData != null && launchData.Mode == LaunchMode.Overlay)
                viewModelType = typeof(ChromecastButtonViewModel);
            
            Mvx.Resolve<IMvxNavigationService>().Navigate(viewModelType);
		}
	}
}
