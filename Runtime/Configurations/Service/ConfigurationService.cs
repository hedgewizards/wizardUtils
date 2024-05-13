using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations.ConfigSettings;

namespace WizardUtils.Configurations
{
    public class ConfigurationService : IConfigurationService
    {
        private StackedConfiguration FullConfiguration;
        private IWritableConfiguration FileConfiguration;
        private WritableConfiguration LiveConfiguration;
        private SettingManifest IndexedSettings;

        public Dictionary<string, EventHandler<ValueChangedEventArgs>> ValueChangedDictionary;

        public ConfigurationService(
            SettingManifest indexedSettings,
            IWritableConfiguration fileConfiguration,
            IConfiguration OverrideConfiguration = null)
        {
            IndexedSettings = indexedSettings;
            ValueChangedDictionary = new Dictionary<string, EventHandler<ValueChangedEventArgs>>();
            FileConfiguration = fileConfiguration;
            LiveConfiguration = new WritableConfiguration();
            FullConfiguration = new StackedConfiguration();
            FullConfiguration.AddConfiguration(FileConfiguration);
            if (OverrideConfiguration != null)
            {
                FullConfiguration.AddConfiguration(OverrideConfiguration);
            }
            FullConfiguration.AddConfiguration(LiveConfiguration);

            FullConfiguration.OnValueChanged += FullConfiguration_OnValueChanged;
        }
        
        public string Read(string key, string defaultValue = null)
        {
            string value = FullConfiguration.Read(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public void Write(string key, string value, bool persist = false)
        {
            LiveConfiguration.Write(key, value);
            if (persist)
            {
                FileConfiguration.Write(key, value);
            }
        }

        public void AddListener(string key, EventHandler<ValueChangedEventArgs> listener)
        {
            if (ValueChangedDictionary.TryGetValue(key, out var existingListener))
            {
                existingListener += listener;
                ValueChangedDictionary[key] = existingListener;
            }
            else
            {
                ValueChangedDictionary.Add(key, listener);
            }
        }

        public void RemoveListener(string key, EventHandler<ValueChangedEventArgs> listener)
        {
            if (ValueChangedDictionary.TryGetValue(key, out var existingListener))
            {
                existingListener -= listener;
                ValueChangedDictionary[key] = existingListener;
            }
            else
            {
                throw new KeyNotFoundException(key);
            }
        }

        private void FullConfiguration_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ValueChangedDictionary.TryGetValue(e.Key, out var existingListener))
            {
                existingListener?.Invoke(this, e);
            }
        }

        public void Save()
        {
            FileConfiguration.Save();
        }

        public bool TryFindSetting(string key, out SettingDescriptor setting)
        {
            return IndexedSettings.TryFindByKey(key, out setting);
        }
    }
}
