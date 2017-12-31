using System;
using Android.Content;
using Android.Util;

namespace IziCast.Droid.Extensions
{
    public static class ContextExtensions
    {
        public static int DpToPx(this Context context, float dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}
