using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class WritableConfiguration : IWritableConfiguration
    {
        public Dictionary<string, string> Data { get; set; }
        public event EventHandler<ValueChangedEventArgs> OnValueChanged;

        public WritableConfiguration()
        {
            Data = new Dictionary<string, string>();
        }

        public string Read(string key)
        {
            if (Data.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public void Write(string key, string value)
        {
            OnValueChanged?.Invoke(this, new ValueChangedEventArgs(key, Read(key), value));
            Data[key] = value;
        }

        public void Save() { }

        public IEnumerable<KeyValuePair<string, string>> Values => Data;
    }
}
