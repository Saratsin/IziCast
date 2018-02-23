using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using IziCast.Droid.Extensions;

namespace IziCast.Droid.Services
{
    public class UserInteractionService
    {
        public UserInteractionService()
        {
            Instance = this;
        }

		public static UserInteractionService Instance { get; private set; } = new UserInteractionService();

        internal Task ShowToastAsync(string text, ToastLength duration)
        {
            Toast.MakeText(Application.Context, text, duration).Show();
            return Task.Delay(duration.ToTimeSpan());
        }
    }
}
