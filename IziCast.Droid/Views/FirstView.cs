using Android.App;
using Android.Content;
using Android.OS;
using IziCast.Droid.Base.Views;
using IziCast.Droid.Services;

namespace IziCast.Droid.Views
{
    [Activity(Label = "View for FirstViewModel", MainLauncher = true)]
    public class FirstView : Activity
    {
        //protected override int LayoutResource => Resource.Layout.FirstView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartService(new Intent(this, typeof(OverlayChromecastButtonService)));

			Finish();

            //SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
