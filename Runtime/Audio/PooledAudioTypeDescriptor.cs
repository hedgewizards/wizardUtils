using System;
using UnityEngine;

namespace WizardUtils.Audio
{
    [CreateAssetMenu(fileName = "audiosourcetype_", menuName = "WizardUtils/Audio/PooledAudioType Descriptor")]
    public class PooledAudioTypeDescriptor : ScriptableObject
    {
        [NonSerialized]
        public int Id;
        public GameObject Prefab;
        public int PoolSize;
    }
}
