using System;

namespace IziCast.Droid.Base.Views
{
    public interface IMvxEventSourceOverlayAndroidView
    {
        event EventHandler ViewCreated;
        event EventHandler ViewWillAttachToWindow;
        event EventHandler ViewAttachedToWindow;
        event EventHandler ViewWillDetachFromWindow;
        event EventHandler ViewDetachedFromWindow;
        event EventHandler ViewDisposed;

        void RaiseViewCreated();
        void RaiseViewWillAttachToWindow();
        void RaiseViewAttachedToWindow();
        void RaiseViewWillDetachFromWindow();
        void RaiseViewDetachedFromWindow();
    }
}
