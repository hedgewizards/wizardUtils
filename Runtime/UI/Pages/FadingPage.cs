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
            if (CanvasGroup == null) CanvasGroup = GetComponent<CanvasGroup>();
            if (FadeCoroutine != null) StopCoroutine(FadeCoroutine);
            if (instant || AppearDurationSeconds == 0)
            {
                gameObject.SetActive(true);
                CanvasGroup.alpha = 1;
                CanvasGroup.blocksRaycasts = true;
                return;
            }

            float startFade = CanvasGroup.alpha;
            gameObject.SetActive(true);
            CanvasGroup.blocksRaycasts = false;
            FadeCoroutine = CoroutineHelpers.StartParametricCoroutine(this, FadeTimeSeconds, (t) => ParametricFade(startFade, 1, t),
                useUnscaledTime: true);
        }


        public override void Disappear(bool instant)
        {
            if (FadeCoroutine != null) StopCoroutine(FadeCoroutine);
            if (instant || DisappearDurationSeconds == 0)
            {
                gameObject.SetActive(false);
                CanvasGroup.alpha = 0;
                return;
            }

            float startFade = CanvasGroup.alpha;
            gameObject.SetActive(true);
            CanvasGroup.blocksRaycasts = false;
            FadeCoroutine = CoroutineHelpers.StartParametricCoroutine(this, FadeTimeSeconds, (t) => ParametricFade(startFade, 0, t),
                useUnscaledTime: true);
        }

        private void ParametricFade(float startFade, float endFade, float t)
        {
            CanvasGroup.alpha = Mathf.Lerp(startFade, endFade, t);
            if (t == 1)
            {
                CanvasGroup.blocksRaycasts = true;
                gameObject.SetActive(endFade != 0);
            }
        }
    }
}
