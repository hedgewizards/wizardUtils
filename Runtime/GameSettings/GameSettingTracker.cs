using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils
{
    public class GameSettingTracker : MonoBehaviour
    {
        [HideInInspector]
        public GameSettingDescriptor GameSettingDescriptor;
        [HideInInspector]
        public string SettingKey;

        public UnityEvent<float> OnSettingChanged;
        GameSettingFloat gameSetting;

        public bool CallEventOnAwake;

        string DisambiguatedKey => GameSettingDescriptor != null ? GameSettingDescriptor.Key : SettingKey;

        private void Start()
        {
            gameSetting = GameManager.GameInstance.FindGameSetting(DisambiguatedKey);
            gameSetting.OnChanged += onGameSettingChanged;
            if (CallEventOnAwake)
            {
                OnSettingChanged?.Invoke(gameSetting.Value);
            }
        }

        private void onGameSettingChanged(object sender, GameSettingChangedEventArgs e)
        {
            OnSettingChanged.Invoke(e.FinalValue);
        }
    }
}