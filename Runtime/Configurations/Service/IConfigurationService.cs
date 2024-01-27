using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public interface IConfigurationService
    {
        public string Read(string key, string defaultValue = null);

        public void Write(string key, string value, bool writeToConfig = false);

        public void Save();

        public void AddListener(string key, EventHandler<ValueChangedEventArgs> listener);
        public void RemoveListener(string key, EventHandler<ValueChangedEventArgs> listener);
    }
}
