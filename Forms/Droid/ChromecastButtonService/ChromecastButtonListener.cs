﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using IziCast.Core;
using Xamarin.Forms.Platform.Android;

namespace IziCast.Droid
{
	public class ChromecastButtonListener : FloatingActionButton.OnVisibilityChangedListener, View.IOnTouchListener
	{
		const int LONG_PRESS_DELAY = 1000;
        const int CONNECTED_DELAY = 1000;

		readonly ChromecastButtonService _service;
		readonly Connectivity _connectivity;
		FloatingActionButton _button;

		public Action OnShownAction;
		public Action OnHiddenAction;

        public ChromecastButtonListener(ChromecastButtonService service, Connectivity connectivity)
		{
			_service = service;
            _connectivity = connectivity;
		}

		public override void OnShown(FloatingActionButton fab)
		{
			_button = fab;
			base.OnShown(fab);

			_connectivity.StatusChanged += OnConnectivityStatusChanged;

			OnShownAction?.Invoke();
		}

		public override void OnHidden(FloatingActionButton fab)
		{
			_button = null;
            base.OnHidden(fab);

			_connectivity.StatusChanged -= OnConnectivityStatusChanged;

			OnHiddenAction?.Invoke();
		}

		void OnConnectivityStatusChanged(object sender, ConnectivityStatus status)
		{
			if (_button == null) return;

            Application.SynchronizationContext.Post(async state =>
            {
				Func<AnimationDrawable> animation = () => (AnimationDrawable)_button.Drawable;

				switch (status)
				{
					case ConnectivityStatus.Connecting:
						_button.SetImageResource(Resource.Drawable.mr_button_connecting_dark);
						animation.Invoke().Start();
						Toast.MakeText(_service, "Connecting to chromecast", ToastLength.Short).Show();
						break;
					case ConnectivityStatus.Connected:
						_button.SetImageResource(Resource.Drawable.mr_button_connected_dark);
						animation.Invoke().Start();
						Toast.MakeText(_service, "Connected", ToastLength.Short).Show();
						
                        await Task.Delay(CONNECTED_DELAY);
						_service.StopSelf();
						break;
					case ConnectivityStatus.Disconnected:
                        _button.SetImageResource(Resource.Drawable.mr_button_connecting_dark);
						Toast.MakeText(_service, "Not connected", ToastLength.Short).Show();
						break;
				}
			}, null);
		}

		CancellationTokenSource _longPressTokenSource;

		public bool OnTouch(View v, MotionEvent e)
		{
			if (_button == null) return false;

            switch(e.Action)
            {
                case MotionEventActions.Down:
                    OnButtonTouchedDown();
                    break;
                case MotionEventActions.Up:
                    OnButtonTouchedUp();
                    break;
            }

			return true;
		}

		void OnButtonTouchedUp()
		{
            if(_connectivity.Status == ConnectivityStatus.Disconnected && _longPressTokenSource != null)
            {
				_longPressTokenSource.Cancel();
                _longPressTokenSource = null;

				_service.Messenger.Send(new Message
				{
					What = 42
				});
            }
		}

		async void OnButtonTouchedDown()
		{
			if (_connectivity.Status == ConnectivityStatus.Disconnected)
			{
				_longPressTokenSource = new CancellationTokenSource();
				await Task.Delay(LONG_PRESS_DELAY, _longPressTokenSource.Token).ContinueWith(t =>
				{
					if (t.IsCompleted && !t.IsCanceled)
						OnButtonLongPressed();
				});
			}
		}

        void OnButtonLongPressed()
        {
			_longPressTokenSource = null;
			var vibrator = (Vibrator)_service.GetSystemService(Context.VibratorService);
			vibrator.Vibrate(100);

			_service.StopSelf();
        }
	}
}
