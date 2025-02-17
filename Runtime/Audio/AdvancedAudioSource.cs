using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AdvancedAudioSource : MonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }
        private float DefaultMaxDistance;
        private System.Random random;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            DefaultMaxDistance = AudioSource.maxDistance;
            random = new System.Random();
        }

        public virtual void PlayAdvancedSound(AdvancedSoundEffect sound, Transform soundParent = null)
        {

            AudioSource.Stop();
            var volume = sound.VolumeRange > 0 ? UnityEngine.Random.Range(sound.Volume - sound.VolumeRange, sound.Volume + sound.VolumeRange) : sound.Volume;
            AudioSource.volume = volume;
            var pitch = sound.PitchRange > 0 ? UnityEngine.Random.Range(sound.Pitch - sound.PitchRange, sound.Pitch + sound.PitchRange) : sound.Pitch;
            var maxDistance = sound.MaxDistance > 0 ? sound.MaxDistance : DefaultMaxDistance;
            AudioSource.maxDistance = maxDistance;
            PlaySound(RandomHelper.FromCollection(random, sound.Clips), pitch, soundParent);
        }

        public void PlayAdvancedSound(AdvancedSoundEffect sound, Vector3 position)
        {
            transform.position = position;
            PlayAdvancedSound(sound);
        }

        public virtual void PlaySound(AudioClip clip, float pitch = 1, Transform soundParent = null)
        {
            AudioSource.clip = clip;
            AudioSource.pitch = pitch;
            AudioSource.Play();
        }

        public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1)
        {
            transform.position = position;
            PlaySound(clip, pitch);
        }
    }
}
