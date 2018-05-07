using System;
using System.Linq;
using System.Threading.Tasks;
using IziCast.Core.Base;
using IziCast.Core.Enums;
using IziCast.Core.Models.IsBusyHandler;
using IziCast.Core.Services;
using MvvmCross.Commands;
using IziCast.Core.Services.Interfaces;

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

		public override async void ViewAppeared()
        {
            //Delay to make Appearing animation visible
            await Task.Delay(1000);

            await ShowChromecastButtonAsyncCommand.ExecuteAsync();

            await Task.Run(AutoCloseIfNeedWithDelay);
		}

		private ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MvxAsyncCommand ConnectButtonClickedCommand => new MvxAsyncCommand(ConnectButtonClicked);

        public MvxAsyncCommand ConnectButtonLongClickedCommand => new MvxAsyncCommand(ConnectButtonLongClicked);

        public MvxAsyncCommand ShowChromecastButtonAsyncCommand { get; set; }

        public MvxAsyncCommand HideChromecastButtonAsyncCommand { get; set; }

        private Task ConnectButtonClicked()
        {
            return Handler.HandleWithDelay(async () =>
            {
                _autoClose = false;

                Status = ConnectivityStatus.Connecting;

                var connectingResult = await _videoSenderService.CurrentChromecastVideoSender.SendVideoAsync(_videoUrl).ConfigureAwait(false);

                if (!connectingResult.OperationSucceeded)
                {
                    Status = ConnectivityStatus.Disconnected;
                    return;
                }

                Status = ConnectivityStatus.Connected;

                await Task.Delay(CloseAfterConnectedDelayInMilliseconds).ConfigureAwait(false);
                await CloseAsync().ConfigureAwait(false);
            });
        }

        private Task ConnectButtonLongClicked()
        {
            return Handler.HandleWithDelay(() => CloseAsync());
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

            await CloseAsync().ConfigureAwait(false);
        }

        private async Task CloseAsync()
        {
            _autoClose = false;

            await HideChromecastButtonAsyncCommand.ExecuteAsync().ConfigureAwait(false);
            await NavigationService.Close(this).ConfigureAwait(false);
        }
    }
}
