using System;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace IziCast.Droid.Extensions
{
    public static class Extensions
    {
        public static int DpToPx(this Context context, float dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }

        public static TimeSpan ToTimeSpan(this ToastLength duration)
        {
            switch(duration)
            {
                case ToastLength.Short:
                    return TimeSpan.FromMilliseconds(2000);
                default:
                    return TimeSpan.FromMilliseconds(3500);
            }
        }
    }
}
