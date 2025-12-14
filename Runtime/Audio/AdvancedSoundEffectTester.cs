using System;
using System.Collections;
using UnityEngine;

namespace WizardUtils.Audio
{
    [ExecuteInEditMode]
    public class AdvancedSoundEffectTester : MonoBehaviour
    {
        private AudioSource AudioSource;

        [NonSerialized]
        private bool IsPlaying;
        [NonSerialized]
        private bool Initialized;

        private bool VolumeIsAnimated;
        private IEnumerator VolumeAnimationCoroutine;

        public void Update()
        {
            if (!IsPlaying)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if (VolumeAnimationCoroutine != null)
            {
                if (!VolumeAnimationCoroutine.MoveNext())
                {
                    VolumeAnimationCoroutine = null;
                }
            }
        }

        public void PlaySound(AdvancedSoundEffect sound)
        {
            if (IsPlaying) StopPlaying();
            if (!Initialized) Initialize();
            IsPlaying = true;

            AudioClip clip = RandomHelper.FromCollection(new System.Random(), sound.Clips);
            float volume = sound.VolumeRange > 0 ? UnityEngine.Random.Range(sound.Volume - sound.VolumeRange, sound.Volume + sound.VolumeRange) : sound.Volume;
            float pitch = sound.PitchRange > 0 ? UnityEngine.Random.Range(sound.Pitch - sound.PitchRange, sound.Pitch + sound.PitchRange) : sound.Pitch;
            float clipLength = clip.length / Mathf.Abs(pitch);

            if (VolumeIsAnimated && VolumeAnimationCoroutine != null)
            {
                VolumeAnimationCoroutine = null;
            }
            AudioSource.Stop();

            if (sound.FadeInTime > 0)
            {
                float fadeInTime = Mathf.Max(clipLength, sound.FadeInTime);
                if (sound.FadeOutTime > 0 && !AudioSource.loop)
                {
                    float fadeOutTime = Mathf.Min(sound.FadeOutTime, Mathf.Max(0, clipLength - fadeInTime));
                    float delay = (clipLength - fadeInTime - fadeOutTime);
                    VolumeAnimationCoroutine = FadeInAndOutVolumeAsync(fadeInTime, delay, fadeOutTime, volume);
                }
                else
                {
                    VolumeAnimationCoroutine = FadeInVolumeAsync(fadeInTime, volume);
                }
            }
            else
            {
                AudioSource.volume = volume;
                if (sound.FadeOutTime > 0 && !AudioSource.loop)
                {
                    float fadeOutTime = Mathf.Min(sound.FadeOutTime, clipLength);
                    float delay = (clipLength - fadeOutTime);
                    VolumeAnimationCoroutine = FadeOutVolumeAsync(delay, fadeOutTime, volume);
                }
            }

            AudioSource.spatialBlend = 0;
            AudioSource.clip = clip;
            AudioSource.pitch = pitch;
            AudioSource.Play();
        }

        private void Initialize()
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void StopPlaying()
        {
            IsPlaying = false;
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
    }
}
