using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Pages
{
    [CustomEditor(typeof(PageDescriptor))]
    public class PageDescriptorEditor : Editor
    {
        public DescriptorManifestAssigner<PageManifest, PageDescriptor> manifestAssigner;

        public override VisualElement CreateInspectorGUI()
        {
            manifestAssigner = new DescriptorManifestAssigner<PageManifest, PageDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            manifestAssigner.DrawRegisterButtons(targets.Cast<PageDescriptor>().ToArray());
        }
    }
}
