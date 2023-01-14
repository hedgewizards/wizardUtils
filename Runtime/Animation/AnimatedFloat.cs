using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Animations
{
    public class AnimatedFloat
    {
        public MonoBehaviour Parent;
        public float CurrentValue {get; private set; }
        public float TargetValue { get; private set; }

        public float PositiveEdgeTransitionRate {get; private set; }
        public float NegativeEdgeTransitionRate { get; private set; }
        public UnityEvent OnValueChanged;
        private Coroutine cachedCoroutine;

        public AnimatedFloat(MonoBehaviour parent, float initialValue, float transitionSpeed) : this(parent, initialValue, transitionSpeed, transitionSpeed) { }

        public AnimatedFloat(MonoBehaviour parent, float initialValue, float positiveEdgeTransitionSpeed, float negativeEdgeTransitionSpeed)
        {
            Parent = parent;
            CurrentValue = initialValue;
            PositiveEdgeTransitionRate = positiveEdgeTransitionSpeed;
            NegativeEdgeTransitionRate = negativeEdgeTransitionSpeed;

            OnValueChanged = new UnityEvent();
        }

        public void SetValueInstantly(float newValue)
        {
            CurrentValue = newValue;
            TargetValue = newValue;
            if (cachedCoroutine != null)
            {
                Parent.StopCoroutine(cachedCoroutine);
                cachedCoroutine = null;
            }
            OnValueChanged?.Invoke();
        }

        public void InterpolateTo(float newValue)
        {
            TargetValue = newValue;
            if (cachedCoroutine == null)
            {
                cachedCoroutine = Parent.StartCoroutine(Interpolate());
            }
        }

        IEnumerator Interpolate()
        {
            while (CurrentValue != TargetValue)
            {
                var transitionRate = CurrentValue < TargetValue ? PositiveEdgeTransitionRate : NegativeEdgeTransitionRate;
                CurrentValue = Mathf.MoveTowards(CurrentValue, TargetValue, Time.deltaTime * transitionRate);
                OnValueChanged?.Invoke();
                yield return null;
            }
            cachedCoroutine = null;
        }
    }
}
