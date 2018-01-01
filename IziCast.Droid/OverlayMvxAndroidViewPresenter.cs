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

namespace IziCast.Droid
{
    public class OverlayMvxAndroidViewPresenter : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        private readonly Context _context;

        public OverlayMvxAndroidViewPresenter(Context context) => _context = context;

        private IWindowManager _windowManager;
        private IWindowManager WindowManager
        {
            get
            {
                if (_windowManager == null)
                    _windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

                return _windowManager;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        private IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
        }

        private IMvxLog _logger;
        private IMvxLog Logger
        {
            get
            {
                if (_logger == null)
                    _logger = Mvx.Resolve<IMvxLog>();
                return _logger;
            }
        }

        private List<MvxOverlayAndroidView> CurrentViews { get; } = new List<MvxOverlayAndroidView>();

        public override void Show(MvxViewModelRequest request)
        {
            var viewToShow = CreateView(request);
            var parameters = CreateWindowManagerParams(viewToShow.LocationParams);

            WindowManager.AddView(viewToShow, parameters);
            CurrentViews.Add(viewToShow);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var viewToClose = CurrentViews.Find(fragment => fragment.ViewModel == viewModel);

            if (viewToClose == null)
            {
                Logger.Warn($"No fragment found for view model: {viewModel.GetType().Name}");
                return;
            }

			CurrentViews.Remove(viewToClose);
            WindowManager.RemoveView(viewToClose);

            if (CurrentViews.Count == 0)
                Mvx.Resolve<IMvxOverlayService>().StopSelf();
        }

        public override void ChangePresentation(MvxPresentationHint hint) => throw new NotSupportedException();

        private MvxOverlayAndroidView CreateView(MvxViewModelRequest request)
        {
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            if (viewType == null || !viewType.IsSubclassOf(typeof(MvxOverlayAndroidView)))
                throw new NotSupportedException();

            var viewInstance = (MvxOverlayAndroidView)Activator.CreateInstance(viewType, _context);

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
