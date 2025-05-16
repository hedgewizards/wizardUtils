using System;
using System.Collections;
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
        private bool VolumeIsAnimated;
        private Coroutine VolumeAnimationCoroutine;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            DefaultMaxDistance = AudioSource.maxDistance;
            random = new System.Random();
        }

        public virtual void PlayAdvancedSound(AdvancedSoundEffect sound, Transform soundParent = null)
        {
            AudioClip clip = RandomHelper.FromCollection(random, sound.Clips);
            float volume = sound.VolumeRange > 0 ? UnityEngine.Random.Range(sound.Volume - sound.VolumeRange, sound.Volume + sound.VolumeRange) : sound.Volume;
            float pitch = sound.PitchRange > 0 ? UnityEngine.Random.Range(sound.Pitch - sound.PitchRange, sound.Pitch + sound.PitchRange) : sound.Pitch;
            float clipLength = clip.length / Mathf.Abs(pitch);
            float maxDistance = sound.MaxDistance > 0 ? sound.MaxDistance : DefaultMaxDistance;

            if (VolumeIsAnimated && VolumeAnimationCoroutine != null)
            {
                StopCoroutine(VolumeAnimationCoroutine);
            }
            AudioSource.Stop();

            if (sound.FadeInTime > 0)
            {
                float fadeInTime = Mathf.Max(clipLength, sound.FadeInTime);
                if (sound.FadeOutTime > 0 && !AudioSource.loop)
                {
                    float fadeOutTime = Mathf.Min(sound.FadeOutTime, Mathf.Max(0, clipLength - fadeInTime));
                    float delay = (clipLength - fadeInTime - fadeOutTime);
                    VolumeAnimationCoroutine = StartCoroutine(FadeInAndOutVolumeAsync(fadeInTime, delay, fadeOutTime, volume));
                }
                else
                {
                    VolumeAnimationCoroutine = StartCoroutine(FadeInVolumeAsync(fadeInTime, volume));
                }
            }
            else
            {
                AudioSource.volume = volume;
                if (sound.FadeOutTime > 0 && !AudioSource.loop)
                {
                    float fadeOutTime = Mathf.Min(sound.FadeOutTime, clipLength);
                    float delay = (clipLength - fadeOutTime);
                    VolumeAnimationCoroutine = StartCoroutine(FadeOutVolumeAsync(delay, fadeOutTime, volume));
                }
            }

            AudioSource.maxDistance = maxDistance;
            PlaySound(clip, pitch, soundParent);
        }

        /// <summary>
        /// Stop playing a loop.
        /// </summary>
        /// <param name="sound"></param>
        /// <returns>0 if instant, otherwise time in seconds until it fades out</returns>
        public virtual float StopLoop(AdvancedSoundEffect sound)
        {
            if (sound.FadeOutTime > 0)
            {
                StartCoroutine(FadeOutAndStopAsync(sound.FadeOutTime));
                return sound.FadeOutTime;
            }
            else
            {
                AudioSource.Stop();
                return 0;
            }
        }

        #region Volume Fading

        private IEnumerator FadeInAndOutVolumeAsync(float fadeInDuration, float delay, float fadeOutDuration, float normalVolume)
        {
            VolumeIsAnimated = true;
            IEnumerator enumerator = InternalLerpVolumeAsync(fadeInDuration, 0, normalVolume);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            enumerator = InternalLerpVolumeAsync(fadeOutDuration, normalVolume, 0);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            VolumeIsAnimated = false;
        }

        private IEnumerator FadeInVolumeAsync(float duration, float normalVolume)
        {
            VolumeIsAnimated = true;
            IEnumerator enumerator = InternalLerpVolumeAsync(duration, 0, normalVolume);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            VolumeIsAnimated = false;
        }

        private IEnumerator FadeOutVolumeAsync(float delay, float duration, float normalVolume)
        {
            VolumeIsAnimated = true;
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            IEnumerator enumerator = InternalLerpVolumeAsync(duration, normalVolume, 0);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            VolumeIsAnimated = false;
        }

        private IEnumerator FadeOutAndStopAsync(float duration)
        {
            float normalVolume = AudioSource.volume;
            VolumeIsAnimated = true;
            IEnumerator enumerator = InternalLerpVolumeAsync(duration, normalVolume, 0);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            VolumeIsAnimated = false;
            AudioSource.Stop();
        }


        private IEnumerator InternalLerpVolumeAsync(float duration, float initialVolume, float finalVolume)
        {
            float startTime = Time.time;
            float t = 0;
            while (t < 1)
            {
                t = (Time.time - startTime) / duration;
                AudioSource.volume = Mathf.Lerp(initialVolume, finalVolume, t);
                yield return null;
            }
            AudioSource.volume = finalVolume;
        }

        #endregion

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
