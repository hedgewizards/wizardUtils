using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Audio.GlobalSounds
{
    [CreateAssetMenu(fileName = "GlobalSoundManifest", menuName = "WizardUtils/Audio/GlobalSounds/GlobalSoundManifest", order = 100)]
    public class GlobalSoundManifest : DescriptorManifest<GlobalSoundDescriptor>
    {
    }
}
