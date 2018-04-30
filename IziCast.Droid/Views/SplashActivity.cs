using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Views;

namespace IziCast.Droid
{
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
	}
}
