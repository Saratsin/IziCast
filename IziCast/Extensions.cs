using System;

namespace IziCast
{
	public static class Extensions
	{
		public static string RemoveEnd(this string obj, string ending)
		{
			var index = obj.LastIndexOf(ending, StringComparison.Ordinal);
			if (index >= 0)
				return obj.Substring(0, index);

			return obj;
		}
	}
}
