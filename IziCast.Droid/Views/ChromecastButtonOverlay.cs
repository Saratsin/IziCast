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

		    var button = new ChromecastButton(Context)
		    {
		        LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
		    };
		    frame.AddView(button);

		    var set = this.CreateBindingSet<ChromecastButtonOverlay, ChromecastButtonViewModel>();

		    set.Bind(button).For(v => v.Status).To(vm => vm.Status);
            set.Bind(button).For(nameof(AView.Click)).To(vm => vm.ConnectButtonClickedCommand);
            set.Bind(button).For(nameof(AView.LongClick)).To(vm => vm.ConnectButtonLongClickedCommand);

		    set.Apply();

		    return frame;
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
