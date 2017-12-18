using IziCast.ViewModels;
using Xamarin.Forms;

namespace IziCast.Pages
{
	public partial class IziCastPage : ContentPage
	{
		public IziCastViewModel ViewModel => BindingContext as IziCastViewModel;

		public IziCastPage()
		{
			InitializeComponent();
		}
	}
}