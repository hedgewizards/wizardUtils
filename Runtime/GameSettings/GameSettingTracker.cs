using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WizardUtils.GameSettings;

namespace WizardUtils
{
    public class GameSettingTracker : MonoBehaviour
    {
        public GameSettingChangedEvent OnSettingChanged;
        GameSettingFloat gameSetting;
        public string SettingName;

        private void Start()
        {
            gameSetting = new GameSettingFloat(GameManager.Instance.Configuration, SettingName, 0f);
            gameSetting.OnChanged += onGameSettingChanged;
        }

        private void onGameSettingChanged(object sender, GameSettingChangedEventArgs<float> e)
        {
            OnSettingChanged.Invoke(e.FinalValue);
        }
    }

    [System.Serializable]
    public class GameSettingChangedEvent : UnityEvent<float>
    {
    }
}