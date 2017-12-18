using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SharpCaster.Controllers;
using SharpCaster.Services;
using Xamarin.Forms;

namespace IziCast.ViewModels
{
	public class IziCastViewModel : BaseViewModel
	{
		string _videoUri = "http://www.html5videoplayer.net/videos/toystory.mp4";
		public string VideoUri
		{
			get { return _videoUri; }
			set
			{
				if (SetProperty(ref _videoUri, value))
					RaisePropertyChanged(nameof(CastButtonIsEnabled));
			}
		}

		public bool CastButtonIsEnabled => Core.CoreApp.Current.ValidateUri(VideoUri);

		ICommand _castCommand;
		public ICommand CastCommand
		{
			get
			{
				return _castCommand ?? (_castCommand = new Command(async () =>
				{
					await Core.CoreApp.Current.ConnectToChromecast(new Core.Connectivity(), VideoUri);
				}));

			}
		}
	}
}
