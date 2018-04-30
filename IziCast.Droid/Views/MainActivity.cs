using Android.App;
using Android.OS;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base;

namespace IziCast.Droid.Views
{
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
