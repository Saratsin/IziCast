using System;
using System.Threading.Tasks;
using IziCast.Core.Sevices.Interfaces;
using MvxOverlayPermissionService = MvvmCross.Plugin.Overlay.Platforms.Android.Services.OverlayPermissionService;

namespace IziCast.Droid.Services
{
    public class OverlayPermissionService : IOverlayPermissionService
    {
        public Task<bool> TryEnablePermissionIfDisabledAsync()
        {
            return MvxOverlayPermissionService.Instance.TryEnablePermissionIfDisabled(TimeSpan.FromMinutes(1));
        }
    }
}
