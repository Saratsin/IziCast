using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Binding.BindingContext;
using IziCast.Core.ViewModels;
using MvvmCross.ViewModels;
using IziCast.Core.Base;

namespace IziCast.Droid.Base
{
    public abstract class BaseActivity<TViewModel> : MvxAppCompatActivity<TViewModel> where TViewModel : class, IBaseViewModel, IMvxViewModel
    {      
        protected Toolbar _toolbar;

        protected abstract int LayoutId { get; }
        
		protected virtual int MenuId { get; } = -1;

		protected virtual int HomeIconId { get; } = -1;
      
		public new string Title
        {
            get => SupportActionBar?.Title;
            set
            {
                if (SupportActionBar != null && SupportActionBar.Title != value)
                    SupportActionBar.Title = value;
            }
        }
  
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (MenuId != -1)
                MenuInflater.Inflate(MenuId, menu);
			
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(LayoutId);

			_toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

			if (_toolbar != null)
			{
				if (HomeIconId > -1)
					SupportActionBar.SetHomeAsUpIndicator(HomeIconId);

				SetSupportActionBar(_toolbar);

				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled(true);

                this.CreateBindingSet<BaseActivity<TViewModel>, IBaseViewModel>()
					.Bind(this)
					.For(nameof(Title))
					.To(vm => vm.Title)
					.Apply();
			}
		}
    }
}
