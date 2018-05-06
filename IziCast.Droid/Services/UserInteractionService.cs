using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using IziCast.Droid.Extensions;
using MvvmCross.Plugin.Overlay.Platforms.Android;
using IziCast.Core.Sevices;
using IziCast.Core.Sevices.Interfaces;

namespace IziCast.Droid.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        public Task ShowToastAsync(string text, bool longDuration = false)
        {
            var toastDuration = longDuration ? ToastLength.Long : ToastLength.Short;

            Toast.MakeText(Application.Context, text, toastDuration).Show();

            return Task.Delay(toastDuration.ToTimeSpan());
        }
    }
}
