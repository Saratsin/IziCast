using System;
using Android.Content;
using Android.Runtime;
using MvvmCross.Droid.Platform;

namespace IziCast.Droid.Base.Services
{
    [Register("izicast.droid.services.MvxStickyIntentService")]
    public abstract class MvxStickyIntentService : StickyIntentService
    {
        protected MvxStickyIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected MvxStickyIntentService()
        {
        }

        protected MvxStickyIntentService(string name) : base(name)
        {
        }

        protected abstract Context Context { get; }

        protected override void OnHandleIntent(Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Context);
            setup.EnsureInitialized();
        }
    }

}
