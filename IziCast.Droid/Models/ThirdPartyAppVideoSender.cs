using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using IziCast.Core;
using IziCast.Core.Models;
using IziCast.Core.Services.Interfaces;
using MvvmCross;
using MvvmCross.Logging;
using AUri = Android.Net.Uri;

namespace IziCast.Droid
{
	public class ThirdPartyAppVideoSender : IVideoSender
    {
        public ThirdPartyAppVideoSender(string appId, bool isChromecastSender)
        {
			VideoSenderId = appId;
            IsChromecastSender = isChromecastSender;
        }

		public string VideoSenderId { get; }

		public bool IsChromecastSender { get; }

		public async Task<Try> SendVideoAsync(string videoUrl)
		{
			try
            {            
                var intent = new Intent(Intent.ActionView);

				intent.SetDataAndType(AUri.Parse(videoUrl), "video/*");
				intent.SetPackage(VideoSenderId);

                Application.Context.StartActivity(intent);

				await Task.Delay(1000).ConfigureAwait(false);

				return Try.Succeed();
            }
            catch (Exception ex)
            {
                IziCastLog.Instance.Warn(ex, "Failed to send media");
				Mvx.Resolve<IExceptionService>().TrackException(ex);
				return Try.Unsucceed();
            }
		}
    }
}
