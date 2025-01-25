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
            int itemCount = 0;
            RectTransform[] rectTransforms = new RectTransform[transform.childCount];
            for (int n = 0; n < transform.childCount; n++)
            {
                rectTransforms[n] = transform.GetChild(n).GetComponent<RectTransform>();
                if (!rectTransforms[n].gameObject.activeSelf) continue;
                itemCount++;
                totalWidthNoPadding += rectTransforms[n].rect.width;
            }

            float runningX = Padding;
            float runningCount = 0;
            for (int n = 0; n < rectTransforms.Length; n++)
            {
                if (!rectTransforms[n].gameObject.activeSelf) continue;
                rectTransforms[n].anchoredPosition = new Vector2(runningX, 0);
                runningX += rectTransforms[n].rect.width;
                if (runningCount < itemCount - 1)
                {
                    runningX += Spacing;
                }
                runningCount++;
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
