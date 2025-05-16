using UnityEngine;

namespace WizardUtils.Audio
{
    [CreateAssetMenu(fileName = "newsound", menuName = "WizardUtils/Audio/Advanced Sound Effect")]
    public class AdvancedSoundEffect : ScriptableObject
    {
        public PooledAudioTypeDescriptor AudioType;
        public AudioClip[] Clips;
        public float Pitch = 1;
        public float PitchRange = 0;
        public float Volume = 1;
        public float VolumeRange;
        [Tooltip("If >0, the max range this audio should be heard from. otherwise use the default value")]
        public float MaxDistance = -1;
        [Tooltip("If >0, fade in volume from zero when playing the sound")]
        public float FadeInTime = 0;
        [Tooltip("if >0, fade out volume from zero when the sound ends")]
        public float FadeOutTime = 0;
    }
}
