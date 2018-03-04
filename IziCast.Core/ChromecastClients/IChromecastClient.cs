using System;
using System.Threading.Tasks;
using IziCast.Core.Models;

namespace IziCast.Core.Services
{
    public interface IChromecastClient
    {
        void SetMediaData(string mediaContentUrl, string mediaContentType);

        Task<Try> SendMediaToChromecast();
    }
}
