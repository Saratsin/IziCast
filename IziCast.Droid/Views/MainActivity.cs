using Android.App;
using Android.Content.PM;
using Android.OS;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base;

namespace IziCast.Droid.Views
{
	[Activity(ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : BaseActivity<MainViewModel>
    {            
		protected override int LayoutId { get; } = Resource.Layout.main_activity;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
		}
	}
}
