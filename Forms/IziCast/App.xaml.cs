using System;
using IziCast.Pages;
using IziCast.ViewModels;
using Xamarin.Forms;

namespace IziCast
{
	public partial class App : Application
	{
		public static new App Current => Application.Current as App;

		readonly UIPresenter _presenter;
		public static UIPresenter Presenter => Current._presenter;

		readonly Action _showFloatingOverlayAction;
		public static void ShowFloatingOverlay() => Current._showFloatingOverlayAction?.Invoke();

		public App(Action showFloatingOverlayAction)
		{
			_showFloatingOverlayAction = showFloatingOverlayAction;
			_presenter = new UIPresenter();

			InitializeComponent();

			var page = new IziCastPage();
			page.BindingContext = new IziCastViewModel();

			MainPage = new NavigationPage(page);
			Presenter.Initialize((NavigationPage)MainPage);
		}

		public void SendUri(string uri)
		{
			var currentPage = Presenter.CurrentPage as IziCastPage;

			if (currentPage?.ViewModel != null)
				currentPage.ViewModel.VideoUri = uri;
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
