namespace IziCast.Droid.Services
{
	using System;
	using Core;
    using Core.Enums;
	using Core.Sevices;

    public class LaunchModeService : ILaunchModeService
    {
        public event EventHandler LaunchModeWillChange;
        public event EventHandler<LaunchMode> LaunchModeChanged;

        private LaunchMode _launchMode = LaunchMode.Default;
        public LaunchMode LaunchMode
        {
            get => _launchMode;
            set
            {
                if (_launchMode == value)
                    return;

                LaunchModeWillChange.SafeInvoke(this, EventArgs.Empty);
                _launchMode = value;
                LaunchModeChanged.SafeInvoke(this, value);
            }
        }
    }
}
