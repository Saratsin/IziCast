using IziCast.Core;
using IziCast.Core.Enums;
using IziCast.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;

namespace IziCast.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        public new IziCastContext ApplicationContext => (IziCastContext)base.ApplicationContext;

		protected override void InitializePlatformServices()
		{
            base.InitializePlatformServices();

            Mvx.LazyConstructAndRegisterSingleton<ILocalCastChromecastClient, LocalCastChromecastClient>();
		}

		public override void InitializePrimary()
		{
            base.InitializePrimary();

            IziCastLog.Instance.Warn("Primary setup initialization");
		}

		public override void InitializeSecondary()
		{
            base.InitializeSecondary();

            IziCastLog.Instance.Warn("Secondary setup initialization");
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            if (ApplicationContext.LaunchMode == LaunchMode.Overlay)
                return new OverlayMvxAndroidViewPresenter(ApplicationContext);
            
            return base.CreateViewPresenter();
        }
    }
}
