using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using AUri = Android.Net.Uri;
using IziCast.Droid.Services;
using System.Threading.Tasks;

namespace IziCast.Droid.Views
{

    [Activity(Label = "Izi cast", Theme = "@style/OverlayTheme", MainLauncher = true)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "rtsp")]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeTypes = new[] { "video/*", "application/sdp" })]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataMimeType = "video/*")]
    public class OverlayChromecastButtonServiceActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(OverlayChromecastButtonService));

            intent.SetDataAndType(Intent.Data, Intent.Type);

            Task.Run( () => StartService(intent));

            Finish();
        }
    }
}
