﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using IziCast.Core.Enums;
using IziCast.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android.Services;
using MvvmCross.ViewModels;

namespace IziCast.Droid.Services
{
    [Service(Exported = true)]
    public class ChromecastButtonOverlayService : MvxIntentService
    {
		protected ChromecastButtonOverlayService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

        public ChromecastButtonOverlayService() : base(nameof(ChromecastButtonOverlayService))
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);

            var mediaContentUri = intent.DataString;
            var mediaContentType = intent.Type;
            
            Mvx.Resolve<IChromecastClient>().SetMediaData(mediaContentUri, mediaContentType);

            if (Mvx.Resolve<IMvxAppStart>().IsStarted)
                Mvx.Resolve<IMvxAppStart>().ResetStart();

            Mvx.Resolve<IMvxAppStart>().Start(LaunchMode.Overlay);

        }
    }
}