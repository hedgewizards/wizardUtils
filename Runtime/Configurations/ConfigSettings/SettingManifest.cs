using System;
using System.Linq;
using UnityEngine;
using WizardUtils;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils.Configurations.ConfigSettings
{
    [CreateAssetMenu(fileName = "SettingsManifest", menuName = "Horde/Settings/SettingsManifest", order = 100)]
    public class SettingManifest : DescriptorManifest<SettingDescriptor>
    {
        public override bool TryFindByKey(string key, out SettingDescriptor result)
        {
            foreach(var item in Items)
            {
                if (string.Equals(item.Key, key, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
