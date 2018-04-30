using Android.App;
using Android.Widget;

namespace IziCast.Droid.Services
{
    public class UserInteractionService
    {
        public UserInteractionService()
        {
            Instance = this;
        }

        public static UserInteractionService Instance { get; private set; } = new UserInteractionService();

        public void ShowToast(string text, bool longDuration = false)
        {
            Toast.MakeText(Application.Context, text, longDuration ? ToastLength.Long : ToastLength.Short).Show();
        }
    }
}
