using UnityEngine;

namespace WizardUtils.Configurations.ConfigSettings
{
    public abstract class SettingDescriptor : ScriptableObject
    {
        public string Key;
        public string DisplayName;
        public bool NoWriteToConfig;

        public virtual bool Validate(out string failReason)
        {
            failReason = default;
            return true;
        }
    }
}
