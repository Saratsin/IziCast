using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using IziCast.Droid.Extensions;
using MvvmCross.Plugin.Overlay.Platforms.Android;
using IziCast.Core.Services;
using IziCast.Core.Services.Interfaces;
using MvvmCross.Base;
using MvvmCross;

namespace IziCast.Droid.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        public Task ShowToastAsync(string text, bool longDuration = false)
        {
            var toastDuration = longDuration ? ToastLength.Long : ToastLength.Short;

            return Mvx.Resolve<IMvxMainThreadAsyncDispatcher>()
                      .ExecuteOnMainThreadAsync(() => Toast.MakeText(Application.Context, text, toastDuration))
                      .ContinueWith(t => Task.Delay(toastDuration.ToTimeSpan()));
        }
    }
}
