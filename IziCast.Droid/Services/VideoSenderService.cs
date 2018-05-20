﻿using System;
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
					_viewVideoIntent.SetType("video/*");
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
                _settingsService.SetValue(nameof(CurrentPhoneVideoSender), value.VideoSenderAppId);
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
                _settingsService.SetValue(nameof(CurrentChromecastVideoSender), value.VideoSenderAppId);
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

        private string GetDefaultVideoSenderAppId()
        {
            var mxPlayerSender = PhoneVideoSenders.FirstOrDefault(x => x.VideoSenderAppId.StartsWith("com.mxtech.videoplayer", StringComparison.InvariantCultureIgnoreCase));

            if (mxPlayerSender != null)
                return mxPlayerSender.VideoSenderAppId;

            return PhoneVideoSenders.First().VideoSenderAppId;
        }

        private IVideoSender CreateCurrentPhoneVideoSender()
        {
            var currentVideoSenderAppId = _settingsService.GetValue(nameof(CurrentPhoneVideoSender), string.Empty);

            if (string.IsNullOrEmpty(currentVideoSenderAppId) || !PhoneVideoSenders.Any(x => x.VideoSenderAppId == currentVideoSenderAppId))
            {
                currentVideoSenderAppId = GetDefaultVideoSenderAppId();
                _settingsService.SetValue(nameof(CurrentPhoneVideoSender), currentVideoSenderAppId);
            }

            return PhoneVideoSenders.First(x => x.VideoSenderAppId == currentVideoSenderAppId);
        }

        private IVideoSender CreateCurrentChromecastVideoSender()
        {
            var currentChromecastVideoSenderAppId = _settingsService.GetValue(nameof(CurrentChromecastVideoSender), string.Empty);

            if(string.IsNullOrEmpty(currentChromecastVideoSenderAppId) || !ChromecastVideoSenders.Any(x => x.VideoSenderAppId == currentChromecastVideoSenderAppId))
            {
                currentChromecastVideoSenderAppId = ChromecastVideoSenders.First(x => x.VideoSenderAppId.StartsWith(
                    IziCastAppId,
                    StringComparison.InvariantCultureIgnoreCase
                )).VideoSenderAppId;

                _settingsService.SetValue(nameof(CurrentChromecastVideoSender), currentChromecastVideoSenderAppId);
            }

            return ChromecastVideoSenders.First(x => x.VideoSenderAppId == currentChromecastVideoSenderAppId);
        }

        private ReadOnlyCollection<IVideoSender> GetVideoSenders()
        {
            var packageNames = PackageManager.QueryIntentActivities(ViewVideoIntent, 0)
                                             .Select(x => x.ActivityInfo.ApplicationInfo)
                                             .Where(x => x.Enabled)
                                             .Select(x => x.PackageName)
                                             .Except(_chromecastSupportedAppNames);

			return packageNames.Select(x => new ThirdPartyAppVideoSender(x, false))
							   .Cast<IVideoSender>()
							   .ToList()
							   .AsReadOnly();
        }

        private ReadOnlyCollection<IVideoSender> GetChromecastVideoSenders()
		{
			var thirdPartyChromecastVideoSenders = PackageManager.QueryIntentActivities(ViewVideoIntent, 0)
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
    }
}
