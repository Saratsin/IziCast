using System;
using System.Threading.Tasks;
using IziCast.Core.Enums;
using MvvmCross.Core.ViewModels;

namespace IziCast.Core.ViewModels
{
    public class OverlayChromecastButtonViewModel : BaseViewModel
    {
        public OverlayChromecastButtonViewModel()
        {
        }

        ConnectivityStatus _status = ConnectivityStatus.Disconnected;
        public ConnectivityStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        bool _buttonIsVisible = false;
        public bool ButtonIsVisible
        {
            get => _buttonIsVisible;
            set => SetProperty(ref _buttonIsVisible, value);
        }

        //public async Task Connect()
        //{
        //    Close();
        //}
    }
}
