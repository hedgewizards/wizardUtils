using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class FilepathAttribute : PropertyAttribute
    {
        public enum SelectionStyles
        {
            Files = 0,
            Folders = 1,
        }
        public SelectionStyles SelectionMode;
        public bool Relative;

        public FilepathAttribute(SelectionStyles selectionStyle, bool relative = true)
        {
            SelectionMode = selectionStyle;
            Relative = relative;
        }
    }
}
