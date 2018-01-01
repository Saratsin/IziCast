using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using IziCast.Droid.Base.Services;

namespace IziCast.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "rtsp")]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeTypes = new[] { "video/*", "application/sdp" })]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataMimeType = "video/*")]
    public class OverlayChromecastButtonService : MvxOverlayService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);
        }
    }
}
