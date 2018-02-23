using Android.App;
using Android.Content;
using IziCast.Droid.Base.Services;

namespace IziCast.Droid.Services
{
    [Service(Exported = true)]
    public class OverlayChromecastButtonService : MvxOverlayService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);
        }
    }
}
