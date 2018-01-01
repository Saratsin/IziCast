using System;
using MvvmCross.Platform.Core;

namespace IziCast.Droid.Base.Views
{
    public interface IMvxEventSourceOverlayAndroidView : IMvxDisposeSource
    {
        event EventHandler AttachWillBeCalled;

        event EventHandler AttachCalled;

        event EventHandler DetachCalled;
    }
}
