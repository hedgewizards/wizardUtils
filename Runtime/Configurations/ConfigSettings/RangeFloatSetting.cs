using System;
using UnityEngine;

namespace WizardUtils.Configurations.ConfigSettings
{

    [CreateAssetMenu(fileName = "setting_", menuName = "Horde/Settings/RangeFloat")]
    public class RangeFloatSetting : SettingDescriptor
    {
        public float DefaultValue;
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;

        public bool UseSlider;

        [HideInInspector]
        public float SliderInterval;
        [HideInInspector]
        public string SliderDisplayFormat = "N0";

        public override bool Validate(out string failReason)
        {
            if (MinValue >= MaxValue)
            {
                failReason = "MaxValue must be greater than MinValue";
                return false;
            }

            return base.Validate(out failReason);
        }
    }
}
