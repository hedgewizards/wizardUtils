using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace WizardUtils.Animation
{
    public class SimpleAnimator : MonoBehaviour
    {
        #region Constants
        public AnimationClip[] Clips;
        public float TransitionDuration;
        private float TransitionRate => 1 / TransitionDuration;
        #endregion

        #region Components
        private Animator MainAnimator;
        private PlayableGraph MainGraph;
        private AnimationMixerPlayable MainMixer;
        private AnimationPlayableOutput MainOutput;
        #endregion

        #region Variables
        private Coroutine TransitionCoroutine;
        #endregion

        public void Awake()
        {
            MainAnimator = GetComponent<Animator>();
            if (MainAnimator == null)
            {
                MainAnimator = gameObject.AddComponent<Animator>();
            }
            MainGraph = PlayableGraph.Create($"{name} SimpleAnimator");
            MainOutput = AnimationPlayableOutput.Create(
                MainGraph,
                "State Machine",
                MainAnimator);

            MainAnimator.runtimeAnimatorController = null;
            MainMixer = AnimationMixerPlayable.Create(MainGraph, Clips.Length);
            MainOutput.SetSourcePlayable(MainMixer);

            for (int n = 0; n < Clips.Length; n++)
            {
                var playable = AnimationClipPlayable.Create(MainGraph, Clips[n]);
                MainMixer.ConnectInput(n, playable, 0);
                MainMixer.SetInputWeight(n, 0);
            }

            MainGraph.Play();
        }

        private void OnEnable()
        {
            MainMixer.SetInputWeight(0, 1);
            for (int n = 1; n < Clips.Length; n++)
            {
                MainMixer.SetInputWeight(n, 0);
            }
        }

        private void OnDisable()
        {
            if (TransitionCoroutine != null)
            {
                StopCoroutine(TransitionCoroutine);
            }
        }

        private void OnDestroy()
        {
            if (MainGraph.IsValid())
            {
                MainGraph.Destroy();
            }
        }

        public void SetState(int stateId)
        {
            if (stateId < 0 || stateId > Clips.Length)
            {
                return;
            }

            if (TransitionCoroutine != null)
            {
                StopCoroutine(TransitionCoroutine);
            }

            if (gameObject.activeInHierarchy)
            {
                TransitionCoroutine = StartCoroutine(TransitionStateAsync(stateId));
            }
            else
            {
                for (int n = 0; n < Clips.Length; n++)
                {
                    MainMixer.SetInputWeight(n, n == stateId ? 1 : 0);
                }
            }
        }

        private IEnumerator TransitionStateAsync(int targetStateId)
        {
            if (MainMixer.GetInputWeight(targetStateId) <= 0)
            {
                MainMixer.GetInput(targetStateId).SetTime(0);
            }

            while (true)
            {
                yield return null;

                bool waiting = false;
                float deltaWeight = Time.deltaTime * TransitionRate;
                for (int n = 0; n < Clips.Length; n++)
                {
                    float oldWeight = MainMixer.GetInputWeight(n);
                    float targetWeight = n == targetStateId ? 1 : 0;
                    if (oldWeight == targetWeight)
                    {
                        continue;
                    }

                    float newWeight = Mathf.MoveTowards(oldWeight, targetWeight, deltaWeight);
                    MainMixer.SetInputWeight(n, newWeight);
                    if (newWeight != targetWeight)
                    {
                        waiting = true;
                    }
                }

                if (!waiting)
                {
                    yield break;
                }
            }
        }
    }
}
