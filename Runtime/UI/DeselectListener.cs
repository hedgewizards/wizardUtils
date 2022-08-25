using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace WizardUtils.UI
{
    /// <summary>
    /// Calculates x & y values from 0 to 1 based on where over the region you are holding click on
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DeselectListener : MonoBehaviour,
        IDeselectHandler
    {
        public UnityEvent OnDeselect;

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            OnDeselect?.Invoke();
        }

    }
}