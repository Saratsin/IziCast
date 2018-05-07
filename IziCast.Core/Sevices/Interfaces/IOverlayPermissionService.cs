using System;
using System.Threading.Tasks;

namespace IziCast.Core.Sevices.Interfaces
{
    public interface IOverlayPermissionService
    {
        Task<bool> TryEnablePermissionIfDisabledAsync();
    }
}
