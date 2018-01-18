using System;
using Android.OS;
using Android.Widget;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Core;

namespace IziCast.Droid.Base.Views
{
    public class MvxEventSourceOverlayAndroidView : FrameLayout, IMvxEventSourceOverlayAndroidView
    {
        public MvxEventSourceOverlayAndroidView(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnAttachedToWindow()
        {
            AttachWillBeCalled?.Invoke(this, EventArgs.Empty);
            base.OnAttachedToWindow();
            AttachCalled?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            DetachCalled?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DisposeCalled?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AttachWillBeCalled;

        public event EventHandler AttachCalled;

        public event EventHandler DetachCalled;

		public event EventHandler DisposeCalled;
    }
}
