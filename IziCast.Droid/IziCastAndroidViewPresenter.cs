using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using IziCast.Droid.Base;
using IziCast.Droid.Services;
using MvvmCross.Presenters;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Views;
using MvvmCross;
using MvvmCross.ViewModels;
using IziCast.Core;
using MvvmCross.Logging;
using MvvmCross.Presenters.Hints;
using MvvmCross.Droid.Support.V7.AppCompat;
using System.Reflection;
using Android.App;
using IziCast.Core.Sevices;
using IziCast.Core.Enums;
using MvvmCross.WeakSubscription;
using MvvmCross.Platforms.Android.Binding.Views;
using Android.Content.Res;

namespace IziCast.Droid
{
    public class IziCastAndroidViewPresenter : MvxAppCompatViewPresenter
    {
        private readonly List<MvxOverlayAndroidView> _currentViews = new List<MvxOverlayAndroidView>();
        private readonly IWindowManager _windowManager;

        public IziCastAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
            _windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            Mvx.Resolve<ILaunchModeService>().WeakSubscribe(nameof(ILaunchModeService.LaunchModeWillChange), OnLaunchModeWillChange);
        }

        private Context OverlayContext { get; } = new ContextThemeWrapper(Application.Context, Resource.Style.OverlayTheme);

        private LaunchMode LaunchMode => Mvx.Resolve<ILaunchModeService>().LaunchMode;

        private void OnLaunchModeWillChange(object sender, EventArgs e)
        {
            CloseAll();
        }

        private MvxOverlayAndroidView CreateView(MvxViewModelRequest request)
        {
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            if (viewType == null || !viewType.IsSubclassOf(typeof(MvxOverlayAndroidView)))
                throw new NotSupportedException();
            
            var viewInstance = (MvxOverlayAndroidView)Activator.CreateInstance(viewType, OverlayContext);

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

        public override async void Show(MvxViewModelRequest request)
        {
            if (LaunchMode != LaunchMode.Overlay)
            {
                base.Show(request);
                return;
            }

            if(!OverlayPermissionService.Instance.CanDrawOverlays)
            {
                var permissionIsEnabled = await OverlayPermissionService.Instance.TryEnablePermissionIfDisabled(TimeSpan.FromSeconds(15));

                if (!permissionIsEnabled)
                {
                    Mvx.Resolve<IOverlayChromecastButtonService>().StopSelf();
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
            if (LaunchMode != LaunchMode.Overlay)
            {
                base.Close(viewModel);
                return;
            }

            var viewToClose = _currentViews.Find(view => view.ViewModel == viewModel);

            if (viewToClose == null)
            {
                IziCastLog.Instance.Warn($"No view found for view model: {viewModel.GetType().Name}");
                return;
            }

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewWillDetachFromWindow();

			_currentViews.Remove(viewToClose);
            _windowManager.RemoveView(viewToClose.View);

            ((IMvxEventSourceOverlayAndroidView)viewToClose).RaiseViewDetachedFromWindow();

            viewToClose.Dispose();

            if (_currentViews.Count == 0)
                Mvx.Resolve<IOverlayChromecastButtonService>().StopSelf();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (LaunchMode != LaunchMode.Overlay)
            {
                base.ChangePresentation(hint);
                return;
            }

            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            IziCastLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
        }

        public void CloseAll()
        {
            foreach (var view in _currentViews)
                Close(view.ViewModel);
        }
    }
}
