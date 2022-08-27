﻿using UnityEngine;

namespace WizardUtils
{
    [CreateAssetMenu(fileName = "s_", menuName = "WizardUtils/Settings/GameSettingFloatDescriptor", order = 0)]
    public class GameSettingDescriptor : ScriptableObject
    {
        public string Key;
        public float DefaultValue;
    }
}