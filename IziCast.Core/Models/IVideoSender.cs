﻿using System;
using System.Threading.Tasks;
using IziCast.Core.Models;

namespace IziCast.Core.Models
{
    public interface IVideoSender
    {   
		string VideoSenderAppId { get; }

		bool IsChromecastSender { get; }

		Task<Try> SendVideoAsync(string videoUrl);
    }
}
