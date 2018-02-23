using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using AUri = Android.Net.Uri;

namespace IziCast.Droid.Services
{
    public class OverlayPermissionService
    {
        private readonly Context _context;

        public OverlayPermissionService()
        {
            _context = Application.Context;
            Instance = this;
        }

        public static OverlayPermissionService Instance { get; private set; } = new OverlayPermissionService();

        public bool CanDrawOverlays => Settings.CanDrawOverlays(_context);

        public async Task<bool> TryEnablePermissionIfDisabled(TimeSpan timeout)
        {
            if (CanDrawOverlays)
                return true;

            await ShowOverlayPermissionScreenAsync().ConfigureAwait(false);

            await WaitTillPermissionIsEnabled(timeout).ConfigureAwait(false);

            return CanDrawOverlays;
        }

        private async Task WaitTillPermissionIsEnabled(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);

            while (!CanDrawOverlays || cts.Token.IsCancellationRequested)
                await Task.Delay(500).ConfigureAwait(false);
        }

        private async Task ShowOverlayPermissionScreenAsync()
        {
            var noPermissionText = "Ooops, we don't have permission to show you our beuatiful app button. Please enable this permissions to enjoy our app features)";
            await UserInteractionService.Instance.ShowToastAsync(noPermissionText, ToastLength.Long).ConfigureAwait(false);
            var intent = new Intent(Settings.ActionManageOverlayPermission, AUri.FromParts("package", _context.PackageName, null));
            intent.AddFlags(ActivityFlags.NewTask);
            _context.StartActivity(intent);
        }
    }
}
