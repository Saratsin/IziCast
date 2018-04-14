using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using System.ComponentModel;

namespace IziCast.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes().EndingWith("Service")
                            .AsInterfaces()
                            .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IChromecastClient>(new SharpcasterChromecastClient());

            RegisterCustomAppStart<AppStart>();
        }

		public override void Startup(object hint)
		{
            var appStartMode = hint as LaunchMode?;

            var viewModelType = typeof(FirstViewModel);

            if (appStartMode == LaunchMode.Overlay)
                viewModelType = typeof(OverlayChromecastButtonViewModel);
            
            Mvx.Resolve<IMvxNavigationService>().Navigate(viewModelType);
		}
	}
}
