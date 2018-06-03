using IziCast.Core.Models.IsBusyHandler;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.Localization;

namespace IziCast.Core.Base
{
    public abstract class BaseViewModel : MvxViewModel, IBaseViewModel
    {
        private readonly string _defaultTitle;

        protected BaseViewModel()
        {
            TextSource = new MvxLanguageBinder(string.Empty, GetType().Name);

            _defaultTitle = TextSource.GetText(nameof(Title));
        }

		public MvxLanguageBinder TextSource { get; }

		public IMvxNavigationService NavigationService { get; } = Mvx.Resolve<IMvxNavigationService>();

        public IsBusyHandler Handler { get; } = new IsBusyHandler();

		public virtual string Title => _defaultTitle;
    }
}
