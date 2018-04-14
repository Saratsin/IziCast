using Android.App;
using Android.Content;
using Android.Widget;
using IziCast.Core.Services;
using IziCast.Droid.Base.Services;
using MvvmCross;

namespace IziCast.Droid.Services
{
    [Service(Exported = true)]
    public class OverlayChromecastButtonService : MvxOverlayService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);

            var mediaContentUri = intent.DataString;
            var mediaContentType = intent.Type;

            Mvx.Resolve<IChromecastClient>().SetMediaData(mediaContentUri, mediaContentType);
        }
    }
}
