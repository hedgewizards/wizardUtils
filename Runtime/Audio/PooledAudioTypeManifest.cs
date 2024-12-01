using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Audio
{
    [CreateAssetMenu(fileName = "audiotypemanifest", menuName = "WizardUtils/Audio/PooledAudioType GlobalSoundManifest")]
    public class PooledAudioTypeManifest : DescriptorManifest<PooledAudioTypeDescriptor>
    {
        public PooledAudioTypeDescriptor DefaultAudioType;
    }
}
