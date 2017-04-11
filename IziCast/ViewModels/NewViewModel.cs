using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace IziCast.ViewModels
{
	public class NewViewModel : BaseViewModel
	{
		ICommand _goBackCommand;
		public ICommand GoBackCommand
		{
			get
			{
				return _goBackCommand ?? (_goBackCommand = new Command(async () =>
				{
					await UIPresenter.Singleton.PopAsync();
				}));
			}
		}
	}
}
