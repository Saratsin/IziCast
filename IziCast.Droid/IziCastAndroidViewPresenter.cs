using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using IziCast.Core;
using IziCast.Droid.Base;
using IziCast.Droid.Services;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using ContextThemeWrapper = Android.Support.V7.View.ContextThemeWrapper;

namespace IziCast.Droid
{
    public class IziCastAndroidViewPresenter : MvxAppCompatViewPresenter
    {
        private readonly List<MvxOverlayAndroidView> _overlayViews = new List<MvxOverlayAndroidView>();
        private readonly IWindowManager _windowManager;

        public IziCastAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
            _windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
        }

        private Context _overlayContext;
        public Context OverlayContext
        {
            get
            {
                if (_overlayContext == null)
                    _overlayContext = new ContextThemeWrapper(Application.Context, Resource.Style.OverlayTheme);

                return _overlayContext;
            }
        }

		public override void RegisterAttributeTypes()
		{
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxOverlayPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) => ShowOverlayView(viewType, request),
                    CloseAction = (viewModel, attribute) => CloseOverlayView(viewModel)
                });
		}

        private async void ShowOverlayView(Type viewType, MvxViewModelRequest request)
        {
            if (!OverlayPermissionService.Instance.CanDrawOverlays)
            {
                var permissionIsEnabled = await OverlayPermissionService.Instance.TryEnablePermissionIfDisabled(TimeSpan.FromMinutes(1.0));

                if (!permissionIsEnabled)
                {
                    Mvx.Resolve<OverlayChromecastButtonService>().StopSelf();
                    return;
                }
            }

            if (!viewType.IsSubclassOf(typeof(MvxOverlayAndroidView)))
                throw new NotSupportedException();

            var viewInstance = (MvxOverlayAndroidView)Activator.CreateInstance(viewType, OverlayContext);

            if (request is MvxViewModelInstanceRequest instanceRequest)
                viewInstance.ViewModel = instanceRequest.ViewModelInstance;


            ((IMvxEventSourceOverlayAndroidView)viewInstance).RaiseViewCreated();

            var parameters = viewInstance.LocationParams.ToWindowManagerLayoutParams();

            ((IMvxEventSourceOverlayAndroidView)viewInstance).RaiseViewWillAttachToWindow();

            _windowManager.AddView(viewInstance.View, parameters);
            _overlayViews.Add(viewInstance);

            ((IMvxEventSourceOverlayAndroidView)viewInstance).RaiseViewAttachedToWindow();
        }

        private bool CloseOverlayView(IMvxViewModel viewModel)
        {
            var viewToClose = _overlayViews.Find(view => view.ViewModel == viewModel);

            if (viewToClose == null)
            {
                IziCastLog.Instance.Warn($"No view found for view model: {viewModel.GetType().Name}");
                return false;
            }

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewWillDetachFromWindow();

			_overlayViews.Remove(viewToClose);
            _windowManager.RemoveView(viewToClose.View);

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewDetachedFromWindow();

            viewToClose.Dispose();

            return true;
        }
    }
}
