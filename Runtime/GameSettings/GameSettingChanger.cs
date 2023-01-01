using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WizardUtils;

namespace WizardUtils
{
    public class GameSettingChanger : MonoBehaviour
    {
        [HideInInspector]
        public GameSettingDescriptor GameSettingDescriptor;
        [HideInInspector]
        public string SettingKey;
        GameSettingFloat setting;
        public UnityEvent<float> OnValueLoaded;

        public bool LoadOnAwake;

        private void Start()
        {
            if (GameSettingDescriptor != null)
            {
                setting = GameManager.GameInstance.FindGameSetting(GameSettingDescriptor.Key);
            }
            else
            {
                setting = GameManager.GameInstance.FindGameSetting(SettingKey);
            }
            if (setting == null)
            {
                Debug.LogError($"Could not find GameSetting with Key {SettingKey}", gameObject);
            }
            if (LoadOnAwake) OnValueLoaded?.Invoke(setting.Value);
        }

        public void SetValue(float newValue)
        {
            if (setting != null)
            {
                setting.Value = newValue;
            }
#if DEBUG
            else
            {
                Debug.LogWarning("Tried to load a GameSetting before this object was initialized... D:", this);
            }
#endif
        }
    }
}