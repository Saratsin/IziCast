

namespace IziCast.Droid.Views
{
	using Android.App;
	using Android.OS;
	using Base;
    using Core.ViewModels;

    [Activity(Label = "MainActivity")]
    public class MainActivity : BaseActivity<FirstViewModel>
    {
        protected override int LayoutResource => Resource.Layout.FirstView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }
    }
}
