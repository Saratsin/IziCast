using System;
using Android.Content;
using Android.Runtime;
using IziCast.Core.Enums;
using IziCast.Core.Models;
using MvvmCross;
using MvvmCross.Platforms.Android.Services;
using MvvmCross.ViewModels;
using Android.App;

namespace IziCast.Droid.Services
{
    [Service]
    public class OverlayIntentService : MvxIntentService
    {
		protected OverlayIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

        public OverlayIntentService() : base(nameof(OverlayIntentService))
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
