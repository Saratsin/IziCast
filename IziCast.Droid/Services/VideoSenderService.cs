using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using IziCast.Core;
using IziCast.Core.Models;
using IziCast.Core.Services.Interfaces;
using MvvmCross;
using MvvmCross.Localization;
using Android.Graphics.Drawables;
using Android.Graphics;
using Path = System.IO.Path;
using System.IO;
using System.Collections.Generic;
using IziCast.Droid.Extensions;
using AUri = Android.Net.Uri;

namespace IziCast.Droid.Services
{
	public class VideoSenderService : IVideoSenderService
    {
		private static readonly string[] _chromecastSupportedAppNames =
		{
			"de.stefanpledl.localcast",
			"dkc.video.vcast"
		};

        private readonly IUserInteractionService _userInteractionService;
        private readonly ISettingsService _settingsService;

        public VideoSenderService(ISettingsService settingsService, IUserInteractionService userInteractionService)
        {
            _settingsService = settingsService;
            _userInteractionService = userInteractionService;
        }

		private PackageManager PackageManager => Application.Context.PackageManager;

        private Intent _viewVideoIntent;
        private Intent ViewVideoIntent
		{
			get
			{
				if (_viewVideoIntent == null)
				{
                    _viewVideoIntent = new Intent(Intent.ActionView);
					_viewVideoIntent.SetDataAndType(AUri.Parse("http://devimages.apple.com/iphone/samples/bipbop/bipbopall.m3u8"), "video/*");
				}

				return _viewVideoIntent;
			}
		}

        private string _iziCastAppId;
        public string IziCastAppId
        {
            get
            {
                if (_iziCastAppId == null)
                    _iziCastAppId = Application.Context.PackageName;

                return _iziCastAppId;
            }
        }
        
        private IVideoSender _currentPhoneVideoSender;
        public IVideoSender CurrentPhoneVideoSender
        {
            get
            {
                if(_currentPhoneVideoSender == null)
                    _currentPhoneVideoSender = CreateCurrentPhoneVideoSender();
                
                return _currentPhoneVideoSender;
            }
            set
            {
                if (_currentPhoneVideoSender == value)
                    return;
                
                _currentPhoneVideoSender = value;
                _settingsService.SetValue(nameof(CurrentPhoneVideoSender), value.VideoSenderId);
            }
        }

        private ReadOnlyCollection<IVideoSender> _phoneVideoSenders;
        public ReadOnlyCollection<IVideoSender> PhoneVideoSenders
        {
            get
            {
                if(_phoneVideoSenders == null)
                    _phoneVideoSenders = GetVideoSenders();

                return _phoneVideoSenders;
            }
        }

        private IVideoSender _currentChromecastVideoSender;
        public IVideoSender CurrentChromecastVideoSender 
        { 
            get
            {
                if (_currentChromecastVideoSender == null)
                    _currentChromecastVideoSender = CreateCurrentChromecastVideoSender();

                return _currentChromecastVideoSender;
            }
            set
            {
                if (_currentChromecastVideoSender == value)
                    return;

                _currentChromecastVideoSender = value;
                _settingsService.SetValue(nameof(CurrentChromecastVideoSender), value.VideoSenderId);
            }
        }

        private ReadOnlyCollection<IVideoSender> _chromecastVideoSenders;
        public ReadOnlyCollection<IVideoSender> ChromecastVideoSenders
        {
            get
            {
                if (_chromecastVideoSenders == null)
                    _chromecastVideoSenders = GetChromecastVideoSenders();

                return _chromecastVideoSenders;
            }
        }

		private string _sendersIconsFolder;
        private string SendersIconsFolder
		{
			get
			{
				if(_sendersIconsFolder == null)
				{
					var folderPath = Path.Combine(Application.Context.ExternalCacheDir.AbsolutePath, "SendersIcons");

					if (!Directory.Exists(folderPath))
						Directory.CreateDirectory(folderPath);

					_sendersIconsFolder = folderPath;
				}

				return _sendersIconsFolder;
			}
		}

        private string GetDefaultVideoSenderAppId()
        {
            var mxPlayerSender = PhoneVideoSenders.FirstOrDefault(x => x.VideoSenderId.StartsWith("com.mxtech.videoplayer", StringComparison.InvariantCultureIgnoreCase));

            if (mxPlayerSender != null)
                return mxPlayerSender.VideoSenderId;

            return PhoneVideoSenders.First().VideoSenderId;
        }

        private IVideoSender CreateCurrentPhoneVideoSender()
        {
            var currentVideoSenderAppId = _settingsService.GetValue(nameof(CurrentPhoneVideoSender), string.Empty);

            if (string.IsNullOrEmpty(currentVideoSenderAppId) || !PhoneVideoSenders.Any(x => x.VideoSenderId == currentVideoSenderAppId))
            {
                currentVideoSenderAppId = GetDefaultVideoSenderAppId();
                _settingsService.SetValue(nameof(CurrentPhoneVideoSender), currentVideoSenderAppId);
            }

            return PhoneVideoSenders.First(x => x.VideoSenderId == currentVideoSenderAppId);
        }

