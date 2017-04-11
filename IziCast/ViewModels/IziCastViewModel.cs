using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace IziCast.ViewModels
{
	public class IziCastViewModel : BaseViewModel
	{
		string _videoUri;
		public string VideoUri
		{
			get { return _videoUri; }
			set { SetProperty(ref _videoUri, value); }
		}

		ICommand _castCommand;
		public ICommand CastCommand
		{
			get
			{
				return _castCommand ?? (_castCommand = new Command(async () =>
				{
					if (Uri.IsWellFormedUriString(VideoUri, UriKind.Absolute))
					{
						await UIPresenter.Singleton.PushAsync(new NewViewModel());
					}
				}));
		
			}
		}
	}
}
