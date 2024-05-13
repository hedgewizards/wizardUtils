using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations;
using WizardUtils.Configurations.MenuSettings;
using WizardUtils.GameSettings;

namespace WizardUtils.GameSettings
{
    public class GameSettingBool : ConfigGameSetting<bool>
    {
        public GameSettingBool(IConfigurationService configurationService, BoolSetting setting)
            : base(configurationService, setting.Key, setting.DefaultValue)
        {
        }

        public GameSettingBool(IConfigurationService configurationService, string key, bool defaultValue) : base(configurationService, key, defaultValue)
        {
        }

        public override string Serialize(bool value) => ConfigHelper.SerializeBool(value);

        public override bool TryParse(string value, out bool result) => ConfigHelper.TryParseBool(value, out result);
    }
}
