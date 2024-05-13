using System;
using System.Linq;
using UnityEngine;
using WizardUtils;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils.Configurations.ConfigSettings
{
    [CreateAssetMenu(fileName = "SettingsManifest", menuName = "Horde/Settings/SettingsManifest", order = 100)]
    public class SettingManifest : ScriptableObject, IDescriptorManifest<SettingDescriptor>
    {
        public SettingDescriptor[] Items;
        void Reset()
        {
            Items ??= new SettingDescriptor[0];
        }

        public void Add(SettingDescriptor descriptor)
        {
            ArrayHelper.InsertAndResize(ref Items, descriptor);
        }

        public bool Contains(SettingDescriptor descriptor)
        {
            return Items.Contains(descriptor);
        }

        public void Remove(SettingDescriptor descriptor)
        {
            ArrayHelper.DeleteAndResize(ref Items, descriptor);
        }

        public bool TryFindByKey(string key, out SettingDescriptor result)
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
