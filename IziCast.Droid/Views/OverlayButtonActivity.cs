
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Support.V7.App;
//using Android.Views;
//using Android.Widget;
//using System.Threading.Tasks;
//using Android.Content.PM;
//using MvvmCross.Droid.Support.V7.AppCompat;
//using IziCast.Core.ViewModels;

//namespace IziCast.Droid.Views
//{
//    [Activity(Label = "Izi cast", Theme = "@style/Theme.Transparent", ScreenOrientation = ScreenOrientation.Landscape)]
//    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "rtsp")]
//    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeTypes = new[] { "video/*", "application/sdp" })]
//    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataMimeType = "video/*")]
//    public class OverlayButtonActivity : MvxAppCompatActivity<OverlayButtonViewModel>
//    {
//        string uri;

//        protected override async void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);
//            Finish();

//            var intent = new Intent(Intent);
//            intent.SetComponent(null);
//            intent.SetPackage("com.mxtech.videoplayer.ad");
//            intent.PutExtra("orientation", 0);
//            StartActivity(intent);
//            uri = intent.DataString;
//            if (MainApplication.Current.Core.ValidateUri(uri))
//            {
//                await Task.Delay(1000);
//                await ChromecastButtonService.CallService(this, ButtonClickAction, 10000);
//            }
//            else
//                uri = null;
//        }

//        Task ButtonClickAction(Core.Connectivity connectivity)
//        {
//            return MainApplication.Current.Core.ConnectToChromecast(connectivity, uri);
//        }
//    }
//}
