using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class QuickCreateableAttribute : PropertyAttribute
    {
        public System.Type AssetType { get; }
        public bool ListSubclasses { get; }

        public QuickCreateableAttribute(System.Type assetType, bool listSubclasses = false)
        {
            AssetType = assetType;
            ListSubclasses = listSubclasses;
        }
    }
}
