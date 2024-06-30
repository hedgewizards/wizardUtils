using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Coroutines
{
    public static class CoroutineHelpers
    {
        public static Coroutine StartDelayCoroutine(this MonoBehaviour self, float delaySeconds, Action callback)
        {
            return self.StartCoroutine(WaitForSeconds(delaySeconds, callback));
        }
        private static IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);

            callback();
        }
        public static Coroutine StartDelayCoroutineUnscaled(this MonoBehaviour self, float delaySeconds, Action callback)
        {
            return self.StartCoroutine(WaitForSecondsUnscaled(delaySeconds, callback));
        }
        private static IEnumerator WaitForSecondsUnscaled(float seconds, Action callback)
        {
            yield return new WaitForSecondsRealtime(seconds);

            callback();
        }

        public static Coroutine StartNextFrameCoroutine(this MonoBehaviour self, Action callback)
        {
            return self.StartCoroutine(WaitForNextFrame(callback));
        }

        private static IEnumerator WaitForNextFrame(Action callback)
        {
            yield return null;

            callback();
        }

        /// <summary>
        /// Call <paramref name="callback"/> every <paramref name="intervalSeconds"/> with increasing values from 0->1 over <paramref name="durationSeconds"/><br/>
        /// This will always begin with a Callback(0), and end with a Callback(1)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="durationSeconds"></param>
        /// <param name="callback"></param>
        /// <param name="useUnscaledTime"></param>
        /// <param name="intervalSeconds"></param>
        /// <returns></returns>
        public static Coroutine StartParametricCoroutine(this MonoBehaviour self,
            float durationSeconds,
            Action<float> callback,
            bool useUnscaledTime = false,
            float intervalSeconds = 0)
        {
            return self.StartCoroutine(ParametricAsync(durationSeconds, callback, useUnscaledTime, intervalSeconds));
        }

        private static IEnumerator ParametricAsync(
            float durationSeconds,
            Action<float> callback,
            bool useUnscaledTime = false,
            float waitTimeSeconds = 0)
        {
            object instruction = useUnscaledTime ? new WaitForSecondsRealtime(waitTimeSeconds)
                : new WaitForSeconds(waitTimeSeconds);
            float t = 0;
            float startTime = Time.time;

            while (t < 1)
            {
                t = Mathf.Clamp01((Time.time - startTime) / durationSeconds);
                callback(t);
                yield return instruction;
            }

            callback(1);
        }
    }
}
