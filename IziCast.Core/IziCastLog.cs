using MvvmCross.Logging;
using MvvmCross;
using System;

namespace IziCast.Core
{   
    public class IziCastLog
    {
		private static Lazy<IMvxLog> _instanceLazy = new Lazy<IMvxLog>(() => Mvx.Resolve<IMvxLog>());

		public static IMvxLog Instance => _instanceLazy.Value;
    }
}
