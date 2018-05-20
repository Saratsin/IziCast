using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using IziCast.Core.Enums;
using IziCast.Droid.Extensions;
using MvvmCross.Commands;
using System;
using System.Threading.Tasks;
using MvvmCross.WeakSubscription;
using IziCast.Core;

namespace IziCast.Droid.Controls
{
    [Register("izicast.droid.controls.FloatingChromecastButton")]
    public class FloatingChromecastButton : FloatingActionButton
    {
        private readonly FloatingChromecastButtonListener _onVisibilityChangedListener = new FloatingChromecastButtonListener();

        private TaskCompletionSource<bool> _showTcs;
        private TaskCompletionSource<bool> _hideTcs;
        private AnimationDrawable _connectingDrawable;
        private AnimationDrawable _connectedDrawable;

		public FloatingChromecastButton(Context context) : base(context)
		{
			Init();
		}
  
        public FloatingChromecastButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public event EventHandler ShowAsyncCommandChanged;
        public event EventHandler HideAsyncCommandChanged;

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

		public MvxAsyncCommand ShowAsyncCommand { get; set; }

        public MvxAsyncCommand HideAsyncCommand { get; set; }

        private void Init()
        {
            _connectingDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connecting_dark, Context.Theme);
            _connectedDrawable = (AnimationDrawable)Resources.GetDrawable(Resource.Drawable.mr_button_connected_dark, Context.Theme);

            BackgroundTintList = BackgroundColor.ToColorStateList();
            SetImageDrawable(_connectingDrawable);

            _onVisibilityChangedListener.WeakSubscribe(nameof(FloatingChromecastButtonListener.Shown), ShowCompleted);
            _onVisibilityChangedListener.WeakSubscribe(nameof(FloatingChromecastButtonListener.Hidden), HideCompleted);

            ShowAsyncCommand = new MvxAsyncCommand(ShowAsync);
            HideAsyncCommand = new MvxAsyncCommand(HideAsync);
        }

        private async Task ShowAsync()
        {
            if (IsShown)
                return;

            _showTcs = new TaskCompletionSource<bool>();

            Show(_onVisibilityChangedListener);

            await _showTcs.Task;
        }

        private Task HideAsync()
        {
            if (!IsShown)
                return Task.FromResult(true);

            _hideTcs = new TaskCompletionSource<bool>();

            Hide(_onVisibilityChangedListener);

            return _hideTcs.Task;
        }

        private void ShowCompleted(object sender, EventArgs e)
        {
            _showTcs.TrySetResult(true);
        }

        private void HideCompleted(object sender, EventArgs e)
        {
            _hideTcs.TrySetResult(true);
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

        public void InvalidateVisibilityCommandsBindings()
        {
            ShowAsyncCommandChanged?.Invoke(this, EventArgs.Empty);
            HideAsyncCommandChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
