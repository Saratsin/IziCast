using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using IziCast.Core;

namespace IziCast.Droid
{
	public partial class ChromecastButtonService
	{
		const int BUTTON_SHOW_ANIMATION_DELAY = 2000;
		const string MESSENGER_NAME = "com.tsh.izicast.BUTTON_MESSENGER";

		static Connectivity _connectivity;
		static bool _autoClose;
		static Func<Connectivity, Task> _buttonClickAction;

		public static async Task CallService(Context context, Func<Connectivity, Task> buttonClickAction, int milliseconds = 0)
		{
			_autoClose = true;
			_buttonClickAction = buttonClickAction;
			var handler = new Handler(OnConnectButtonClicked);
			var messenger = new Messenger(handler);
			var svc = new Intent(context, typeof(ChromecastButtonService));
			svc.PutExtra(MESSENGER_NAME, messenger);
			context.StartService(svc);
			if (milliseconds != 0)
			{
				await Task.Delay(milliseconds + BUTTON_SHOW_ANIMATION_DELAY);
				if (_autoClose)
					context.StopService(svc);
			}
		}

        static async void OnConnectButtonClicked(Message msg)
		{
            if (msg.What == 42) //Handles button clicked
            {
				_autoClose = false;
				var task = _buttonClickAction?.Invoke(_connectivity);
				if (task != null)
					await task;
            }
		}
	}
}