        private IVideoSender CreateCurrentChromecastVideoSender()
        {
            var currentChromecastVideoSenderAppId = _settingsService.GetValue(nameof(CurrentChromecastVideoSender), string.Empty);

            if(string.IsNullOrEmpty(currentChromecastVideoSenderAppId) || !ChromecastVideoSenders.Any(x => x.VideoSenderId == currentChromecastVideoSenderAppId))
            {
                currentChromecastVideoSenderAppId = ChromecastVideoSenders.First(x => x.VideoSenderId.StartsWith(
                    IziCastAppId,
                    StringComparison.InvariantCultureIgnoreCase
                )).VideoSenderId;

                _settingsService.SetValue(nameof(CurrentChromecastVideoSender), currentChromecastVideoSenderAppId);
            }

            return ChromecastVideoSenders.First(x => x.VideoSenderId == currentChromecastVideoSenderAppId);
        }

        private ReadOnlyCollection<IVideoSender> GetVideoSenders()
        {
			var packageNames = PackageManager.QueryIntentActivities(ViewVideoIntent, PackageInfoFlags.MetaData)
											 .Select(x => x.ActivityInfo.ApplicationInfo)
			                                 .Where(x => x.Enabled && x.PackageName != IziCastAppId)
											 .Select(x => x.PackageName)
											 .Except(_chromecastSupportedAppNames);

			return packageNames.Select(x => new ThirdPartyAppVideoSender(x, false))
							   .Cast<IVideoSender>()
							   .ToList()
							   .AsReadOnly();
        }

        private ReadOnlyCollection<IVideoSender> GetChromecastVideoSenders()
		{         
			var thirdPartyChromecastVideoSenders = PackageManager.QueryIntentActivities(ViewVideoIntent, PackageInfoFlags.MetaData)
			                                                     .Select(x => x.ActivityInfo.ApplicationInfo)
			                                                     .Where(x => x.Enabled && _chromecastSupportedAppNames.Contains(x.PackageName))
																 .Select(x => new ThirdPartyAppVideoSender(x.PackageName, true));
			
			var googleCastVideoSender = Mvx.Resolve<GoogleCastVideoSender>();

			return thirdPartyChromecastVideoSenders.Cast<IVideoSender>()
												   .Prepend(googleCastVideoSender)
												   .ToList()
												   .AsReadOnly();
		}

        public async Task<bool> EnsureAtLeastOneVideoSenderIsAvailable()
        {
            var videoSendersNotEmpty = PhoneVideoSenders.Any();

			var textBinder = new MvxLanguageBinder(string.Empty, nameof(VideoSenderService));

			await _userInteractionService.ShowToastAsync(textBinder.GetText("NoPlayersAvailable"), true);

            return videoSendersNotEmpty;
        }

        public void LoadSendersIcons()
		{
			var currentSendersIds = PhoneVideoSenders.Concat(ChromecastVideoSenders)
													 .Select(x => x.VideoSenderId)
													 .Select(x => x.StartsWith(IziCastAppId, StringComparison.InvariantCultureIgnoreCase) ? IziCastAppId : x)
			                                         .Distinct()
													 .OrderBy(x => x)
													 .ToList();

			var savedSendersIcons = _settingsService.GetValue("SendersIcons", new List<string>());

			var savedSendersIds = savedSendersIcons.Select(Path.GetFileNameWithoutExtension).ToList();

			if (currentSendersIds.SequenceEqual(savedSendersIds))
				return;

			var dirInfo = new DirectoryInfo(SendersIconsFolder);
			dirInfo.ClearDirectory();

			var bitmapsWithIds = currentSendersIds.Select(x => (Id: x, Bitmap: PackageManager.GetApplicationIcon(x).ToBitmap())).ToList();

			foreach(var bitmapWithId in bitmapsWithIds)
			{
				var path = Path.Combine(SendersIconsFolder, $"{bitmapWithId.Id}.png");

				using(var stream = new FileStream(path, FileMode.Create))
				{
					bitmapWithId.Bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
				}
			}

			_settingsService.SetValue("SendersIcons", bitmapsWithIds.Select(x => $"{x.Id}.png").ToList());
		}

		private Drawable CreateVideoSenderIconDrawable(IVideoSender videoSender)
		{
			var id = videoSender.VideoSenderId;

			if(id.StartsWith(IziCastAppId, StringComparison.InvariantCultureIgnoreCase))
			{
				return Application.Context.GetDrawable(Resource.Mipmap.ic_launcher);
			}

			return PackageManager.GetApplicationIcon(id);
		}
    }
}
