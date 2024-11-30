using UnityEngine;

namespace WizardUtils.Audio
{
    public class AdvancedSoundEffect : ScriptableObject
    {
        public AudioClip[] Clips;
        public float Pitch = 1;
        public float PitchRange = 0;
        public float Volume;
        public float VolumeRange;
    }
}
