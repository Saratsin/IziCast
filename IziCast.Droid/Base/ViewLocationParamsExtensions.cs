using Android.Graphics;
using Android.OS;
using Android.Views;

namespace IziCast.Droid.Base
{
    public static class ViewLocationParamsExtensions
    {
        public static WindowManagerLayoutParams ToWindowManagerLayoutParams(this ViewLocationParams locationParams)
        {
            var type = WindowManagerTypes.Phone;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                type = WindowManagerTypes.ApplicationOverlay;


            return new WindowManagerLayoutParams(w: locationParams.Width,
                                                 h: locationParams.Height,
                                                 xpos: locationParams.X,
                                                 ypos: locationParams.Y,
                                                 _type: type,
                                                 _flags: WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
                                                 _format: Format.Translucent)
            {
                Gravity = locationParams.Gravity
            };
        }
    }
}
