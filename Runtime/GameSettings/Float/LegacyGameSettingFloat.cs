using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.GameSettings
{
    public class GameSettingFloat : IGameSetting<float>
    {
        string key;
        public string Key => key;
        float defaultValue;
        private float _value;

        public event EventHandler<GameSettingChangedEventArgs<float>> OnChanged;

        public GameSettingFloat(string key, float defaultValue)
        {
            this.key = key;
            this.defaultValue = defaultValue;
            _value = defaultValue;
        }

        public float Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    OnChanged?.Invoke(this, new GameSettingChangedEventArgs<float>(_value, value));
                }
                _value = value;
            }
        }
    }
}
