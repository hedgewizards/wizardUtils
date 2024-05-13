using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.Configurations;
using WizardUtils.GameSettings;

namespace WizardUtils.GameSettings
{
    public class GameSettingFloat : ConfigGameSetting<float>
    {
        public GameSettingFloat(IConfigurationService configurationService, string key, float defaultValue) : base(configurationService, key, defaultValue)
        {
        }

        public override string Serialize(float value) => ConfigHelper.SerializeFloat(value);

        public override bool TryParse(string value, out float result) => ConfigHelper.TryParseFloat(value, out result);
    }
}
