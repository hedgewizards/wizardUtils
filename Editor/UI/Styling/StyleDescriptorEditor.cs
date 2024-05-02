using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Styling
{
    [CustomEditor(typeof(StyleDescriptor))]
    public class StyleDescriptorEditor : Editor
    {
        public DescriptorManifestAssigner<StyleManifest, StyleDescriptor> manifestAssigner;

        public override VisualElement CreateInspectorGUI()
        {
            manifestAssigner = new DescriptorManifestAssigner<StyleManifest, StyleDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            manifestAssigner.DrawRegisterButtons(targets.Cast<StyleDescriptor>().ToArray());
        }
    }
}
