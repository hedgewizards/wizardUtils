using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class QuickCreateableAttribute : PropertyAttribute
    {
        public System.Type AssetType { get; }
        public bool ListSubclasses { get; }
        public bool ShowPresets { get; }

        public QuickCreateableAttribute(System.Type assetType, bool listSubclasses = false, bool showPresets = true)
        {
            AssetType = assetType;
            ListSubclasses = listSubclasses;
            ShowPresets = showPresets;
        }

        public QuickCreateableAttribute(bool listSubclasses = false, bool showPresets = true)
        {
            ListSubclasses = listSubclasses;
            ShowPresets = showPresets;
        }
    }
}
