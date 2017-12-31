
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Content.PM;
using MvvmCross.Droid.Support.V7.AppCompat;
using IziCast.Core.ViewModels;
using IziCast.Droid.Services;
using IziCast.Droid.Base.Services;

namespace IziCast.Droid.Views
{
    [Activity(Label = "Izi cast", Theme = "@style/Theme.Transparent", ScreenOrientation = ScreenOrientation.Landscape)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "rtsp")]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeTypes = new[] { "video/*", "application/sdp" })]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataMimeType = "video/*")]
    public class OverlayChromecastButtonActivity : AppCompatActivity
    {
        string uri;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Finish();

            var intent = new Intent(Intent);
            intent.SetComponent(null);
            intent.SetPackage("com.mxtech.videoplayer.ad");
            StartActivity(intent);
            uri = intent.DataString;

            var overlayButtonServiceIntent = new Intent(this, typeof(MvxOverlayService));
            //var overlayButtonServiceConnection = new OverlayButtonServiceConnection();
            //BindService(overlayButtonServiceIntent, overlayButtonServiceConnection, Bind.AutoCreate);
            //if (MainApplication.Current.Core.ValidateUri(uri))
            //{
            //    await Task.Delay(1000);
            //    await ChromecastButtonService.CallService(this, ButtonClickAction, 10000);
            //}
            //else
            //    uri = null;
        }

        public void CallBackgroundService()
        {
        }

        //Task ButtonClickAction(Core.Connectivity connectivity)
        //{
        //    return MainApplication.Current.Core.ConnectToChromecast(connectivity, uri);
        //}
    }
}
