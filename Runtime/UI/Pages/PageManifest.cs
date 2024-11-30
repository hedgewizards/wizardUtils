using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Pages
{
    [CreateAssetMenu(fileName = "PageManifest", menuName = "WizardUtils/UI/PageManifest", order = 100)]
    public class PageManifest : DescriptorManifest<PageDescriptor>
    {
    }
}
