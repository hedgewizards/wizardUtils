using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Pages
{
    [CreateAssetMenu(fileName = "p_", menuName = "WizardUtils/UI/PageDescriptor")]
    public class PageDescriptor : ManifestedDescriptor<PageManifest>
    {
        public string Key;
        public GameObject Prefab;

        public override string GetKey() => Key;
    }
}
