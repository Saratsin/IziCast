using System;
using System.Linq;
using System.Threading.Tasks;
using IziCast.Core.Models;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using Sharpcaster;
using Sharpcaster.Core.Interfaces;
using Sharpcaster.Discovery;

namespace IziCast.Core.Services
{
    public class SharpcasterChromecastClient : IChromecastClient
    {
        private readonly IChromecastLocator _chromecastLocator = new MdnsChromecastLocator();
        private readonly ChromecastClient _chromecastClient = new ChromecastClient();

        private string _mediaContentUrl;
        private string _mediaContentType;

        public void SetMediaData(string mediaContentUrl, string mediaContentType)
        {
            _mediaContentUrl = mediaContentUrl;
            _mediaContentType = mediaContentType;
        }

        public async Task<Try> SendMediaToChromecast()
        {
            try
            {
                var chromecasts = await _chromecastLocator.FindReceiversAsync().ConfigureAwait(false);

                if (!chromecasts.Any())
                    return Try.Unsucceed();

                var chromecast = chromecasts.First();

                await _chromecastClient.ConnectChromecast(chromecast).ConfigureAwait(false);

                var mediaChannel = _chromecastClient.GetChannel<IMediaChannel>();

                await mediaChannel.LoadAsync(new Sharpcaster.Core.Models.Media.Media
                {
                    ContentUrl = _mediaContentUrl,
                    ContentType = _mediaContentType
                }).ConfigureAwait(false);

                return Try.Succeed();
            }
            catch(Exception ex)
            {
                Mvx.Resolve<IMvxLog>().Warn(ex, "Failed to send media");
                return Try.Unsucceed();
            }
        }
    }
}
