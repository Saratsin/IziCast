using IziCast.Core.Enums;
using IziCast.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace IziCast.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();


            if (AppLaunchMode.GetMode() != LaunchMode.Overlay)
            {
				RegisterNavigationServiceAppStart<FirstViewModel>();
                return;
            }

            RegisterAppStart(new MvxOverlayAppStart<OverlayChromecastButtonViewModel>());
        }
    }
}
