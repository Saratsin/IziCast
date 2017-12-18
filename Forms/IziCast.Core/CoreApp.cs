using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpCaster.Controllers;
using SharpCaster.Services;

namespace IziCast.Core
{
	public class CoreApp
	{
		static CoreApp _current;
		public static CoreApp Current => _current;

		readonly string _ipAddress;

		public CoreApp(string ipAddress)
		{
			_current = this;
			_ipAddress = ipAddress;
		}

		public bool ValidateUri(string videoUri)
		{
			if (Uri.IsWellFormedUriString(videoUri, UriKind.Absolute))
			{
				var uri = default(Uri);
				Uri.TryCreate(videoUri, UriKind.RelativeOrAbsolute, out uri);

				if (uri != null && (uri.Scheme == "http" || uri.Scheme == "https"))
				{
					try
					{
						var extension = System.IO.Path.GetExtension(videoUri);
						if (extension == ".mp4")
							return true;
					}
					catch
					{
						return false;
					}
				}
			}

			return false;
		}

		public async Task ConnectToChromecast(Connectivity connectivity, string videoUri)
		{
			var currentService = new ChromecastService();
			try
			{
				connectivity.Status = ConnectivityStatus.Connecting;

				var ipAddress = _ipAddress;
				var chromecasts = await currentService.StartLocatingDevices(ipAddress);
				var chromecast = chromecasts.FirstOrDefault();
				if (chromecast != null)
				{
					var _controller = default(SharpCasterDemoController);
					currentService.ChromeCastClient.ConnectedChanged += async (sender, e) =>
					{
						try
						{
							if (_controller == null)
								_controller = await currentService.ChromeCastClient.LaunchSharpCaster();
						}
						catch (Exception ex)
						{
                            connectivity.Status = ConnectivityStatus.Disconnected;
							Debug.WriteLine(ex.Message + ex.StackTrace);
						}
					};
					currentService.ChromeCastClient.ApplicationStarted += async (sender, e) =>
					{
						try
						{
							while (_controller == null)
								await Task.Delay(500);

							var extension = System.IO.Path.GetExtension(videoUri);

							var contentType = default(string);

							switch (extension)
							{
								case ".mp4":
									contentType = "video/mp4";
									break;
								default:
									contentType = "video/*";
									break;

							}

							await _controller.LoadMedia(videoUri, contentType, null);
							connectivity.Status = ConnectivityStatus.Connected;
						}
						catch (Exception ex)
						{
                            connectivity.Status = ConnectivityStatus.Disconnected;
							Debug.WriteLine(ex.Message + ex.StackTrace);
						}
					};

					await currentService.ConnectToChromecast(chromecast);
				}
				else
				{
                    connectivity.Status = ConnectivityStatus.Disconnected;
				}
			}
			catch (Exception ex)
			{
                connectivity.Status = ConnectivityStatus.Disconnected;
				Debug.WriteLine(ex.Message + ex.StackTrace);
			}
		}
	}
}