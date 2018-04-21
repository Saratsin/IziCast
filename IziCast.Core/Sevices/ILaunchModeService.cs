using System;
using IziCast.Core.Enums;
namespace IziCast.Core.Sevices
{
    public interface ILaunchModeService
    {
        event EventHandler LaunchModeWillChange;
        event EventHandler<LaunchMode> LaunchModeChanged;

        LaunchMode LaunchMode { get; set; }
    }
}
