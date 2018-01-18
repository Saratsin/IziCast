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
            Init(); 
        }

        async void Init()
        {
            while (true)
            {
                switch (Status)
                {
                    case ConnectivityStatus.Connected:
                        Status = ConnectivityStatus.Disconnected;
                        break;
                    case ConnectivityStatus.Connecting:
                        Status = ConnectivityStatus.Connected;
                        break;
                    case ConnectivityStatus.Disconnected:
                        Status = ConnectivityStatus.Connecting;
                        break;
                }

                await Task.Delay(1000);
            }
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
