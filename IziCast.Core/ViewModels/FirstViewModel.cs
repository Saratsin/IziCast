namespace IziCast.Core.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
        private string hello = "Hello MvvmCross";
        public string Hello
        {
            get { return hello; }
            set { SetProperty(ref hello, value); }
        }
    }
}
