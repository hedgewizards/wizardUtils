using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.Configuration.SettingWatchers;
using WizardUtils.Configurations;

namespace WizardUtils.SettingWatchers
{
    public abstract class SettingWatcher<T>
    {
        private IConfigurationService ConfigurationService;
        public readonly string Key;
        private T DefaultValue;

        public event EventHandler<SettingChangedEventArgs<T>> OnChanged;

        protected SettingWatcher(IConfigurationService configurationService, string key, T defaultValue)
        {
            ConfigurationService = configurationService;
            Key = key;
            DefaultValue = defaultValue;
            configurationService.AddListener(key, ValueChanged);
        }

        private void ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!TryParse(e.OldValue, out T oldValue))
            {
                oldValue = DefaultValue;
            }
            if (!TryParse(e.NewValue, out T newValue))
            {
                newValue = DefaultValue;
            }
            if (!oldValue.Equals(newValue))
            {
                OnChanged.Invoke(sender, new SettingChangedEventArgs<T>(oldValue, newValue));
            }
        }

        public T Value
        {
            get
            {
                string textValue = ConfigurationService.Read(Key, null);
                if (string.IsNullOrEmpty(textValue)) return DefaultValue;
                if (TryParse(textValue, out T value))
                {
                    return value;
                }
                else
                {
                    Debug.LogWarning($"{GetType()} failed to parse string to type '{typeof(T)}' (value: {textValue})");
                    return DefaultValue;
                }
            }
            set
            {
                ConfigurationService.Write(Key, value.ToString());
            }
        }

        public abstract string Serialize(T value);
        public abstract bool TryParse(string value, out T result);
    }
}
