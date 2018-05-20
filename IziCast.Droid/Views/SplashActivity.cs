using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace IziCast.Droid
{
    [Activity(Icon = "@mipmap/ic_launcher",
              Label = "Izi cast",
              MainLauncher = true, 
              NoHistory = true, 
	          ScreenOrientation = ScreenOrientation.Portrait,
              Theme = "@style/Theme.Splash")]
    public class SplashActivity : MvxSplashScreenActivity
    {
		public SplashActivity() : base(Resource.Layout.splash_screen_activity)
        {
        }
	}
}
