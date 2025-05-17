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
    public class PooledAdvancedAudioSource : AdvancedAudioSource
    {
        private Coroutine PlayCoroutine;

        public event EventHandler OnFree;

        private void Free()
        {
            OnFree?.Invoke(this, EventArgs.Empty);
        }

        public override void PlayAdvancedSound(AdvancedSoundEffect sound, Transform soundParent = null)
        {
            if (PlayCoroutine != null)
            {
                StopCoroutine(PlayCoroutine);
            }

            base.PlayAdvancedSound(sound, soundParent);
        }

        public override float StopLoop(AdvancedSoundEffect sound)
        {
            float time = base.StopLoop(sound);
            if (time > 0)
            {
                StartCoroutine(WaitThenFreeAsync(time));
            }
            return time;
        }

        public override void PlaySound(AudioClip clip, float pitch = 1, Transform soundParent = null)
        {
            base.PlaySound(clip, pitch, soundParent);
            if (!AudioSource.loop)
            {
                if (soundParent != null)
                {
                    PlayCoroutine = StartCoroutine(FollowParentThenFreeAsync(clip.length / Mathf.Abs(pitch), soundParent));
                }
                else
                {
                    PlayCoroutine = StartCoroutine(WaitThenFreeAsync(clip.length / Mathf.Abs(pitch)));
                }
            }
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
