using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.InspectorAttributes
{
    public class QuickCreatableOrderAttribute : Attribute
    {
        public string Path;
        public string DisplayName;

        public QuickCreatableOrderAttribute(string path = null, string displayName = null)
        {
            Path = path;
            DisplayName = displayName;
        }
    }
}
