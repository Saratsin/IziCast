using Android.Content;
using IziCast.Core;
using IziCast.Core.Bindings;
using IziCast.Core.Enums;
using IziCast.Core.Services;
using IziCast.Droid.Services;
using IziCast.Droid.Widgets;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using static Android.Views.View;

namespace IziCast.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public new IziCastContext ApplicationContext => (IziCastContext)base.ApplicationContext;

        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.LazyConstructAndRegisterSingleton<IMainThreadDispatcherService, MainThreadDispatcherService>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalCastChromecastClient, LocalCastChromecastClient>();
        }

        protected override IMvxApplication CreateApp() => new App();

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            if (ApplicationContext.LaunchMode == LaunchMode.Overlay)
                return new OverlayMvxAndroidViewPresenter(ApplicationContext);
            
            return base.CreateViewPresenter();
        }

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();
    }
}
