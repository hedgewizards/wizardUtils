using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public interface IConfiguration
    {
        public string Read(string key);

        public event EventHandler<ValueChangedEventArgs> OnValueChanged;

        IEnumerable<KeyValuePair<string, string>> Values { get; }
    }
}
