using System;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Audio
{
    [CreateAssetMenu(fileName = "audiosourcetype_", menuName = "WizardUtils/Audio/PooledAudioType Descriptor")]
    public class PooledAudioTypeDescriptor : ManifestedDescriptor<PooledAudioTypeManifest>
    {
        [NonSerialized]
        public int Id;
        public GameObject Prefab;
        public int PoolSize;
    }
}
