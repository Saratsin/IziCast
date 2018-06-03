using System;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Android.Graphics.Drawables;
using System.IO;

namespace IziCast.Droid.Extensions
{
    public static class Extensions
    {
        public static int DpToPx(this Context context, float dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }

        public static ColorStateList ToColorStateList(this Color color)
        {
            var states = new int[][] {
                new int[] {Android.Resource.Attribute.StateEnabled},  // enabled
                new int[] {-Android.Resource.Attribute.StateEnabled}, // disabled
                new int[] {-Android.Resource.Attribute.StateChecked}, // unchecked
                new int[] {Android.Resource.Attribute.StatePressed}   // pressed
            };

            var colors = Enumerable.Repeat((int)color, states.Length).ToArray();

            return new ColorStateList(states, colors);
        }

        public static TimeSpan ToTimeSpan(this ToastLength duration)
        {
            switch (duration)
            {
                case ToastLength.Short:
                    return TimeSpan.FromMilliseconds(2000);
                default:
                    return TimeSpan.FromMilliseconds(3500);
            }
        }

		public static void ClearDirectory(this DirectoryInfo dirInfo)
		{
			foreach (var dir in dirInfo.EnumerateDirectories())
				dir.Delete(true);

			foreach (var file in dirInfo.EnumerateFiles())
				file.Delete();
		}

		public static Bitmap ToBitmap(this Drawable drawable)
		{
			if(drawable is BitmapDrawable bitmapDrawable && bitmapDrawable.Bitmap != null)
				return bitmapDrawable.Bitmap;

			var bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);

			var canvas = new Canvas(bitmap);
			drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
			drawable.Draw(canvas);

			return bitmap;
		}
    }
}
