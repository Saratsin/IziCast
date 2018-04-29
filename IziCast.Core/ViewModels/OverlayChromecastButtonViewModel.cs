using System;
using System.Linq;
using System.Threading.Tasks;
using IziCast.Core.Enums;
using IziCast.Core.Models.IsBusyHandler;
using IziCast.Core.Services;
using MvvmCross.Commands;

namespace IziCast.Core.ViewModels
{
    public class OverlayChromecastButtonViewModel : BaseViewModel
    {
        private const int AutoCloseDelayInMilliseconds = 20000;
        private const int CloseAfterConnectedDelayInMilliseconds = 3000;

        private readonly IChromecastClient _chromecastClient;

        private bool _autoClose = true;

        public OverlayChromecastButtonViewModel(IChromecastClient chromecastClient)
        {
            _chromecastClient = chromecastClient;
        }

        public override void ViewAppeared()
        {
			Task.Run(AutoCloseIfNeedWithDelay);
		}

		private ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MvxAsyncCommand ConnectButtonClickedCommand => new MvxAsyncCommand(ConnectButtonClicked);

        public MvxAsyncCommand ConnectButtonLongClickedCommand => new MvxAsyncCommand(ConnectButtonLongClicked);

        private Task ConnectButtonClicked()
        {
            return Handler.HandleWithDelay(async () =>
            {
                _autoClose = false;

                Status = ConnectivityStatus.Connecting;

                var connectingResult = await _chromecastClient.SendMediaToChromecast();

                if (!connectingResult.OperationSucceeded)
                {
                    Status = ConnectivityStatus.Disconnected;
                    return;
                }

                Status = ConnectivityStatus.Connected;

                await Task.Delay(CloseAfterConnectedDelayInMilliseconds).ConfigureAwait(false);

                Close();
            });
        }

        private Task ConnectButtonLongClicked()
        {
            return Handler.HandleWithDelay(() => Close());
        }

        private async Task AutoCloseIfNeedWithDelay()
        {
            if (!_autoClose)
                return;

            for (int i = 0; i < 10; ++i)
            {
                await Task.Delay(AutoCloseDelayInMilliseconds / 10).ConfigureAwait(false);

                Status = Status == ConnectivityStatus.Connecting ? ConnectivityStatus.Disconnected : ConnectivityStatus.Connecting;

                if (!_autoClose)
                    return;
            }

            Close();
        }

        private void Close()
        {
            _autoClose = false;
            NavigationService.Close(this);
        }
    }
}
