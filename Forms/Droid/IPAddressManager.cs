using System;
using System.Collections.Generic;
using System.Net;
using IziCast.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(IPAddressManager))]
namespace IziCast.Droid
{
	public class IPAddressManager : IIPAddressManager
	{
		internal static string IP_Address
		{
			get
			{
				var addresses = Dns.GetHostAddresses(Dns.GetHostName());

				if (addresses?[0] != null)
					return addresses[0].ToString();
				else
					return null;
			}
		}

		public string IPAddress => IP_Address;
	}
}
