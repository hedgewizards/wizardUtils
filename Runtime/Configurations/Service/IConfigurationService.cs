using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations.Settings;

namespace WizardUtils.Configurations
{
    public interface IConfigurationService
    {
        public string Read(string key, string defaultValue = null);

        public void Write(string key, string value, bool writeToConfig = false);

        public void Save();

        public void AddListener(string key, EventHandler<ValueChangedEventArgs> listener);
        public void RemoveListener(string key, EventHandler<ValueChangedEventArgs> listener);
        public bool TryFindSetting(string key, out SettingDescriptor setting);
        public SettingDescriptor FindSetting(string key)
        {
            if (!TryFindSetting(key, out var setting))
            {
                throw new KeyNotFoundException($"Couldn't find IndexedSetting with key '{key}'");
            }

            return setting;
        }
        public T FindSetting<T>(string key) where T : SettingDescriptor
        {
            SettingDescriptor setting = FindSetting(key);

            if (setting is not T castedSetting)
            {
                throw new InvalidCastException($"Setting for key '{key}' of unexpected type '{setting.GetType()}' (expected {typeof(T)})");
            }

            return castedSetting;
        }
    }
}
