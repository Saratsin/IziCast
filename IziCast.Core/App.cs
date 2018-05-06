using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Core.Sevices;
using IziCast.Core.Sevices.Interfaces;
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

			Mvx.RegisterSingleton(new GoogleCastVideoSender());
   
            RegisterCustomAppStart<AppStart>();
        }

		public override async void Startup(object hint)
		{
            var launchData = hint as LaunchData;
			var videoSenderService = Mvx.Resolve<IVideoSenderService>();
            var navigationService = Mvx.Resolve<IMvxNavigationService>();

            if (launchData == null || launchData.Mode == LaunchMode.Default)
            {
                await navigationService.Navigate<MainViewModel>().ConfigureAwait(false);
                return;
            }


            var phoneVideoSenderIsAvailable = await videoSenderService.EnsureAtLeastOneVideoSenderIsAvailable().ConfigureAwait(false);

            if (!phoneVideoSenderIsAvailable)
                return;

            await videoSenderService.CurrentPhoneVideoSender.SendVideoAsync(launchData.ContentUrl).ConfigureAwait(false);
            await navigationService.Navigate<ChromecastButtonViewModel, string>(launchData.ContentUrl).ConfigureAwait(false);
		}
	}
}
