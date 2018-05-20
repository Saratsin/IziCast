using System;
using System.Threading.Tasks;
using IziCast.Core.Sevices.Interfaces;
using MvxOverlayPermissionService = MvvmCross.Plugin.Overlay.Platforms.Android.Services.OverlayPermissionService;
using MvvmCross.Localization;

namespace IziCast.Droid.Services
{
    public class OverlayPermissionService : IOverlayPermissionService
    {
        public Task<bool> TryEnablePermissionIfDisabledAsync()
        {
			var text = new MvxLanguageBinder(string.Empty, nameof(OverlayPermissionService)).GetText("NeedToEnablePermission");

			return MvxOverlayPermissionService.Instance.TryEnablePermissionIfDisabled(TimeSpan.FromMinutes(1), text);
        }
    }
}
