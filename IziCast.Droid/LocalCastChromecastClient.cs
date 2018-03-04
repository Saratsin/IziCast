using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using IziCast.Core.Models;
using IziCast.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using AUri = Android.Net.Uri;

namespace IziCast.Droid
{
    public class LocalCastChromecastClient : ILocalCastChromecastClient
    {
        private const string LocalCastAppPackageName = "de.stefanpledl.localcast";

        private string _mediaContentUrl;
        private string _mediaContentType;

        private PackageManager PackageManager => Application.Context.PackageManager;

        private bool IsLocalCastInstalled()
        {
            var installedPackages = PackageManager.GetInstalledPackages(PackageInfoFlags.Activities);

            return installedPackages.Any(p => p.ApplicationInfo.PackageName == LocalCastAppPackageName);
        }

        public void SetMediaData(string mediaContentUrl, string mediaContentType)
        {
            _mediaContentUrl = mediaContentUrl;
            _mediaContentType = mediaContentType;
        }

        public Task<Try> SendMediaToChromecast()
        {
            try
            {
                if (IsLocalCastInstalled())
                    return Task.FromResult(Try.Unsucceed());

                var intent = new Intent(Intent.ActionView);

                intent.SetDataAndType(AUri.Parse(_mediaContentUrl), _mediaContentType);
                intent.SetPackage(LocalCastAppPackageName);

                Application.Context.StartActivity(intent);

                return Task.FromResult(Try.Succeed());
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IMvxLog>().Warn(ex, "Failed to send media");
                return Task.FromResult(Try.Unsucceed());
            }
        }
    }
}
