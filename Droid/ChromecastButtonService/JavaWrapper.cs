using System;
using System.ComponentModel;

namespace IziCast.Droid
{
	class JavaWrapper : Java.Lang.Object
	{
		public object Obj { get; private set; }

		public static JavaWrapper Wrap<T>(T obj)
		{
			return new JavaWrapper
			{
				Obj = obj
			};
		}
	}
}