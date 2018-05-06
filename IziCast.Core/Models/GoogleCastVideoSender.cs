using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using IziCast.Core.Models;
using MvvmCross.Logging;
using MvvmCross;
using IziCast.Core.Services;
using IziCast.Core.Services.Interfaces;

namespace IziCast.Core
{
	public class GoogleCastVideoSender : IVideoSender
    {
        public const string AppId = "com.tsh.izicast.googlecast";

		private readonly DeviceLocator _chromecastLocator = new DeviceLocator();
        private readonly Sender _chromecastSender = new Sender();

        private string _videoSenderAppId;
        public string VideoSenderAppId
        {
            get
            {
                if(_videoSenderAppId == null)
                    _videoSenderAppId = $"{Mvx.Resolve<IVideoSenderService>().IziCastAppId}.googlecast";

                return _videoSenderAppId;
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
