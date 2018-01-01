using System;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;

namespace IziCast.Droid.Base.Views
{
    public abstract class MvxBaseOverlayAndroidViewAdapter
    {
        private readonly IMvxEventSourceOverlayAndroidView _eventSource;

        protected IMvxView View => _eventSource as IMvxView;

        protected MvxBaseOverlayAndroidViewAdapter(IMvxEventSourceOverlayAndroidView eventSource)
        {
            _eventSource = eventSource;

            _eventSource.AttachWillBeCalled += EventSourceOnAttachWillBeCalled;
            _eventSource.AttachCalled += EventSourceOnAttachCalled;
            _eventSource.DetachCalled += EventSourceOnDetachCalled;
            _eventSource.DisposeCalled += EventSourceOnDisposeCalled;
        }

        protected virtual void EventSourceOnAttachWillBeCalled(object sender, EventArgs e)
        {
        }

        protected virtual void EventSourceOnAttachCalled(object sender, EventArgs e)
        {
        }

        protected virtual void EventSourceOnDetachCalled(object sender, EventArgs e)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }
    }
}
