using Android.App;
using Android.OS;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base;

namespace IziCast.Droid.Views
{
    [Activity(Label = "Izi cast", MainLauncher = true)]
	public class MainActivity : BaseActivity<MainViewModel>
    {            
		protected override int LayoutId { get; } = Resource.Layout.main_activity;
    }
}
