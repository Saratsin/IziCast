using System;
using System.Globalization;
using System.IO;
using Android.App;
using IziCast.Core.Models;
using IziCast.Core.Services.Interfaces;
using MvvmCross;
using MvvmCross.Converters;

namespace IziCast.Droid.Converters
{
	public class VideoSenderIdConverter : MvxValueConverter<string, string>
    {
		private readonly string _iziCastAppId = Mvx.Resolve<IVideoSenderService>().IziCastAppId;
        private readonly string _sendersIconsFolder = Path.Combine(Application.Context.ExternalCacheDir.AbsolutePath, "SendersIcons");
      
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterHint = (string)parameter;

            var appId = value;

            if (appId.StartsWith(_iziCastAppId, StringComparison.InvariantCultureIgnoreCase))
                appId = _iziCastAppId;

            switch (parameterHint)
            {
                case "ImagePath":
                    return Path.Combine(_sendersIconsFolder, $"{appId}.png");
                case "Text":
					return Application.Context
									  .PackageManager
									  .GetApplicationInfo(appId, Android.Content.PM.PackageInfoFlags.MetaData)
									  .LoadLabel(Application.Context.PackageManager);
                default:
                    return null;
            }
        }
    }
}
