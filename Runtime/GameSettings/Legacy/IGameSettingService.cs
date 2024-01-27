using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.GameSettings.Legacy;

namespace WizardUtils.GameSettings
{
    [Obsolete]
    public interface IGameSettingService
    {
        public LegacyGameSettingFloat GetSetting(string key);
        public void Save();
    }
}
