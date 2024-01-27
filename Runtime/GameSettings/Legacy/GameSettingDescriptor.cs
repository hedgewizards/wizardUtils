using System;
using UnityEngine;

namespace WizardUtils
{
    [CreateAssetMenu(fileName = "s_", menuName = "WizardUtils/GameSetting/Descriptor", order = 0)]
    [Obsolete("We do this through code now")]
    public class GameSettingDescriptor : ScriptableObject
    {
        public string Key;
        public float DefaultValue;
    }
}