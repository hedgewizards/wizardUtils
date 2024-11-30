using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Configurations.ConfigSettings
{
    public abstract class SettingDescriptor : ManifestedDescriptor<SettingManifest>
    {
        public string Key;
        public string DisplayName;
        public bool NoWriteToConfig;

        public override string GetKey() => Key;

        public virtual bool Validate(out string failReason)
        {
            failReason = default;
            return true;
        }
    }
}
