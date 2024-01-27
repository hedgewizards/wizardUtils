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
        private WritableConfiguration MainConfiguration;

        public Dictionary<string, EventHandler<ValueChangedEventArgs>> ValueChangedDictionary;

        public ConfigurationService(IConfiguration BaseConfiguration)
        {
            MainConfiguration = new WritableConfiguration();
            FullConfiguration = new StackedConfiguration(
                BaseConfiguration,
                MainConfiguration);

            FullConfiguration.OnValueChanged += FullConfiguration_OnValueChanged;
        }
        
        public string GetOption(string key, string defaultValue = null)
        {
            string value = FullConfiguration[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public void SetOption(string key, string value)
        {
            MainConfiguration[key] = value;
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
    }
}
