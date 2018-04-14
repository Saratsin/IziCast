using System;
using Android.App;
using Android.Runtime;
using IziCast.Core;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace IziCast.Droid
{
    [Application]
    public class Application : MvxAppCompatApplication<Setup, App>
    {
        public Application()
        {
        }

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
