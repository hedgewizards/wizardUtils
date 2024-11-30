using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip[] Clips;
        /// <summary>
        /// How long until Play() will produce another sound
        /// </summary>
        public float ReplayDelay;

        public bool RandomizePitch;
        public float RandomizePitchMinimum = 1;
        public float RandomizePitchMaximum = 1;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
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
                    audioSource.pitch = RandomPitch();
                }

                audioSource.PlayOneShot(Clips[chosenSoundIndex]);
            }
        }

        public void PlayStoredClip()
        {
            if (CanPlay)
            {
                lastPlay = Time.time;
                if (RandomizePitch)
                {
                    audioSource.pitch = RandomPitch();
                }

                audioSource.Play();
            }
        }

        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }

        public float Pitch
        {
            get => audioSource.pitch;
            set => audioSource.pitch = value;
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
                audioSource.pitch = RandomPitch();
            }

            lastPlay = Time.time;
            audioSource.PlayOneShot(clip, volume);
        }

        private bool CanPlay => lastPlay + ReplayDelay < Time.time;
    }
}