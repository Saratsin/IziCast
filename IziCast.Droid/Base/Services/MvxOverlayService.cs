using Android.Content;
using Android.Runtime;
using IziCast.Core.Enums;
using MvvmCross;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android.Services;

namespace IziCast.Droid.Base.Services
{
    [Register("izicast.droid.services.MvxOverlayService")]
    public abstract class MvxOverlayService : MvxIntentService, IMvxOverlayService
    {
        public MvxOverlayService() : base(nameof(MvxOverlayService))
        {
        }

        private Context _context;
        public Context Context
        {
            get
            {
                if (_context == null)
                    _context = IziCastContext.FromApplicationContext(Resource.Style.OverlayTheme, LaunchMode.Overlay);

                return _context;
            }
        }

        public override Context ApplicationContext => Context;

        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);

            Mvx.RegisterSingleton<IMvxOverlayService>(this);
            Mvx.Resolve<IMvxAppStart>().Start(LaunchMode.Overlay);
        }
    }
}
