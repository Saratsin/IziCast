using System;
namespace IziCast.Core
{
	public enum ConnectivityStatus
	{
		Connecting,
		Connected,
		Disconnected
	}

	public class Connectivity
	{
		public event EventHandler<ConnectivityStatus> StatusChanged;

		void RaiseStatusChanged()
		{
			StatusChanged?.Invoke(this, _status);
		}

		ConnectivityStatus _status = ConnectivityStatus.Disconnected;
		public ConnectivityStatus Status
		{
			get { return _status; }
			set
			{
				if (_status != value)
				{
					_status = value;
					RaiseStatusChanged();
				}
			}
		}
	}
}
