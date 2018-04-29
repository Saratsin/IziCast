using Android.Views;

namespace IziCast.Droid.Base
{
    public class ViewLocationParams
    {
        public ViewLocationParams(GravityFlags gravity = GravityFlags.Start | GravityFlags.Top,
                                  int x = 0,
                                  int y = 0,
                                  int width = ViewGroup.LayoutParams.WrapContent,
                                  int height = ViewGroup.LayoutParams.WrapContent)
        {
            Gravity = gravity;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public GravityFlags Gravity { get; }

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }
    }
}
