using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations;
using WizardUtils.Configurations.Settings;
using WizardUtils.SettingWatchers;

namespace WizardUtils.SettingWatchers
{
    public class SettingWatcherFloat : SettingWatcher<float>
    {
        public SettingWatcherFloat(IConfigurationService configurationService, RangeFloatSetting setting)
            : base(configurationService, setting.Key, setting.DefaultValue)
        {

        }

        public SettingWatcherFloat(IConfigurationService configurationService, string key, float defaultValue) : base(configurationService, key, defaultValue)
        {
        }

        public override string Serialize(float value) => ConfigHelper.SerializeFloat(value);

        public override bool TryParse(string value, out float result) => ConfigHelper.TryParseFloat(value, out result);
    }
}
