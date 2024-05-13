using System;
using UnityEngine;

namespace WizardUtils.Configurations.Settings
{

    [CreateAssetMenu(fileName = "setting_", menuName = "Horde/Settings/Bool")]
    public class BoolSetting : SettingDescriptor
    {
        public bool DefaultValue;
    }
}
