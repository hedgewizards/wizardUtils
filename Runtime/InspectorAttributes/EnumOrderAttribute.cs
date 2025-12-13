using System;

namespace WizardUtils.InspectorAttributes
{
    public class EnumOrderAttribute : Attribute
    {
        public string Path;
        public EnumOrderAttribute(string path)
        {
            Path = path;
        }
    }
}