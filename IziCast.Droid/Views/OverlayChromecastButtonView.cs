using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base.Views;
using IziCast.Droid.Widgets;
using MvvmCross.Droid.Views.Fragments;

namespace IziCast.Droid.Views
{
    public class OverlayChromecastButtonView : MvxOverlayAndroidView<OverlayChromecastButtonViewModel>
    {
        public OverlayChromecastButtonView(Context context) : base(context)
        { }

        public override int LayoutId { get; } = Resource.Layout.overlay_chromecast_button_view;

        public override ViewLocationParams LocationParams { get; set; } = new ViewLocationParams
        {
            Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top,
            Y = 50
        };
    }
}
