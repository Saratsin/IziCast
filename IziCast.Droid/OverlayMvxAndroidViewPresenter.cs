using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using IziCast.Droid.Base.Services;
using IziCast.Droid.Base.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using IziCast.Droid.Services;

namespace IziCast.Droid
{
    public class OverlayMvxAndroidViewPresenter : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        private readonly List<MvxOverlayAndroidView> _currentViews = new List<MvxOverlayAndroidView>();
        private readonly IMvxLog _logger = Mvx.Resolve<IMvxLog>();
        private readonly IMvxViewsContainer _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
        private readonly IziCastContext _context;
        private readonly IWindowManager _windowManager;

        public OverlayMvxAndroidViewPresenter(IziCastContext context)
        {
            _context = context;
            _windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
        }

        public override async void Show(MvxViewModelRequest request)
        {
            if(!OverlayPermissionService.Instance.CanDrawOverlays)
            {
                var permissionIsEnabled = await OverlayPermissionService.Instance.TryEnablePermissionIfDisabled(TimeSpan.FromSeconds(15));

                if (!permissionIsEnabled)
                {
                    Mvx.Resolve<IMvxOverlayService>().StopSelf();
                    return;
                }
            }

            var viewToShow = CreateView(request);

            ((IMvxEventSourceOverlayAndroidView)viewToShow).RaiseViewCreated();

            var parameters = CreateWindowManagerParams(viewToShow.LocationParams);

            ((IMvxEventSourceOverlayAndroidView)viewToShow).RaiseViewWillAttachToWindow();

            _windowManager.AddView(viewToShow.View, parameters);
            _currentViews.Add(viewToShow);

            ((IMvxEventSourceOverlayAndroidView)viewToShow).RaiseViewAttachedToWindow();
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var viewToClose = _currentViews.Find(view => view.ViewModel == viewModel);

            if (viewToClose == null)
            {
                _logger.Warn($"No view found for view model: {viewModel.GetType().Name}");
                return;
            }

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewWillDetachFromWindow();

			_currentViews.Remove(viewToClose);
            _windowManager.RemoveView(viewToClose.View);

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewDetachedFromWindow();

            viewToClose.Dispose();

            if (_currentViews.Count == 0)
                Mvx.Resolve<IMvxOverlayService>().StopSelf();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            _logger.Warn("Hint ignored {0}", hint.GetType().Name);
        }

        private MvxOverlayAndroidView CreateView(MvxViewModelRequest request)
        {
            var viewType = _viewsContainer.GetViewType(request.ViewModelType);

            if (viewType == null || !viewType.IsSubclassOf(typeof(MvxOverlayAndroidView)))
                throw new NotSupportedException();
            
            var viewInstance = (MvxOverlayAndroidView)Activator.CreateInstance(viewType, new MutableContextWrapper(_context));

            if (request is MvxViewModelInstanceRequest instanceRequest)
                viewInstance.ViewModel = instanceRequest.ViewModelInstance;

            return viewInstance;
        }

        private WindowManagerLayoutParams CreateWindowManagerParams(ViewLocationParams locationParams)
        {
            var type = WindowManagerTypes.Phone;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                type = WindowManagerTypes.ApplicationOverlay;


            return new WindowManagerLayoutParams(w: locationParams.Width,
                                                 h: locationParams.Height,
                                                 xpos: locationParams.X,
                                                 ypos: locationParams.Y,
                                                 _type: type,
                                                 _flags: WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
                                                 _format: Format.Translucent)
            {
                Gravity = locationParams.Gravity
            };
        }
    }
}
