using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.Coroutines;

namespace WizardUtils.UI.Pages
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingPage : Page
    {
        private CanvasGroup CanvasGroup;
        public float FadeTimeSeconds;
        private Coroutine FadeCoroutine;

        public override float AppearDurationSeconds => FadeTimeSeconds;

        public override float DisappearDurationSeconds => FadeTimeSeconds;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Appear(bool instant)
        {
            if (FadeCoroutine != null) StopCoroutine(FadeCoroutine);
            if (instant)
            {
                gameObject.SetActive(true);
                CanvasGroup.alpha = 1;
                CanvasGroup.interactable = true;
                return;
            }

            float startFade = CanvasGroup.alpha;
            CanvasGroup.interactable = false;
            FadeCoroutine = CoroutineHelpers.StartParametricCoroutine(this, FadeTimeSeconds, (t) => ParametricFade(startFade, 1, t),
                useUnscaledTime: true);
        }


        public override void Disappear(bool instant)
        {
            if (FadeCoroutine != null) StopCoroutine(FadeCoroutine);
            if (instant)
            {
                gameObject.SetActive(false);
                CanvasGroup.alpha = 0;
                return;
            }

            float startFade = CanvasGroup.alpha;
            FadeCoroutine = CoroutineHelpers.StartParametricCoroutine(this, FadeTimeSeconds, (t) => ParametricFade(startFade, 0, t),
                useUnscaledTime: true);
        }

        private void ParametricFade(float startFade, float endFade, float t)
        {
            CanvasGroup.alpha = Mathf.Lerp(startFade, endFade, t);
            if (t == 1)
            {
                gameObject.SetActive(endFade != 0);
                CanvasGroup.interactable = endFade != 0;
            }
        }
    }
}
