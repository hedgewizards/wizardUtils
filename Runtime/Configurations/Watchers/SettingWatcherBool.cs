using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations;
using WizardUtils.Configurations.ConfigSettings;
using WizardUtils.SettingWatchers;

namespace WizardUtils.Configuration.SettingWatchers
{
    public class SettingWatcherBool : SettingWatcher<bool>
    {
        public SettingWatcherBool(IConfigurationService configurationService, BoolSetting setting)
            : base(configurationService, setting.Key, setting.DefaultValue)
        {
        }

        public SettingWatcherBool(IConfigurationService configurationService, string key, bool defaultValue) : base(configurationService, key, defaultValue)
        {
        }

        public override string Serialize(bool value) => ConfigHelper.SerializeBool(value);

        public override bool TryParse(string value, out bool result) => ConfigHelper.TryParseBool(value, out result);
    }
}
