using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;

namespace IziCast.Droid.Base.Views
{
    public abstract class MvxOverlayAndroidView : MvxEventSourceOverlayAndroidView, IMvxView, IMvxLayoutInflaterHolder, IMvxBindingContextOwner
    {
        protected MvxOverlayAndroidView(Context context) : base(context) => Init();

        private void Init()
        {
			BindingContext = new MvxAndroidBindingContext(Context, this);

            var layoutView = this.BindingInflate(LayoutId, this, false);

            AddView(layoutView);

            this.AddEventListeners();
        }

        private Context _context;
        public new Context Context
        {
            get
            {
                if (_context == null)
                    _context = MvxContextWrapper.Wrap(base.Context, this);

                return _context;
            }
        }

        public abstract int LayoutId { get; }

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

        private LayoutInflater _layoutInflater;
        public LayoutInflater LayoutInflater
        {
            get
            {
                if (_layoutInflater == null)
                    _layoutInflater = LayoutInflater.From(Context);

                return _layoutInflater;
            }
        }

        public abstract ViewLocationParams LocationParams { get; set; }

        protected virtual void OnViewModelSet()
        {
        }
    }
}
