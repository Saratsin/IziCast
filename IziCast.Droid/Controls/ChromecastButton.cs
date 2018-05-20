using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using IziCast.Core.Enums;
using IziCast.Droid.Extensions;

namespace IziCast.Droid.Controls
{
	[Register("izicast.droid.controls.ChromecastButton")]
	public class ChromecastButton : AppCompatImageButton
    {    
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
                BackgroundTintList = value.ToColorStateList();
            }
        }

        private void Init()
		{
			_connectingDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connecting_dark, Context.Theme);
            _connectedDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connected_dark, Context.Theme);

			BackgroundTintList = BackgroundColor.ToColorStateList();
            SetImageDrawable(_connectingDrawable);
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
                    break;
                case ConnectivityStatus.Connected:
                    _connectedDrawable.Stop();
                    _connectedDrawable.SelectDrawable(0);
                    SetImageDrawable(_connectedDrawable);
                    _connectedDrawable.Start();
                    break;
                case ConnectivityStatus.Disconnected:
                    _connectingDrawable.Stop();
                    _connectingDrawable.SelectDrawable(0);
                    SetImageDrawable(_connectingDrawable);
                    break;
            }
        }
    }
}
