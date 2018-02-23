using System;

namespace IziCast.Droid.Base.Views
{
    public abstract class MvxEventSourceOverlayAndroidView : IMvxEventSourceOverlayAndroidView, IDisposable
    {
        protected MvxEventSourceOverlayAndroidView()
        {
        }


        public event EventHandler ViewCreated;

		public event EventHandler ViewWillAttachToWindow;

		public event EventHandler ViewAttachedToWindow;

        public event EventHandler ViewWillDetachFromWindow;

        public event EventHandler ViewDetachedFromWindow;

        public event EventHandler ViewDisposed;


        void IMvxEventSourceOverlayAndroidView.RaiseViewCreated()
        {
			OnViewCreated();
            var viewCreated = ViewCreated;
            viewCreated?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlayAndroidView.RaiseViewWillAttachToWindow()
        {
            OnViewWillAttachToWindow();
            var viewWillAttachToWindow = ViewWillAttachToWindow;
            viewWillAttachToWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlayAndroidView.RaiseViewAttachedToWindow()
        {
            OnViewAttachedToWindow();
            var viewAttachedToWindow = ViewAttachedToWindow;
            viewAttachedToWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlayAndroidView.RaiseViewWillDetachFromWindow()
        {
            OnViewWillDetachFromWindow();
            var viewWillDetachFromWindow = ViewWillDetachFromWindow;
            viewWillDetachFromWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlayAndroidView.RaiseViewDetachedFromWindow()
        {
            OnViewDetachedFromWindow();
            var viewDetachedFromWindow = ViewDetachedFromWindow;
            viewDetachedFromWindow?.Invoke(this, EventArgs.Empty);
        }


        protected virtual void OnViewCreated()
        {
        }

        protected virtual void OnViewWillAttachToWindow()
        {
        }

        protected virtual void OnViewAttachedToWindow()
        {
        }

        protected virtual void OnViewWillDetachFromWindow()
        {
        }

        protected virtual void OnViewDetachedFromWindow()
        {
        }


        #region IDisposable implementation
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                var viewDisposed = ViewDisposed;

                viewDisposed?.Invoke(this, EventArgs.Empty);
            }

            _disposed = true;
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
