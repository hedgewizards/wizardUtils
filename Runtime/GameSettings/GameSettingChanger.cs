using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WizardUtils.GameSettings;

namespace WizardUtils
{
    public class GameSettingChanger : MonoBehaviour
    {
        public string SettingKeyName;
        GameSettingFloat setting;
        public UnityEvent<float> OnValueLoaded;

        private void Start()
        {
            setting = GameManager.Instance.FindGameSetting(SettingKeyName);
            if (setting == null)
            {
                Debug.LogError($"Could not find GameSetting with Key {SettingKeyName}", gameObject);
            }
            OnValueLoaded?.Invoke(setting.Value);
        }

        public void SetValue(float newValue)
        {
#if DEBUG
            if (setting == null)
            {
                Debug.LogError("GameSettingChanger not set up yet D:", gameObject);
                return;
            }
#endif
            setting.Value = newValue;
        }
    }
}