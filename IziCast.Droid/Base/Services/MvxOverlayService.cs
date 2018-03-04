using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using IziCast.Core;
using IziCast.Core.Enums;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;

namespace IziCast.Droid.Base.Services
{
    [Register("izicast.droid.services.MvxOverlayService")]
    public abstract class MvxOverlayService : StickyIntentService, IMvxOverlayService
    {
        private IziCastContext _context;
        protected IziCastContext Context
        {
            get
            {
                if(_context == null)
                    _context = IziCastContext.FromApplicationContext(Resource.Style.OverlayTheme, LaunchMode.Overlay);
                
                return _context;
            }
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Context);
            setup.EnsureInitialized();

            Mvx.RegisterSingleton<IMvxOverlayService>(this);
            Mvx.Resolve<IMvxAppStart>().Start(LaunchMode.Overlay);
        }
    }
}
