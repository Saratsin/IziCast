using System;
using MvvmCross.Binding.BindingContext;

namespace IziCast.Droid.Base.Views
{
    public class MvxBindingOverlayAndroidViewAdapter : MvxBaseOverlayAndroidViewAdapter
    {
        private IMvxBindingContext BindingContext
        {
            get
            {
                var contextOwner = (IMvxBindingContextOwner)View;
                return contextOwner.BindingContext;
            }
        }

        public MvxBindingOverlayAndroidViewAdapter(IMvxEventSourceOverlayAndroidView eventSource) : base(eventSource)
        {
        }

        protected override void EventSourceOnAttachWillBeCalled(object sender, EventArgs e)
        {
            BindingContext.ClearAllBindings();
        }

        protected override void EventSourceOnDetachCalled(object sender, EventArgs e)
        {
            BindingContext.ClearAllBindings();
        }

        protected override void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
            BindingContext.ClearAllBindings();
        }
    }
}
