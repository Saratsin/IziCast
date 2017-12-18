using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace IziCast.Droid
{
    public class ChromecastButton : RelativeLayout
	{
        ChromecastButtonListener _listener;

		Color _backgroundColor = Color.ParseColor("#03A9F4");
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set
			{
				if (_backgroundColor != value && Button != null)
				{
					_backgroundColor = value;
					Button.BackgroundTintList = new ColorStateList(new int[][] { new int[0] }, new int[] { value });
				}
			}
		}

		public FloatingActionButton Button;

		public ChromecastButton(Context context) : base (context)
		{
			Init(context);
		}

        public ChromecastButton(Context context, ChromecastButtonListener listener) : this(context)
        {
            SetButtonListener(listener);
        }

		void Init(Context context)
		{
			var themedContext = new ContextThemeWrapper(context, Resource.Style.Theme_AppCompat_NoActionBar);

			Button = new FloatingActionButton(themedContext)
			{
				BackgroundTintList = new ColorStateList(new int[][] { new int[0] }, new int[] { BackgroundColor }),
				Visibility = ViewStates.Invisible
			};
			Button.SetImageResource(Resource.Drawable.mr_button_connecting_dark);

			LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			var padding = (int)(16 * Resources.DisplayMetrics.Density);
			SetPadding(padding, padding, padding, padding);
			SetClipChildren(false);
			SetClipToPadding(false);
			AddView(Button);
		}

        public void SetButtonListener(ChromecastButtonListener listener)
        {
            Button.SetOnTouchListener(listener);
            _listener = listener;
        }

        public void Show(Action onShown = null)
        {
			if (_listener != null)
			{
				_listener.OnShownAction = onShown;
				Button.Show(_listener);
			}
            else
                Button.Show();
        }

        public void Hide(Action onHidden = null)
        {
			if (_listener != null)
			{
				_listener.OnHiddenAction = onHidden;
				Button.Hide(_listener);
			}
            else
                Button.Hide();
        }
	}
}
