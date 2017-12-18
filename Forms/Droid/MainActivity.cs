using Android.App;
using Android.Content.PM;
using Android.OS;

namespace IziCast.Droid
{
	[Activity(Label = "Izi Cast", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);
			Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App(ShowFloatingOverlay));
		}

		async void ShowFloatingOverlay()
		{
			await ChromecastButtonService.CallService(this, null, 5000);
		}
	}
}