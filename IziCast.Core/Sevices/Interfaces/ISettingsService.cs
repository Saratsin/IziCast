using System;
namespace IziCast.Core.Services.Interfaces
{
    public interface ISettingsService
    {
        T GetValue<T>(string valueKey, T defaultValue);

        void SetValue<T>(string valueKey, T value);
    }
}
