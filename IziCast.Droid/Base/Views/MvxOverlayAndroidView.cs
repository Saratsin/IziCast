using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;

namespace IziCast.Droid.Base.Views
{
    public abstract class MvxOverlayAndroidView : MvxEventSourceOverlayAndroidView, IMvxView, IMvxBindingContextOwner, IMvxLayoutInflaterHolder
    {
        private readonly MvxOverlayAndroidViewAdapter _viewAdapter;

        protected MvxOverlayAndroidView(Context context)
        {
            Context = MvxContextWrapper.Wrap(context, this);

            BindingContext = new MvxAndroidBindingContext(Context, this);

            _viewAdapter = new MvxOverlayAndroidViewAdapter(this);
        }

		public View View { get; private set; }
  
        public ViewLocationParams LocationParams { get; private set; }
  
        public Context Context { get; }

        public IMvxBindingContext BindingContext { get; set; }

		public object DataContext 
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        private MvxLayoutInflater _layoutInflater;
        public LayoutInflater LayoutInflater
        {
            get
            {
                if(_layoutInflater == null)
                    _layoutInflater = (MvxLayoutInflater)LayoutInflater.From(Context);

                return _layoutInflater;
            }
        }

        public abstract View CreateAndSetViewBindings();

        public abstract ViewLocationParams CreateLocationParams();

        protected virtual void OnViewModelSet()
        {
        }

        protected override void OnViewCreated()
        {
            LocationParams = CreateLocationParams();
        }

        protected override void OnViewWillAttachToWindow()
        {
            View = this.BindingInflate(Resource.Layout.overlay_chromecast_button_view, null);

            View = CreateAndSetViewBindings();
        }

        protected override void Dispose(bool disposing)
        {
			_viewAdapter.Dispose();
            View.Dispose();
            base.Dispose(disposing);
        }
    }
}
