using Android.App;
using Android.Content;
using Android.Views;
using IziCast.Core.Enums;

namespace IziCast.Droid
{
    public class IziCastContext : ContextThemeWrapper
    {
        public IziCastContext(Context @base, int themeResId, LaunchMode launchMode) : base(@base, themeResId)
        {
            LaunchMode = launchMode;
        }

		public LaunchMode LaunchMode { get; }

        public static IziCastContext FromApplicationContext(int themeResId, LaunchMode launchMode)
        {
            return new IziCastContext(Application.Context, themeResId, launchMode);
        }
    }
}
