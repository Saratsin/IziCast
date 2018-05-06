using System;
using IziCast.Core.Sevices;
namespace IziCast.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
    {      
		private readonly IVideoSenderService _videoSenderService;

		public MainViewModel(IVideoSenderService videoSenderService)
		{
			_videoSenderService = videoSenderService;         
		}

		public override string Title { get; set; } = "Izi Cast";
	}
}
