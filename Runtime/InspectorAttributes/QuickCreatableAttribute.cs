using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class QuickCreatableAttribute : PropertyAttribute
    {
        public System.Type AssetType { get; }
        public bool ListSubclasses { get; }
        public bool ShowPresets { get; }

        public QuickCreatableAttribute(System.Type assetType, bool listSubclasses = false, bool showPresets = true)
        {
            AssetType = assetType;
            ListSubclasses = listSubclasses;
            ShowPresets = showPresets;
        }

        public QuickCreatableAttribute(bool listSubclasses = false, bool showPresets = true)
        {
            ListSubclasses = listSubclasses;
            ShowPresets = showPresets;
        }
    }
}
