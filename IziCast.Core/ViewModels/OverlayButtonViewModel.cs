using System;
using IziCast.Core.Enums;

namespace IziCast.Core.ViewModels
{
    public class OverlayButtonViewModel : BaseViewModel
    {
        ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public OverlayButtonViewModel()
        {
        }
    }
}
