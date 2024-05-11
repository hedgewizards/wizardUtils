using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.GlobalSounds
{
    [CreateAssetMenu(fileName = "GlobalSoundManifest", menuName = "WizardUtils/GlobalSounds/GlobalSoundManifest", order = -100)]
    public class GlobalSoundManifest : ScriptableObject, IDescriptorManifest<GlobalSoundDescriptor>
    {
        public GlobalSoundDescriptor[] Descriptors;

        public void Add(GlobalSoundDescriptor descriptor)
        {
            ArrayHelper.InsertAndResize(ref Descriptors, descriptor);
        }

        public bool Contains(GlobalSoundDescriptor descriptor)
        {
            return Descriptors.Contains(descriptor);
        }

        public void Remove(GlobalSoundDescriptor descriptor)
        {
            ArrayHelper.DeleteAndResize(ref Descriptors, descriptor);
        }
    }
}
