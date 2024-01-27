using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class ConfigurationService : IConfigurationService
    {
        private StackedConfiguration FullConfiguration;
        private CfgFileConfiguration FileConfiguration;
        private WritableConfiguration LiveConfiguration;

        public Dictionary<string, EventHandler<ValueChangedEventArgs>> ValueChangedDictionary;

        public ConfigurationService(ITwoWayConfiguration FileConfiguration, IConfiguration OverrideConfiguration = null)
        {
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
            throw new NotImplementedException();
        }
    }
}
