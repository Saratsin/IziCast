using System;
using System.Collections.Generic;
using IziCast.Core.Services;
using IziCast.Core.Services.Interfaces;
using Newtonsoft.Json;
using Plugin.Settings;

namespace IziCast.Droid.Services
{
    public class SettingsService : ISettingsService
    {
        private Dictionary<string, dynamic> _cachedSettings = new Dictionary<string, dynamic>();

        private dynamic GetValueFromNativeStorage(string valueKey, dynamic defaultValue, Type valueType)
        {
            switch (valueType.Name)
            {
                case nameof(Boolean):
                case nameof(DateTime):
                case nameof(Decimal):
                case nameof(Double):
                case nameof(Single):
                case nameof(Guid):
                case nameof(Int32):
                case nameof(Int64):
                case nameof(String):
                    return CrossSettings.Current.GetValueOrDefault(valueKey, defaultValue);
                default:
                    var jsonValue = CrossSettings.Current.GetValueOrDefault(valueKey, null);
                    if (jsonValue == null)
                        return defaultValue;
                    return JsonConvert.DeserializeObject(jsonValue, valueType);
            }
        }

        private void SetValueToNativeStorage(string valueKey, dynamic value, Type valueType)
        {
            switch (valueType.Name)
            {
                case nameof(Boolean):
                case nameof(DateTime):
                case nameof(Decimal):
                case nameof(Double):
                case nameof(Single):
                case nameof(Guid):
                case nameof(Int32):
                case nameof(Int64):
                case nameof(String):
                    CrossSettings.Current.AddOrUpdateValue(valueKey, value);
                    break;
                default:
                    var valueString = JsonConvert.SerializeObject((object)value);
                    CrossSettings.Current.AddOrUpdateValue(valueKey, valueString);
                    break;
            }
        }

        private dynamic GetCachedValue(string valueKey, dynamic defaultValue, Type valueType)
        {
            if (_cachedSettings.ContainsKey(valueKey))
                return _cachedSettings[valueKey];

            var value = GetValueFromNativeStorage(valueKey, defaultValue, valueType);

            _cachedSettings[valueKey] = value;

            return value;
        }

        private void SetCachedValue(string valueKey, dynamic value, Type valueType)
        {
            SetValueToNativeStorage(valueKey, value, valueType);
            _cachedSettings[valueKey] = value;
        }

        public virtual T GetValue<T>(string valueKey, T defaultValue)
        {
            return (T)GetCachedValue(valueKey, defaultValue, typeof(T));
        }

        public virtual void SetValue<T>(string valueKey, T value)
        {
            SetCachedValue(valueKey, value, typeof(T));
        }
    }
}