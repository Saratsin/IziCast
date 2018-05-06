using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using IziCast.Core.Enums;
using IziCast.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android.Services;
using MvvmCross.ViewModels;
using IziCast.Core.Sevices;

namespace IziCast.Droid.Services
{
    public class OverlayService : MvxIntentService
    {
		protected OverlayService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

        public OverlayService() : base(nameof(OverlayService))
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);

            if (Mvx.Resolve<IMvxAppStart>().IsStarted)
                Mvx.Resolve<IMvxAppStart>().ResetStart();

            Mvx.Resolve<IMvxAppStart>().Start(new LaunchData(LaunchMode.Overlay, intent.DataString));

        }
    }
}
