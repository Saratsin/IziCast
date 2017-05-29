using System;
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

namespace IziCast.Droid
{
	class ChromecastButtonListener : FloatingActionButton.OnVisibilityChangedListener, View.IOnTouchListener
	{
		const long LONG_PRESS_DELAY = 1000L;

		readonly ChromecastButtonService _service;
		readonly Connectivity _connectivity;
		FloatingActionButton _button;

		public ChromecastButtonListener(ChromecastButtonService service)
		{
			_service = service;
			_connectivity = new Connectivity();
		}

		public override void OnShown(FloatingActionButton fab)
		{
			_button = fab;
			base.OnShown(fab);
			_connectivity.StatusChanged += OnConnectivityStatusChanged;
		}

		public override void OnHidden(FloatingActionButton fab)
		{
			_button = null;
			base.OnHidden(fab);
			_service.WindowManager.RemoveView(_service.Button);
			_service.Button = null;
			_connectivity.StatusChanged -= OnConnectivityStatusChanged;
		}

		CancellationTokenSource _longPressTokenSource;

		public bool OnTouch(View v, MotionEvent e)
		{
			if (_button == null) return false;

			if (e.Action == MotionEventActions.Down)
			{
				OnButtonTouchedDown();
			}
			else if (e.Action == MotionEventActions.Up)
			{
				if (_connectivity.Status == ConnectivityStatus.Disconnected || _connectivity.Status == ConnectivityStatus.Failed)
				{
					var downUpDelay = e.EventTime - e.DownTime;

					if (downUpDelay <= LONG_PRESS_DELAY)
						OnButtonTouchedUp((FloatingActionButton)v);
				}
			}

			return false;
		}

		void OnConnectivityStatusChanged(object sender, ConnectivityStatus status)
		{
			Application.SynchronizationContext.Post(async state =>
			{
				if (_button == null) return;

				Func<AnimationDrawable> animation = () => (AnimationDrawable)_button.Drawable;

				switch (status)
				{
					case ConnectivityStatus.Connecting:
						animation.Invoke().Stop();
						_button.SetImageResource(Resource.Drawable.mr_button_connecting_dark);
						animation.Invoke().Start();
						Toast.MakeText(_service, "Connecting to chromecast", ToastLength.Short).Show();
						break;
					case ConnectivityStatus.Connected:
						_button.SetImageResource(Resource.Drawable.mr_button_connected_dark);
						animation.Invoke().Start();
						Toast.MakeText(_service, "Connected", ToastLength.Short).Show();
						await Task.Delay((int)LONG_PRESS_DELAY);
						_service.StopSelf();
						break;
					case ConnectivityStatus.Failed:
						animation.Invoke().Stop();
						animation.Invoke().SelectDrawable(0);
						Toast.MakeText(_service, "Failed to connect", ToastLength.Short).Show();
						break;
				}
			}, null);
		}

		void OnButtonTouchedUp(FloatingActionButton button)
		{
			_longPressTokenSource?.Cancel();

			_service.Messenger.Send(new Message
			{
				What = 42,
				Obj = JavaWrapper.Wrap(_connectivity)
			});
		}

		async void OnButtonTouchedDown()
		{
			_longPressTokenSource?.Cancel();
			_longPressTokenSource = new CancellationTokenSource();
			await Task.Delay((int)LONG_PRESS_DELAY, _longPressTokenSource.Token).ContinueWith(t =>
			{
				if (t.IsCompleted && !t.IsCanceled)
				{
					var vibrator = (Vibrator)_service.GetSystemService(Context.VibratorService);
					vibrator.Vibrate(100);
					OnButtonLongPressed();
				}
			});
		}

		void OnButtonLongPressed()
		{
			_service.StopSelf();
		}
	}
}
