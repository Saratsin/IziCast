using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using IziCast.Core.Models;
using MvvmCross.Logging;

namespace IziCast.Core.Services
{
    public class GoogleCastChromecastClient : IChromecastClient
    {
        private readonly DeviceLocator _chromecastLocator = new DeviceLocator();
        private readonly Sender _chromecastSender = new Sender();

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

                await _chromecastSender.ConnectAsync(chromecast).ConfigureAwait(false);

                var mediaChannel = _chromecastSender.GetChannel<IMediaChannel>();

                await _chromecastSender.LaunchAsync(mediaChannel).ConfigureAwait(false);

                await mediaChannel.LoadAsync(new MediaInformation
                {
                    ContentId = _mediaContentUrl,
                    ContentType = _mediaContentType
                }).ConfigureAwait(false);

                return Try.Succeed();
            }
            catch(Exception ex)
            {
                IziCastLog.Instance.Warn(ex, "Failed to send media");
                return Try.Unsucceed();
            }
        }
    }
}
