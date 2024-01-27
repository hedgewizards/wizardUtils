using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public interface ITwoWayConfiguration : IConfiguration
    {
        public void Write(string key, string value);
        public void Save();
    }
}
