using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;

namespace IziCast.Droid
{
	[Service]
	partial class ChromecastButtonService : Service
	{
		internal IWindowManager WindowManager => ApplicationContext.GetSystemService(WindowService).JavaCast<IWindowManager>();

		internal FloatingActionButton Button;
		internal Messenger Messenger;
		ChromecastButtonListener _buttonListener;

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

			var themedContext = new ContextThemeWrapper(this, Resource.Style.Theme_AppCompat_NoActionBar);

			Button = new FloatingActionButton(themedContext)
			{
				BackgroundTintList = new ColorStateList(new int[][] { new int[0] }, new int[] { Color.ParseColor("#03A9F4") }),
				Visibility = ViewStates.Invisible,
				Elevation = 0f,
				CompatElevation = 0f
			};
			Button.SetImageResource(Resource.Drawable.mr_button_connecting_dark);

			_buttonListener = new ChromecastButtonListener(this);
			Button.SetOnTouchListener(_buttonListener);

			var parameters = new WindowManagerLayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent,
				WindowManagerTypes.SystemAlert, WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal, Format.Translucent)
			{
				Gravity = GravityFlags.CenterVertical | GravityFlags.Top,
				Y = 50
			};


			WindowManager.AddView(Button, parameters);

			await Task.Delay(BUTTON_SHOW_ANIMATION_DELAY);
			Button.Show(_buttonListener);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			if (Button != null)
			{
				Button.Hide(_buttonListener);
			}
		}
	}
}
