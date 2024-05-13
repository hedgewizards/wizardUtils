using UnityEngine;

namespace WizardUtils.Configurations.MenuSettings
{
    public abstract class SettingDescriptor : ScriptableObject
    {
        public string Key;
        public string DisplayName;

        public virtual bool Validate(out string failReason)
        {
            failReason = default;
            return true;
        }
    }
}
