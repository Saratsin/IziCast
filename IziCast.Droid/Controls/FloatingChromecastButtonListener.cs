using System;
using Android.Support.Design.Widget;
using IziCast.Core;
namespace IziCast.Droid.Controls
{
    public class FloatingChromecastButtonListener : FloatingActionButton.OnVisibilityChangedListener
    {
        public event EventHandler Shown;
        public event EventHandler Hidden;

		public override void OnShown(FloatingActionButton fab)
		{
            base.OnShown(fab);

            Shown.SafeInvoke(this, EventArgs.Empty);
		}

		public override void OnHidden(FloatingActionButton fab)
		{
            base.OnHidden(fab);

            Hidden.SafeInvoke(this, EventArgs.Empty);
		}
	}
}
