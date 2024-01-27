using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public interface IConfigurationService
    {
        public string GetOption(string key, string defaultValue = null);

        public void SetOption(string key, string value);
    }
}
