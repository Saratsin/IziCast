using System;
using Android.App;
using IziCast.Core;

namespace IziCast.Droid
{
	[Application]
	public class MainApplication : Application
	{
		static MainApplication _current;
		public static MainApplication Current => _current;

		CoreApp _core;
		public CoreApp Core => _core;

		public MainApplication(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Init();
		}

		public MainApplication()
		{
			Init();
		}

		void Init()
		{
			_core = new CoreApp(IPAddressManager.IP_Address);
		}

		public override void OnCreate()
		{
			base.OnCreate();

			_current = this;
		}

		public override void OnLowMemory()
		{
			Console.WriteLine("App has low memory warning");
			base.OnLowMemory();
		}

		public override void OnTerminate()
		{
			Console.WriteLine("App terminating");
			base.OnTerminate();
		}
	}
}
