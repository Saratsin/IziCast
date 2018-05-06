using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IziCast.Core.Models;

namespace IziCast.Core.Sevices
{
    public interface IVideoSenderService
    {
        string IziCastAppId { get; }

        ReadOnlyCollection<IVideoSender> PhoneVideoSenders { get; }

        ReadOnlyCollection<IVideoSender> ChromecastVideoSenders { get; }

		IVideoSender CurrentPhoneVideoSender { get; set; }
		
		IVideoSender CurrentChromecastVideoSender { get; set; }

        Task<Try> EnsureAtLeastOneVideoSenderIsAvailable();
    }
}
