using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using IziCast.Core.Enums;
using IziCast.Droid.Listeners;

namespace IziCast.Droid.Widgets
{
    [Register("izicast.droid.widgets.ChromecastButton")]
    public class ChromecastButton : FloatingActionButton
    {
        private ChromecastButtonListener _listener;
        private AnimationDrawable _connectingDrawable;
        private AnimationDrawable _connectedDrawable;

		public ChromecastButton(Context context) : base(context)
		{
			Init();
		}
  
        public ChromecastButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value)
                    return;
                        
                if (value)
                    Show(_listener);
                else
                    Hide(_listener);
                
				_isVisible = value;
            }
        }

        private ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                
                _status = value;
                OnStatusChanged(value);
            }
        }

        private Color _backgroundColor = Color.ParseColor("#03A9F4");
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (_backgroundColor == value)
                    return;
                
                _backgroundColor = value;
                BackgroundTintList = new ColorStateList(new int[][] { new int[0] }, new int[] { value });
            }
        }

        private void Init()
        {
            _connectingDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connecting_dark, Context.Theme);
            _connectedDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connected_dark, Context.Theme);
            _listener = new ChromecastButtonListener(this);

            BackgroundTintList = new ColorStateList(new int[][] { new int[0] }, new int[] { BackgroundColor });
            Visibility = ViewStates.Visible;
            SetImageDrawable(_connectingDrawable);
            SetOnTouchListener(_listener);
        }


        private void OnStatusChanged(ConnectivityStatus status)
        {
            switch (status)
            {
                case ConnectivityStatus.Connecting:
                    _connectingDrawable.Stop();
                    _connectingDrawable.SelectDrawable(0);
                    SetImageDrawable(_connectingDrawable);
                    _connectingDrawable.Start();
                    Toast.MakeText(Context, "Connecting to chromecast", ToastLength.Short).Show();
                    break;
                case ConnectivityStatus.Connected:
                    _connectedDrawable.Stop();
                    _connectedDrawable.SelectDrawable(0);
                    SetImageDrawable(_connectedDrawable);
                    _connectedDrawable.Start();
                    Toast.MakeText(Context, "Connected", ToastLength.Short).Show();
                    break;
                case ConnectivityStatus.Disconnected:
                    _connectingDrawable.Stop();
                    _connectingDrawable.SelectDrawable(0);
                    SetImageDrawable(_connectingDrawable);
                    Toast.MakeText(Context, "Not connected", ToastLength.Short).Show();
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                _listener.Dispose();

            base.Dispose(disposing);
        }
    }
}
