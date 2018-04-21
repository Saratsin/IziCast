
namespace IziCast.Droid
{
	using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.OS;
	using MvvmCross.Platforms.Android.Views;
	using MvvmCross;
	using Services;
	using LaunchMode = Core.Enums.LaunchMode;
    using IziCast.Core.Sevices;

    [Activity(Icon = "@mipmap/ic_launcher",
              Label = "IziCast",
              MainLauncher = false, 
              NoHistory = true, 
	          ScreenOrientation = ScreenOrientation.Portrait,
              Theme = "@style/Theme.Splash")]
    public class SplashActivity : MvxSplashScreenActivity
    {
		public SplashActivity() : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);

            Mvx.Resolve<ILaunchModeService>().LaunchMode = LaunchMode.Default;
		}
	}
}
