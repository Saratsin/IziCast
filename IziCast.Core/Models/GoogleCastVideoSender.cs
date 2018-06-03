using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using IziCast.Core.Models;
using MvvmCross.Logging;
using MvvmCross;
using IziCast.Core.Services.Interfaces;
using MvvmCross.Converters;
using System.Globalization;

namespace IziCast.Core
{   
	public class GoogleCastVideoSender : IVideoSender
    {      
		private readonly DeviceLocator _chromecastLocator = new DeviceLocator();
        private readonly Sender _chromecastSender = new Sender();

		private string _videoSenderId;
        public string VideoSenderId
        {
            get
            {
                if(_videoSenderId == null)
                    _videoSenderId = $"{Mvx.Resolve<IVideoSenderService>().IziCastAppId}.googlecast";

                return _videoSenderId;
            }
        }

		public bool IsChromecastSender { get; } = true;

        public async Task<Try> SendVideoAsync(string videoUrl)
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

                var status = await mediaChannel.LoadAsync(new MediaInformation
                {
					ContentId = videoUrl,
					ContentType = "video/*"
                }).ConfigureAwait(false);

                return Try.Succeed();
            }
            catch (Exception ex)
            {
                IziCastLog.Instance.Warn(ex, "Failed to send media");
                return Try.Unsucceed();
            }
		}
	}
}
