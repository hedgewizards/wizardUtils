using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;

namespace WizardUtils.GlobalSounds
{
    [CustomEditor(typeof(GlobalSoundDescriptor))]
    public class GlobalSoundDescriptorEditor : Editor
    {
        public DescriptorManifestAssigner<GlobalSoundManifest,GlobalSoundDescriptor> manifestAssigner;

        public override VisualElement CreateInspectorGUI()
        {
            manifestAssigner = new DescriptorManifestAssigner<GlobalSoundManifest, GlobalSoundDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            manifestAssigner.DrawRegisterButtons(targets.Cast<GlobalSoundDescriptor>().ToArray());
        }
    }
}
