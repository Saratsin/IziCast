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
        const int AutoCloseDelayInMilliseconds = 10000;
        const int CloseAfterConnectedDelayInMilliseconds = 3000;

        private readonly IChromecastClient _chromecastClient;

        private bool _autoClose = true;

        public OverlayChromecastButtonViewModel(IChromecastClient chromecastClient)
        {
            _chromecastClient = chromecastClient;

            ConnectButtonClickedCommand = new MvxAsyncCommand(ConnectButtonClicked);
            ConnectButtonLongClickedCommand = new MvxAsyncCommand(ConnectButtonLongClicked);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await AutoCloseIfNeedWithDelay().ConfigureAwait(false);
        }

        private ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MvxAsyncCommand ConnectButtonClickedCommand { get; }

        public MvxAsyncCommand ConnectButtonLongClickedCommand { get; }

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

        /*
        public async Task ConnectToChromecast(Connectivity connectivity, string videoUri)
        {
            var client = new Sharpcaster.ChromecastClient();

            try
            {
                connectivity.Status = ConnectivityStatus.Connecting;

                var ipAddress = _ipAddress;
                var chromecasts = await currentService.StartLocatingDevices(ipAddress);
                var chromecast = chromecasts.FirstOrDefault();
                if (chromecast != null)
                {
                    var _controller = default(SharpCasterDemoController);
                    currentService.ChromeCastClient.ConnectedChanged += async (sender, e) =>
                    {
                        try
                        {
                            if (_controller == null)
                                _controller = await currentService.ChromeCastClient.LaunchSharpCaster();
                        }
                        catch (Exception ex)
                        {
                            connectivity.Status = ConnectivityStatus.Disconnected;
                            Debug.WriteLine(ex.Message + ex.StackTrace);
                        }
                    };
                    currentService.ChromeCastClient.ApplicationStarted += async (sender, e) =>
                    {
                        try
                        {
                            while (_controller == null)
                                await Task.Delay(500);

                            var extension = System.IO.Path.GetExtension(videoUri);

                            var contentType = default(string);

                            switch (extension)
                            {
                                case ".mp4":
                                    contentType = "video/mp4";
                                    break;
                                default:
                                    contentType = "video/*";
                                    break;

                            }

                            await _controller.LoadMedia(videoUri, contentType, null);
                            connectivity.Status = ConnectivityStatus.Connected;
                        }
                        catch (Exception ex)
                        {
                            connectivity.Status = ConnectivityStatus.Disconnected;
                            Debug.WriteLine(ex.Message + ex.StackTrace);
                        }
                    };

                    await currentService.ConnectToChromecast(chromecast);
                }
                else
                {
                    connectivity.Status = ConnectivityStatus.Disconnected;
                }
            }
            catch (Exception ex)
            {
                connectivity.Status = ConnectivityStatus.Disconnected;
                Debug.WriteLine(ex.Message + ex.StackTrace);
            }
        }*/
    }
}
