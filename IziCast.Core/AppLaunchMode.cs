using System;
using IziCast.Core.Enums;

namespace IziCast.Core
{
    public static class AppLaunchMode
    {
        private static LaunchMode? _mode;

        public static LaunchMode GetMode()
        {
            if (_mode == null)
                throw new Exception("Launch mode hasn't been set. That should never happen");

            return _mode.Value;
        }

        public static void SetMode(IAppLaunchModeSetter setter) => _mode = setter.LaunchMode;
    }
}
