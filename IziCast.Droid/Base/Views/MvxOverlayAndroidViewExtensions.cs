using System;
using System.Linq;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace IziCast.Droid.Base.Views
{
    public static class MvxOverlayAndroidViewExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceOverlayAndroidView view)
        {
            if (view == null)
                return;
            
            var adapter = new MvxOverlayAndroidViewAdapter(view);
            var bindingAdapter = new MvxBindingOverlayAndroidViewAdapter(view);
        }

        public static void OnViewAttached(this IMvxView overlayView)
        {
            overlayView.OnViewCreate(() => overlayView.LoadViewModel());
        }

        public static void OnViewDetached(this IMvxView overlayView)
        {
            overlayView.OnViewDestroy();
        }

        private static IMvxViewModel LoadViewModel(this IMvxView androidView)
        {
            var viewModelType = androidView.GetType()
                                           .GetProperties()
                                           .Where(p => p.Name == nameof(androidView.ViewModel))
                                           .First(vmp => vmp.PropertyType != typeof(IMvxViewModel))
                                           .PropertyType;

            var request = new MvxViewModelRequest(viewModelType);
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, null);

            if (viewModel == null)
                Mvx.Warning("ViewModel not loaded for {0}", request.ViewModelType.FullName);

            return viewModel;
        }
    }
}
