using IziCast.Core.Base;
using IziCast.Core.Services.Interfaces;
using System.Collections.ObjectModel;
using IziCast.Core.Models;
using MvvmCross.Commands;
using System.Threading.Tasks;
using System;
using IziCast.Core.Enums;
using IziCast.Core.Models.IsBusyHandler;

namespace IziCast.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
    {      
		private readonly IVideoSenderService _videoSenderService;

		public MainViewModel(IVideoSenderService videoSenderService)
		{
			_videoSenderService = videoSenderService;         
		}
            
        public bool SendDataButtonIsEnabled => !string.IsNullOrEmpty(DataUrl) && Uri.IsWellFormedUriString(DataUrl, UriKind.Absolute);

        public bool DataUrlEntryIsEnabled => SendDataButtonStatus != ConnectivityStatus.Connecting;

        private string _dataUrl;
        public string DataUrl
        {
            get => _dataUrl;
            set
            {
                if (!SetProperty(ref _dataUrl, value))
                    return;

                RaisePropertyChanged(nameof(SendDataButtonIsEnabled));
				SendDataButtonStatus = ConnectivityStatus.Disconnected;
            }
        }

        private ConnectivityStatus _sendDataButtonStatus = ConnectivityStatus.Disconnected;
        public ConnectivityStatus SendDataButtonStatus
        {
            get => _sendDataButtonStatus;
            private set
            {
                if (!SetProperty(ref _sendDataButtonStatus, value))
                    return;

                RaisePropertyChanged(nameof(DataUrlEntryIsEnabled));
            }
        }

        public ReadOnlyCollection<IVideoSender> VideoPlayers => _videoSenderService.PhoneVideoSenders;

        public ReadOnlyCollection<IVideoSender> ChromecastPlayers => _videoSenderService.ChromecastVideoSenders;

        public IVideoSender CurrentVideoPlayer
        {
            get => _videoSenderService.CurrentPhoneVideoSender;
            set
            {
                if (_videoSenderService.CurrentPhoneVideoSender == value)
                    return;

                _videoSenderService.CurrentPhoneVideoSender = value;
                RaisePropertyChanged();
            }
        }

        public IVideoSender CurrentChromecastPlayer
        {
            get => _videoSenderService.CurrentChromecastVideoSender;
            set
            {
                if (_videoSenderService.CurrentChromecastVideoSender == value)
                    return;

                _videoSenderService.CurrentChromecastVideoSender = value;
                RaisePropertyChanged();
                var a = true;
                if (a)
                {
                    throw new Exception("Player Exception for testing purposes");
                }

            }
        }

        public MvxAsyncCommand SendDataUrlToChromecastCommand => new MvxAsyncCommand(SendDataUrlToChromecast);

        public MvxAsyncCommand AboutButtonClickedCommand => new MvxAsyncCommand(AboutButtonClicked);

        private Task SendDataUrlToChromecast()
        {
            return Handler.SilentHandleWithDelay(async () =>
            {
                if (SendDataButtonStatus != ConnectivityStatus.Disconnected)
                    return;

                SendDataButtonStatus = ConnectivityStatus.Connecting;

                var sendingResult = await CurrentChromecastPlayer.SendVideoAsync(DataUrl).ConfigureAwait(false);

                SendDataButtonStatus = sendingResult.OperationSucceeded ? ConnectivityStatus.Connected : ConnectivityStatus.Disconnected;
            });
        }

        private Task AboutButtonClicked()
		{
			return Handler.HandleWithDelay(() =>
			{
				return NavigationService.Navigate<AboutViewModel>();
			});
		}
	}
}
