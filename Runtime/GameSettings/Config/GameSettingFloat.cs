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

        public override bool TryParse(string key, out float value) => float.TryParse(key, out value);
    }
}
