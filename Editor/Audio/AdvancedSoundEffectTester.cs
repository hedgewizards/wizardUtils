using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Audio
{
    [ExecuteInEditMode]
    public class AdvancedSoundEffectTester
    {
        private AudioSource AudioSource;
        private AdvancedSoundEffect LastSound;

        private bool isPlaying;

        private bool VolumeIsAnimated;
        private EditorCoroutine VolumeAnimationCoroutine;

        public AdvancedSoundEffectTester()
        {
            GameObject playObject = new GameObject();
            playObject.hideFlags = HideFlags.HideAndDontSave;
            AudioSource = playObject.AddComponent<AudioSource>();
        }

        public bool IsPlaying => isPlaying;

        public void PlaySound(AdvancedSoundEffect sound)
        {
            if (isPlaying) StopPlaying();
            isPlaying = true;
            LastSound = sound;

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
                    VolumeAnimationCoroutine = EditorCoroutineUtility.StartCoroutine(FadeInAndOutVolumeAsync(fadeInTime, delay, fadeOutTime, volume), AudioSource);
                }
                else
                {
                    VolumeAnimationCoroutine = EditorCoroutineUtility.StartCoroutine(FadeInVolumeAsync(fadeInTime, volume), AudioSource);
                }
            }
            else
            {
                AudioSource.volume = volume;
                if (sound.FadeOutTime > 0 && !AudioSource.loop)
                {
                    float fadeOutTime = Mathf.Min(sound.FadeOutTime, clipLength);
                    float delay = (clipLength - fadeOutTime);
                    VolumeAnimationCoroutine = EditorCoroutineUtility.StartCoroutine(FadeOutVolumeAsync(delay, fadeOutTime, volume), AudioSource);
                }
            }

            AudioSource.spatialBlend = 0;
            AudioSource.clip = clip;
            AudioSource.pitch = pitch;
            AudioSource.Play();
        }

        public void StopPlaying(bool hard = true)
        {
            if (!hard && isPlaying && LastSound != null && LastSound.FadeOutTime > 0)
            {
                VolumeAnimationCoroutine = EditorCoroutineUtility.StartCoroutine(FadeOutVolumeAsync(0, LastSound.FadeOutTime, AudioSource.volume), this);
            }
            else
            {
                isPlaying = false;
                AudioSource.Stop();
                if (VolumeAnimationCoroutine != null)
                {
                    EditorCoroutineUtility.StopCoroutine(VolumeAnimationCoroutine);
                    VolumeIsAnimated = false;
                }
                return;
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
                yield return new EditorWaitForSeconds(delay);
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
                yield return new EditorWaitForSeconds(delay);
            }
            IEnumerator enumerator = InternalLerpVolumeAsync(duration, normalVolume, 0);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            VolumeIsAnimated = false;
        }

        private IEnumerator InternalLerpVolumeAsync(float duration, float initialVolume, float finalVolume)
        {
            double startTime = EditorApplication.timeSinceStartup;
            float t = 0;
            while (t < 1)
            {
                t = (float)((EditorApplication.timeSinceStartup - startTime) / duration);
                AudioSource.volume = Mathf.Lerp(initialVolume, finalVolume, t);
                yield return new EditorWaitForSeconds(0.01f);
            }
            AudioSource.volume = finalVolume;
        }

        #endregion
    }
}
