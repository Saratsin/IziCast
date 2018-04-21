using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using IziCast.Core;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base;
using IziCast.Droid.Extensions;
using IziCast.Droid.Widgets;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;

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

            button.Click += (sender, e) => 
            {
                IziCastLog.Instance.Warn("I was clicked, bitch");
            };

            set.Apply();

            frame.SetOnTouchListener(new OnTouchListener());

            return frame;
        }

        private class OnTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            public bool OnTouch(View v, MotionEvent e)
            {
                IziCastLog.Instance.Warn("Listener handled fucking touch");

                return false;
            }
        }

        public override ViewLocationParams CreateLocationParams()
        {
            return new ViewLocationParams
            {
                Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top,
                Y = 50
            };
        }
    }
}
