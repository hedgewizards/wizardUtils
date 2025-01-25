using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI
{
    public class ManualHorizontalLayout : MonoBehaviour
    {
        public float Spacing;
        public float Padding;
        public bool AutoResizeSelf;

        public void Recalculate()
        {
            float totalWidthNoPadding = 0;
            RectTransform[] rectTransforms = new RectTransform[transform.childCount];
            for (int n = 0; n < transform.childCount; n++)
            {
                rectTransforms[n] = transform.GetChild(n).GetComponent<RectTransform>();
                totalWidthNoPadding += rectTransforms[n].rect.width;
            }

            float runningX = Padding;
            for (int n = 0; n < rectTransforms.Length; n++)
            {
                rectTransforms[n].anchoredPosition = new Vector2(runningX, 0);
                runningX += rectTransforms[n].rect.width;
                if (n < rectTransforms.Length - 1)
                {
                    runningX += Spacing;
                }
            }

            if (AutoResizeSelf)
            {
                runningX += Padding;
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(runningX, rectTransform.sizeDelta.y);
            }
        }
    }
}
