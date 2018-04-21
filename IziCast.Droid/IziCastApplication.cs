using System;
using Android.App;
using Android.Runtime;
using IziCast.Core;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace IziCast.Droid
{
    [Application]
    public class IziCastApplication : MvxAppCompatApplication<IziCastSetup, App>
    {
        public IziCastApplication()
        {
        }

        public IziCastApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
	}
}
