using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Styling
{
    [CreateAssetMenu(fileName = "StyleManifest", menuName = "WizardUtils/UI/StyleManifest", order = 100)]
    public class StyleManifest : DescriptorManifest<StyleDescriptor>
    {
    }
}
