using System;
using MvvmCross.Logging;
using MvvmCross;
namespace IziCast.Core
{
    public class IziCastLog
    {
        public static IMvxLog Instance => Mvx.Resolve<IMvxLog>();
    }
}
