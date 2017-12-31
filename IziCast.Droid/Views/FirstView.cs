using Android.App;
using Android.OS;
using IziCast.Droid.Base.Views;

namespace IziCast.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : Activity
    {
        //protected override int LayoutResource => Resource.Layout.FirstView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            //SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
