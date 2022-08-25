using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class ColorPickerController : MonoBehaviour
    {
        [SerializeField]
        Color CurrentColor = Color.white;
        float CurrentSaturation
        {
            get
            {
                Color.RGBToHSV(CurrentColor, out _, out float S, out _);
                return S;
            }
        }

        public UnityEvent<Color> OnColorChanged;
        public PointerHoldLocator PointerLocator;
        public DeselectListener DeselectListener;
        public Image Dot;
        ToggleableUIElement pickerMenu;

        [Range(0,1)]
        public float LeftPadding;
        [Range(0,1)]
        public float RightPadding;
        [Range(0, 1)]
        public float TopPadding;
        [Range(0, 1)]
        public float BottomPadding;

        private void Awake()
        {
            PointerLocator.OnDrag.AddListener(OnPointerLocated);
            DeselectListener.OnDeselect.AddListener(Close);
            pickerMenu = GetComponent<ToggleableUIElement>();
        }

        public void Open()
        {
            pickerMenu.SetOpen(true);
            EventSystem.current.SetSelectedGameObject(PointerLocator.gameObject);
            //StartCoroutine(DelayedSetSelectionToPicker());
        }

        IEnumerator DelayedSetSelectionToPicker()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(PointerLocator.gameObject);
        }

        public void Close()
        {
            pickerMenu.SetOpen(false);
        }

        public void PickColor(Color color)
        {
            CurrentColor = color;
            OnColorChanged?.Invoke(color);
            UpdateDot();
        }

        private void OnValidate()
        {
            OnColorChanged?.Invoke(CurrentColor);
            UpdateDot();
        }

        private void UpdateDot()
        {
            if (Dot != null)
            {
                float minColorComponent = Mathf.Min(CurrentColor.r, CurrentColor.g, CurrentColor.b);
                Dot.color = minColorComponent > 0.5f ? Color.black : Color.white;

                var parent = Dot.rectTransform.parent as RectTransform;
                var width = parent.rect.width;
                var height = parent.rect.height;

                Vector2 parametricPoint = CalculateColorSpaceDotPoint();

                Dot.rectTransform.localPosition = new Vector3()
                {
                    x = parametricPoint.x * width,
                    y = parametricPoint.y * height,
                    z = Dot.rectTransform.localPosition.z
                };
            }
        }

        private Vector2 CalculateColorSpaceDotPoint()
        {
            Color.RGBToHSV(CurrentColor, out float H, out _, out float V);
            float x = Mathf.InverseLerp(LeftPadding, 1 - RightPadding, H);
            float y = Mathf.InverseLerp(BottomPadding, 1 - TopPadding, V);

            return new Vector2(x, y);
        }

        private void OnPointerLocated(Vector2 parametric)
        {
            float H = Mathf.Lerp(LeftPadding, 1 - RightPadding, parametric.x);
            float V = Mathf.Lerp(BottomPadding, 1 - TopPadding, parametric.y);

            PickColor(Color.HSVToRGB(H, CurrentSaturation, V));
        }
    }
}