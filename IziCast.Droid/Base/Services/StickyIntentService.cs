using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace IziCast.Droid.Base.Services
{
    [Register("izicast.droid.services.StickyIntentService")]
    public abstract class StickyIntentService : Service
    {
        private volatile Looper _serviceLooper;
        private volatile ServiceHandler _serviceHandler;
        private string _name;

        private sealed class ServiceHandler : Handler
        {
            private readonly StickyIntentService _sis;

            public ServiceHandler(Looper looper, StickyIntentService sis) : base(looper) => _sis = sis;

            public override void HandleMessage(Message msg) => _sis.OnHandleIntent((Intent)msg.Obj);
        }

        /// <summary>
        /// Creates an <see cref="T:IziCast.Droid.Services.StickyIntentService"/>. Invoked by your subclass's constructor
        /// </summary>
        protected StickyIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        { }

        /// <summary>
        /// Creates an <see cref="T:IziCast.Droid.Services.StickyIntentService"/>. Invoked by your subclass's constructor
        /// </summary>
        protected StickyIntentService()
        {
            _name = GetType().Name;
        }

        /// <summary>
        /// Creates an <see cref="T:IziCast.Droid.Services.StickyIntentService"/>. Invoked by your subclass's constructor
        /// </summary>
        /// <param name="name">Used to name the worker thread, important only for debugging</param>
        protected StickyIntentService(String name) => _name = name;

        public override void OnCreate()
        {
            base.OnCreate();
            var thread = new HandlerThread("IntentService[" + _name + "]");
            thread.Start();

            _serviceLooper = thread.Looper;
            _serviceHandler = new ServiceHandler(_serviceLooper, this);
        }

        public sealed override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var msg = _serviceHandler.ObtainMessage();
            msg.Arg1 = startId;
            msg.Obj = intent;

            _serviceHandler.SendMessage(msg);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy() => _serviceLooper.Quit();

        /// <summary>
        /// Unless you provide binding for your service, you don't need to implement this
        /// method, because the default implementation returns null. 
        /// </summary>
        public override IBinder OnBind(Intent intent) => null;

        /// <summary>
        /// This method is invoked on the worker thread with a request to process.
        /// Only one Intent is processed at a time, but the processing happens on a
        /// worker thread that runs independently from other application logic.
        /// So, if this code takes a long time, it will hold up other requests to
        /// the same IntentService, but it will not hold up anything else.
        /// </summary>
        protected abstract void OnHandleIntent(Intent intent);
    }
}
