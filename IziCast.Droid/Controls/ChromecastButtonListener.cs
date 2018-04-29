
using System;
using System.Threading;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using IziCast.Core.Enums;
using IziCast.Droid.Widgets;

namespace IziCast.Droid.Controls
{
    public class ChromecastButtonListener : FloatingActionButton.OnVisibilityChangedListener, View.IOnTouchListener
    {
        const int LONG_PRESS_DELAY = 1000;
        const int CONNECTED_DELAY = 1000;

        private readonly ChromecastButton _button;

        public ChromecastButtonListener(ChromecastButton button)
        {
            _button = button;
        }

        public override void OnShown(FloatingActionButton fab)
        {
            base.OnShown(fab);

            //OnShownAction?.Invoke();
        }

        public override void OnHidden(FloatingActionButton fab)
        {
            base.OnHidden(fab);

            //_connectivity.StatusChanged -= OnConnectivityStatusChanged;

            //OnHiddenAction?.Invoke();
        }


        CancellationTokenSource _longPressTokenSource;

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    OnButtonTouchedDown();
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    OnButtonTouchedUp();
                    break;
            }

            return true;
        }

        void OnButtonTouchedUp()
        {
            //if (_connectivity.Status == ConnectivityStatus.Disconnected && _longPressTokenSource != null)
            //{
            //    _longPressTokenSource.Cancel();
            //    _longPressTokenSource = null;

            //    _service.Messenger.Send(new Message
            //    {
            //        What = 42
            //    });
            //}
        }

        void OnButtonTouchedDown()
        {
            //if (_connectivity.Status == ConnectivityStatus.Disconnected)
            //{
            //    _longPressTokenSource = new CancellationTokenSource();
            //    await Task.Delay(LONG_PRESS_DELAY, _longPressTokenSource.Token).ContinueWith(t =>
            //    {
            //        if (t.IsCompleted && !t.IsCanceled)
            //            OnButtonLongPressed();
            //    });
            //}
        }

        void OnButtonLongPressed()
        {
            _longPressTokenSource = null;
            //var vibrator = (Vibrator)_service.GetSystemService(Context.VibratorService);
            //vibrator.Vibrate(100);

            //_service.StopSelf();
        }
    }
}
