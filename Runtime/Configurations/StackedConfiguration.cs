using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Build.Content;
using WizardUtils.Configurations;

namespace WizardUtils.Configurations
{
    /// <summary>
    /// Stacks IConfigurations, with the value of the last-added configuration having priority
    /// </summary>
    public class StackedConfiguration : IConfiguration, IDisposable
    {
        public string this[string key] => Table[key].Value;

        public IEnumerable<KeyValuePair<string, string>> Values => Table.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.Value));

        public event EventHandler<ValueChangedEventArgs> OnValueChanged;

        private List<ConfigData> Configurations;
        private Dictionary<string, EntryData> Table;

        public StackedConfiguration(params IConfiguration[] configs) : this()
        {
            foreach(var config in configs)
            {
                AddConfiguration(config);
            }
        }

        public StackedConfiguration()
        {
            Configurations = new List<ConfigData>();
            Table = new Dictionary<string, EntryData>();
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            int newSourceId = Configurations.Count;
            ConfigData newConfig = new ConfigData()
            {
                Configuration = configuration,
                CachedOnValueChanged = (_, e) =>
                {
                    OnSourceValueChanged(e, newSourceId);
                }
            };
            Configurations.Add(newConfig);
            foreach (var entry in configuration.Values)
            {
                Table[entry.Key] = new EntryData
                {
                    SourceId = newSourceId,
                    Value = entry.Value
                };
            }

            configuration.OnValueChanged += newConfig.CachedOnValueChanged;
        }

        private void OnSourceValueChanged(ValueChangedEventArgs e, int sourceId)
        {
            if (!Table.TryGetValue(e.Key, out EntryData entryData))
            {
                // if we fail to find this value in our table, let's hope we are now defining it
                if (!string.IsNullOrEmpty(e.NewValue))
                {
                    //great. 
                    var oldValue = entryData.Value;
                    entryData.Value = e.NewValue;
                    entryData.SourceId = sourceId;

                    Table[e.Key] = entryData;
                    OnValueChanged.Invoke(this, new ValueChangedEventArgs(e.Key, oldValue, e.NewValue));
                }
                else if(!string.IsNullOrEmpty(e.OldValue))
                {
                    // we are trying to unset a configuration value, but the previous value wasn't correctly loaded.
                    // somehow we became misconfigured. cry and give up
                    var configuration = Configurations[sourceId].Configuration;
                    throw new InvalidOperationException($"StackedConfiguration sub-configuration {configuration.GetType()} called OnValueChanged on uninitialized value '{e.Key}' ({e.OldValue}->NULL). somehow we are misconfigured");
                }
            }
            else
            {
                // the value exists in our table already
                string oldValue = entryData.Value;
                if (!string.IsNullOrEmpty(e.NewValue) && entryData.SourceId <= sourceId)
                {
                    // the source updated its own value, or it was replaced by one with precedence
                    entryData.Value = e.NewValue;
                    entryData.SourceId = sourceId;

                    Table[e.Key] = entryData;
                    OnValueChanged.Invoke(this, new ValueChangedEventArgs(e.Key, oldValue, e.NewValue));
                }
                else if (string.IsNullOrEmpty(e.NewValue) && entryData.SourceId == sourceId)
                {
                    // we are unsetting the value. we should fall back
                    for (int id = sourceId - 1; id >= 0; id--)
                    {
                        string newValue = Configurations[id].Configuration[e.Key];
                        if (!string.IsNullOrEmpty(newValue))
                        {
                            entryData.Value = e.NewValue;
                            entryData.SourceId = sourceId;

                            Table[e.Key] = entryData;
                            OnValueChanged.Invoke(this, new ValueChangedEventArgs(e.Key, oldValue, e.NewValue));
                            return;
                        }
                    }
                    // nothing to fall back to. so unset it
                    Table.Remove(e.Key);
                    OnValueChanged.Invoke(this, new ValueChangedEventArgs(e.Key, oldValue, e.NewValue));
                }
            }
        }

        public void Dispose()
        {
            foreach (var data in Configurations)
            {
                data.Configuration.OnValueChanged -= data.CachedOnValueChanged;
            }
        }

        private struct ConfigData
        {
            public IConfiguration Configuration;
            public EventHandler<ValueChangedEventArgs> CachedOnValueChanged;
        }

        private struct EntryData
        {
            public string Value;
            public int SourceId;
        }
    }
}
