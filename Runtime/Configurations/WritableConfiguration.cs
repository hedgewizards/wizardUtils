using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class WritableConfiguration : IConfiguration
    {
        public Dictionary<string, string> Data { get; set; }
        public event EventHandler<ValueChangedEventArgs> OnValueChanged;

        public WritableConfiguration()
        {
            Data = new Dictionary<string, string>();
        }


        public string this[string key]
        {
            get
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
            set
            {
                OnValueChanged?.Invoke(this, new ValueChangedEventArgs(key, this[key], value));
                Data[key] = value;
            }
        }

        public IEnumerable<KeyValuePair<string, string>> Values => Data;
    }
}
