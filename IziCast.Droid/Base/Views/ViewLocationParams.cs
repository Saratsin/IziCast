using Android.Views;

namespace IziCast.Droid.Base.Views
{
    public class ViewLocationParams
    {
        public GravityFlags Gravity { get; set; } = GravityFlags.Start | GravityFlags.Top;

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public int Width { get; set; } = ViewGroup.LayoutParams.WrapContent;

        public int Height { get; set; } = ViewGroup.LayoutParams.WrapContent;
    }
}
