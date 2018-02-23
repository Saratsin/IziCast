using System;
using System.Threading.Tasks;
using IziCast.Core.Enums;
using MvvmCross.Core.ViewModels;

namespace IziCast.Core.ViewModels
{
    public class OverlayChromecastButtonViewModel : BaseViewModel
    {
        public OverlayChromecastButtonViewModel()
        {
            ConnectButtonClickedCommand = new MvxAsyncCommand(ConnectButtonClicked);
            ConnectButtonLongClickedCommand = new MvxAsyncCommand(ConnectButtonLongClicked);
        }

        private ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MvxAsyncCommand ConnectButtonClickedCommand { get; }

        public MvxAsyncCommand ConnectButtonLongClickedCommand { get; }

        private async Task ConnectButtonClicked()
        {
            System.Diagnostics.Debug.WriteLine("Hello from clicked");
        }

        private async Task ConnectButtonLongClicked()
        {
            System.Diagnostics.Debug.WriteLine("Hello from long clicked");
            Close(this);
        }
    }
}
