using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class ExplicitConfiguration : IConfiguration
    {
        private Dictionary<string, string> Data;

        public ExplicitConfiguration(ExplicitConfigurationData data)
        {
            Data = new Dictionary<string, string>();
            foreach(var item in data.Entries)
            {
                if (Data.ContainsKey(item.Key))
                {
                    throw new ArgumentException($"Explicit Configuration contains duplicate key {item.Key}");
                }
                Data[item.Key] = item.Value;
            }
        }

        public string this[string key] => Data.GetValueOrDefault(key);

        public IEnumerable<KeyValuePair<string, string>> Values => Data;

        public event EventHandler<ValueChangedEventArgs> OnValueChanged;
    }
}
