using System;
using System.Threading.Tasks;

namespace IziCast.Core.Sevices.Interfaces
{
    public interface IUserInteractionService
    {
        Task ShowToastAsync(string text, bool longDuration = false);
    }
}
