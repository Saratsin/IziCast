using Android.Content;
using IziCast.Core;
using IziCast.Core.Bindings;
using IziCast.Core.Enums;
using IziCast.Droid.Widgets;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
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
