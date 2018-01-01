using System;
using Android.Content;
using Android.OS;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Droid.Views;
using MvvmCross.Platform.Platform;

namespace IziCast.Droid.Base.Views
{
    public class MvxOverlayAndroidViewAdapter : MvxBaseOverlayAndroidViewAdapter
    {
        public MvxOverlayAndroidViewAdapter(IMvxEventSourceOverlayAndroidView eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnAttachCalled(object sender, EventArgs e)
        {
            View.OnViewAttached();
        }

        protected override void EventSourceOnDetachCalled(object sender, EventArgs e)
        {
            View.OnViewDetached();
        }
    }
}