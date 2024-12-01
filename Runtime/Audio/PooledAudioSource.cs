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
    public class PooledAudioSource : MonoBehaviour
    {
        private AudioSource AudioSource;
        private float DefaultMaxDistance;
        private Coroutine PlayCoroutine;
        private System.Random random;

        public event EventHandler OnFree;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            DefaultMaxDistance = AudioSource.maxDistance;
            random = new System.Random();
        }

        public void PlayAdvancedSound(AdvancedSoundEffect sound, Vector3 position)
        {
            transform.position = position;
            PlayAdvancedSound(sound);
        }

        public void PlayAdvancedSound(AdvancedSoundEffect sound, Transform soundParent = null)
        {
            if (PlayCoroutine != null)
            {
                AudioSource.Stop();
                StopCoroutine(PlayCoroutine);
            }

            var volume = sound.VolumeRange > 0 ? UnityEngine.Random.Range(sound.Volume - sound.VolumeRange, sound.Volume + sound.VolumeRange) : sound.Volume;
            AudioSource.volume = volume;
            var pitch = sound.PitchRange > 0 ? UnityEngine.Random.Range(sound.Pitch - sound.PitchRange, sound.Pitch + sound.PitchRange) : sound.Volume;
            var maxDistance = sound.MaxDistance > 0 ? sound.MaxDistance : DefaultMaxDistance;
            AudioSource.maxDistance = maxDistance;
            PlaySound(RandomHelper.FromCollection(random, sound.Clips), pitch, soundParent);
        }

        public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1)
        {
            transform.position = position;
            PlaySound(clip, pitch);
        }

        public void PlaySound(AudioClip clip, float pitch = 1, Transform soundParent = null)
        {
            AudioSource.clip = clip;
            AudioSource.pitch = pitch;
            AudioSource.Play();
            if (soundParent != null)
            {
                PlayCoroutine = StartCoroutine(FollowParentThenFreeAsync(clip.length / Mathf.Abs(pitch), soundParent));
            }
            else
            {
                PlayCoroutine = StartCoroutine(WaitThenFreeAsync(clip.length / Mathf.Abs(pitch)));
            }
        }

        private void Free()
        {
            OnFree?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator FollowParentThenFreeAsync(float durationSeconds, Transform soundParent)
        {
            var startTime = Time.time;
            while (Time.time < startTime + durationSeconds)
            {
                if (soundParent == null)
                {
                    yield return new WaitForSeconds((startTime + durationSeconds) - Time.time);
                    break;
                }
                transform.position = soundParent.position;
                yield return null;
            }

            AudioSource.Stop();
            Free();
        }

        private IEnumerator WaitThenFreeAsync(float durationSeconds)
        {
            yield return new WaitForSeconds(durationSeconds);
            AudioSource.Stop();
            Free();
        }
    }
}
