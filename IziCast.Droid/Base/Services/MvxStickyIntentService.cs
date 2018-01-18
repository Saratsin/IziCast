using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Platform;

namespace IziCast.Droid.Base.Services
{
    [Register("izicast.droid.services.MvxStickyIntentService")]
    public abstract class MvxStickyIntentService : Service
    {
        protected MvxStickyIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected MvxStickyIntentService()
        {
        }

        //protected MvxStickyIntentService(string name) : base(name)
        //{
        //}

        protected abstract Context Context { get; }


        public override void OnCreate()
        {
            base.OnCreate();
        //protected override void OnHandleIntent(Intent intent)
        //{
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Context);
            setup.EnsureInitialized();
        }

        public override IBinder OnBind(Intent intent) => null;
    }

}
