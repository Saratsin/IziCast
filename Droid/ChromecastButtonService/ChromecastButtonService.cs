using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using IziCast.Core;

namespace IziCast.Droid
{
	[Service]
	public partial class ChromecastButtonService : Service
	{
		IWindowManager WindowManager => ApplicationContext.GetSystemService(WindowService).JavaCast<IWindowManager>();

        ChromecastButton Button;
		internal Messenger Messenger;

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			Messenger = (Messenger)intent.GetParcelableExtra(MESSENGER_NAME);
			return base.OnStartCommand(intent, flags, startId);
		}

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override async void OnCreate()
		{
			base.OnCreate();

			_connectivity = new Connectivity();
            var buttonListener = new ChromecastButtonListener(this, _connectivity);

            Button = new ChromecastButton(this, buttonListener);

			var parameters = new WindowManagerLayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent,
				WindowManagerTypes.SystemAlert, WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal, Format.Translucent)
			{
				Gravity = GravityFlags.CenterVertical | GravityFlags.Top,
				Y = 50
			};

			WindowManager.AddView(Button, parameters);

			await Task.Delay(BUTTON_SHOW_ANIMATION_DELAY);
			Button.Show();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			
            if (Button != null)
			{
				Button.Hide(() =>
				{
					WindowManager.RemoveView(Button);
					Button = null;
				});
			}
		}
	}
}
