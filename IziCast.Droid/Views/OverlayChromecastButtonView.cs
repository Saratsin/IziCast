using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base.Views;
using IziCast.Droid.Extensions;
using IziCast.Droid.Widgets;
using MvvmCross.Binding.BindingContext;

namespace IziCast.Droid.Views
{
    public class OverlayChromecastButtonView : MvxOverlayAndroidView<OverlayChromecastButtonViewModel>
    {
        public OverlayChromecastButtonView(Context context) : base(context)
        { 
        }

        public override View CreateAndSetViewBindings()
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

            var set = this.CreateBindingSet<OverlayChromecastButtonView, OverlayChromecastButtonViewModel>();
            set.Bind(button).For(v => v.Status).To(vm => vm.Status);
            set.Bind(button).For("Click").To(vm => vm.ConnectButtonClickedCommand);
            set.Bind(button).For("LongClick").To(vm => vm.ConnectButtonLongClickedCommand);
            set.Apply();


            return frame;
        }

        public override ViewLocationParams CreateLocationParams() => new ViewLocationParams
        {
            Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top,
            Y = 50
        };
    }
}
