namespace IziCast.Droid.Services
{
	using System;
    using Android.App;
    using Android.Content;
    using Android.Runtime;
    using Core.Enums;
    using Core.Services;
	using IziCast.Core.Sevices;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Services;
    using MvvmCross.ViewModels;

    [Service(Exported = true)]
    public class OverlayChromecastButtonService : MvxIntentService
    {
		protected OverlayChromecastButtonService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

        public OverlayChromecastButtonService() : base(nameof(OverlayChromecastButtonService))
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);

            Mvx.RegisterSingleton(this);

			Mvx.Resolve<ILaunchModeService>().LaunchMode = LaunchMode.Overlay;

            var mediaContentUri = intent.DataString;
            var mediaContentType = intent.Type;
            
            Mvx.Resolve<IChromecastClient>().SetMediaData(mediaContentUri, mediaContentType);

            if (Mvx.Resolve<IMvxAppStart>().IsStarted)
                Mvx.Resolve<IMvxAppStart>().ResetStart();

            Mvx.Resolve<IMvxAppStart>().Start();

        }
    }
}
