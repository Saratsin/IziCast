using System;
using System.Linq;
using System.Threading.Tasks;
using IziCast.Core.Base;
using IziCast.Core.Enums;
using IziCast.Core.Models.IsBusyHandler;
using IziCast.Core.Services;
using MvvmCross.Commands;
using IziCast.Core.Sevices;
using IziCast.Core.Sevices.Interfaces;

namespace IziCast.Core.ViewModels
{
    public class ChromecastButtonViewModel : BaseViewModel<string>
    {
        private const int AutoCloseDelayInMilliseconds = 20000;
        private const int CloseAfterConnectedDelayInMilliseconds = 3000;

        private readonly IVideoSenderService _videoSenderService;

        private bool _autoClose = true;
        private string _videoUrl;

        public ChromecastButtonViewModel(IVideoSenderService videoSenderService)
        {
            _videoSenderService = videoSenderService;
        }

        public override void Prepare(string parameter)
        {
            _videoUrl = parameter;
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

                var connectingResult = await _videoSenderService.CurrentChromecastVideoSender.SendVideoAsync(_videoUrl);

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
