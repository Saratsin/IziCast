using System;
using Android.Content;
using Android.Views;
using IziCast.Core;
using IziCast.Core.Enums;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace IziCast.Droid.Base.Services
{
    public abstract class MvxOverlayService : MvxStickyIntentService, IMvxOverlayService, IAppLaunchModeSetter
    {
        private Context _context;
        protected override Context Context
        {
            get
            {
                if(_context == null)
                    _context = new ContextThemeWrapper(ApplicationContext, Resource.Style.OverlayTheme);
                
                return _context;
            }
        }

        LaunchMode IAppLaunchModeSetter.LaunchMode { get; } = LaunchMode.Overlay;

        public override void OnCreate()
        {
            AppLaunchMode.SetMode(this);

            base.OnCreate();
        //protected override void OnHandleIntent(Intent intent)
        //{
            //AppLaunchMode.SetMode(this);

            //base.OnHandleIntent(intent);

            if (Mvx.CanResolve<IMvxOverlayService>())
                throw new Exception("IMvxOverlayService singleton is already registered. That should never happen");

            Mvx.RegisterSingleton<IMvxOverlayService>(this);
            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}
