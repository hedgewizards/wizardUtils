using System;
using System.Collections.Generic;
using UnityEngine;
using WizardUtils.GameSettings.Legacy;

namespace WizardUtils.GameSettings
{
    public class PlayerPrefsGameSettingService : IGameSettingService
    {
        Dictionary<string, LegacyGameSettingFloat> GameSettings;

        public PlayerPrefsGameSettingService(IEnumerable<LegacyGameSettingFloat> settings)
        {
            GameSettings = new Dictionary<string, LegacyGameSettingFloat>();
            foreach (LegacyGameSettingFloat newSetting in settings)
            {
                GameSettings.Add(newSetting.Key, newSetting);
                newSetting.OnChanged += (sender, e) => OnGameSettingChanged(newSetting, e);
            }
        }

        public LegacyGameSettingFloat GetSetting(string key)
        {
            return GameSettings[key];
        }

        public void Save()
        {
        }

        private void OnGameSettingChanged(LegacyGameSettingFloat setting, GameSettingChangedEventArgs<float> e)
        {
            PlayerPrefs.SetFloat(setting.Key, e.FinalValue);
        }
    }
}
