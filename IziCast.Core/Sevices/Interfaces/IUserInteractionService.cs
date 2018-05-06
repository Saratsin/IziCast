using System;
using System.Threading.Tasks;

namespace IziCast.Core.Services.Interfaces
{
    public interface IUserInteractionService
    {
        Task ShowToastAsync(string text, bool longDuration = false);
    }
}
