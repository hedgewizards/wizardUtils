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
    public class PointerHoldLocator : MonoBehaviour,
        IPointerDownHandler,
        IDragHandler
    {

        public UnityEvent<Vector2> OnDrag;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            TestPoint(eventData.position, eventData.pressEventCamera);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            TestPoint(eventData.position, eventData.pressEventCamera);
        }

        void TestPoint(Vector2 position, Camera camera)
        {
            RectTransform rectTransform = transform as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, camera, out Vector2 localPoint))
            {
                var localPointParametric = new Vector2()
                {
                    x = Mathf.Clamp01(localPoint.x / rectTransform.rect.width),
                    y = Mathf.Clamp01(localPoint.y / rectTransform.rect.height),
                };
                OnDrag?.Invoke(localPointParametric);
            }
        }
    }
}