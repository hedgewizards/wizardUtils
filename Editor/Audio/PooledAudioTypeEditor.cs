using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;
using WizardUtils.Audio;
using WizardUtils.ManifestPattern;

namespace Horde.Items
{
    [CustomEditor(typeof(PooledAudioTypeDescriptor), true)]
    public class PooledAudioTypeEditor : Editor
    {
        public DescriptorManifestAssigner<PooledAudioTypeManifest, PooledAudioTypeDescriptor> manifestAssigner;

        public override VisualElement CreateInspectorGUI()
        {
            manifestAssigner = new DescriptorManifestAssigner<PooledAudioTypeManifest, PooledAudioTypeDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            manifestAssigner.DrawRegisterButtons(targets.Cast<PooledAudioTypeDescriptor>().ToArray());
        }
    }
}
