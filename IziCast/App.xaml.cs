using IziCast.Pages;
using IziCast.ViewModels;
using Xamarin.Forms;

namespace IziCast
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var page = new IziCastPage();
			page.BindingContext = new IziCastViewModel();

			MainPage = new NavigationPage(page);
			UIPresenter.Singleton.Initialize((NavigationPage)MainPage);
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
