using Android.Content;
using Android.Views;
using Android.Widget;
using IziCast.Core.ViewModels;
using IziCast.Droid.Controls;
using IziCast.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Overlay.Platforms.Android;
using AView = Android.Views.View;

namespace IziCast.Droid.Views
{
    public class ChromecastButtonOverlay : MvxOverlay<ChromecastButtonViewModel>
    {
        private FloatingChromecastButton _button;

        public ChromecastButtonOverlay(Context context) : base(context)
        { 
        }

		public override AView CreateAndSetViewBindings()
		{
		    var frame = new FrameLayout(Context)
		    {
		        LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
		    };
		    var pxPadding = Context.DpToPx(16);

		    frame.SetPadding(pxPadding, pxPadding, pxPadding, pxPadding);
		    frame.SetClipChildren(false);
		    frame.SetClipToPadding(false);

		    _button = new FloatingChromecastButton(Context)
		    {
		        LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent),
                Visibility = ViewStates.Invisible
		    };
		    frame.AddView(_button);

		    var set = this.CreateBindingSet<ChromecastButtonOverlay, ChromecastButtonViewModel>();

		    set.Bind(_button).For(v => v.Status).To(vm => vm.Status);
			set.Bind(_button).For(v => v.ShowAsyncCommand).To(vm => vm.ShowChromecastButtonAsyncCommand).OneWayToSource();
			set.Bind(_button).For(v => v.HideAsyncCommand).To(vm => vm.HideChromecastButtonAsyncCommand).OneWayToSource();
            set.Bind(_button).For(nameof(AView.Click)).To(vm => vm.ConnectButtonClickedCommand);
            set.Bind(_button).For(nameof(AView.LongClick)).To(vm => vm.ConnectButtonLongClickedCommand);

		    set.Apply();

		    return frame;
		}

		protected override void OnViewWillAttachToWindow()
		{
            base.OnViewWillAttachToWindow();

            _button.InvalidateVisibilityCommandsBindings();
		}

		public override OverlayLocationParams CreateLocationParams()
        {
            return new OverlayLocationParams(
                gravity: GravityFlags.CenterHorizontal | GravityFlags.Top, 
                y: 50
            );
        }
    }
}
