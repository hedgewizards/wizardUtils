using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour
    {
        private AudioSource baseAudioSource;
        public AudioClip[] Clips;
        /// <summary>
        /// How long until PlayGlobalSound() will produce another sound
        /// </summary>
        public float ReplayDelay;

        public bool RandomizePitch;
        public float RandomizePitchMinimum = 1;
        public float RandomizePitchMaximum = 1;

        private void Awake()
        {
            baseAudioSource = GetComponent<AudioSource>();
        }

        float lastPlay = float.MinValue;
        public void Play()
        {
            if (CanPlay)
            {
                lastPlay = Time.time;
                int chosenSoundIndex = Random.Range(0, Clips.Length);
                if (RandomizePitch)
                {
                    baseAudioSource.pitch = RandomPitch();
                }

                baseAudioSource.PlayOneShot(Clips[chosenSoundIndex]);
            }
        }

        public void PlayStoredClip()
        {
            if (CanPlay)
            {
                lastPlay = Time.time;
                if (RandomizePitch)
                {
                    baseAudioSource.pitch = RandomPitch();
                }

                baseAudioSource.Play();
            }
        }

        public float Volume
        {
            get => baseAudioSource.volume;
            set => baseAudioSource.volume = value;
        }

        public float Pitch
        {
            get => baseAudioSource.pitch;
            set => baseAudioSource.pitch = value;
        }

        private float RandomPitch()
        {
            return Random.Range(RandomizePitchMinimum, RandomizePitchMaximum);
        }

        public void PlayOneShot(AudioClip clip, float volume)
        {
            if (CanPlay)
            {
                PlayOneShotIgnoreDelay(clip, volume);
            }
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (CanPlay)
            {
                PlayOneShotIgnoreDelay(clip);
            }
        }

        public void PlayOneShotIgnoreDelay(AudioClip clip) => PlayOneShotIgnoreDelay(clip, 1);
        public void PlayOneShotIgnoreDelay(AudioClip clip, float volume)
        {
            if (RandomizePitch)
            {
                baseAudioSource.pitch = RandomPitch();
            }

            lastPlay = Time.time;
            baseAudioSource.PlayOneShot(clip, volume);
        }

        private bool CanPlay => lastPlay + ReplayDelay < Time.time;
    }
}