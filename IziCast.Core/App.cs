using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
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

            Mvx.RegisterSingleton<IChromecastClient>(new SharpcasterChromecastClient());

            RegisterAppStart(new IziCastAppStart());
        }
    }
}
