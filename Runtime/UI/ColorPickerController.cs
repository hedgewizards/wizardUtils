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
        float CurrentHue => CurrentHSV.x;
        float CurrentSaturation => CurrentHSV.y;
        float CurrentValue => CurrentHSV.z;
        Vector3 CurrentHSV;

        public UnityEvent<Color> OnColorChanged;
        public PointerHoldLocator PointerLocator;
        public Image HueValDot;
        public Image SatDot;
        public Image HueValField;
        public Image SatField;
        public Slider SatSlider;
        ToggleableUIElement pickerMenu;

        [Range(0,1)]
        public float LeftPadding;
        [Range(0,1)]
        public float RightPadding;
        [Range(0, 1)]
        public float TopPadding;
        [Range(0, 1)]
        public float BottomPadding;

        int ShaderSaturationId;
        int ShaderHueId;
        int ShaderValueId;

        private void Awake()
        {
            UpdateStoredHSV();
            ShaderSaturationId = Shader.PropertyToID("_ConstantComponentValue");
            ShaderHueId = Shader.PropertyToID("_Hue");
            ShaderValueId = Shader.PropertyToID("_Value");
            PointerLocator.OnDrag.AddListener(OnPointerLocated);
            pickerMenu = GetComponent<ToggleableUIElement>();
            SatSlider.onValueChanged.AddListener(OnSatSliderChanged);
        }

        private void OnSatSliderChanged(float newSaturation)
        {
            CurrentHSV = new Vector3(CurrentHue, newSaturation, CurrentValue);
            UpdateStoredRGB();
            OnColorChanged?.Invoke(CurrentColor);
            UpdateVisuals();
        }

        public void Open()
        {
            pickerMenu.SetOpen(true);
        }

        public void Close()
        {
            pickerMenu.SetOpen(false);
        }

        public void PickColor(Color color)
        {
            CurrentColor = color;
            UpdateStoredHSV();
            OnColorChanged?.Invoke(color);
            UpdateVisuals();
        }

        private void UpdateStoredHSV()
        {
            Color.RGBToHSV(CurrentColor, out float H, out float S, out float V);
            CurrentHSV = new Vector3(H, S, V);
        }

        private void UpdateStoredRGB()
        {
            CurrentColor = Color.HSVToRGB(CurrentHue, CurrentSaturation, CurrentValue);
        }

        private void OnValidate()
        {
            OnColorChanged?.Invoke(CurrentColor);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (HueValDot != null)
            {
                HueValDot.color = CurrentValue > 0.5f ? Color.black : Color.white;

                var parent = HueValDot.rectTransform.parent as RectTransform;
                var width = parent.rect.width;
                var height = parent.rect.height;

                Vector2 parametricPoint = CalculateColorSpaceHueValDotPoint();

                HueValDot.rectTransform.localPosition = new Vector3()
                {
                    x = parametricPoint.x * width,
                    y = parametricPoint.y * height,
                    z = HueValDot.rectTransform.localPosition.z
                };
            }

            if (SatDot != null)
            {
                SatDot.color = CurrentValue > 0.5f ? Color.black : Color.white;
            }

            if (HueValField != null)
            {
                HueValField.material.SetFloat(ShaderSaturationId, CurrentSaturation);
            }

            if (SatField != null)
            {
                SatField.material.SetFloat(ShaderHueId, CurrentHue);
                SatField.material.SetFloat(ShaderValueId, CurrentValue);
            }

            if (SatSlider != null)
            {
                SatSlider.value = CurrentSaturation;
            }
        }

        private Vector2 CalculateColorSpaceHueValDotPoint()
        {
            float x = Mathf.Lerp(LeftPadding, 1 - RightPadding, CurrentHue);
            float y = Mathf.Lerp(BottomPadding, 1 - TopPadding, CurrentValue);

            return new Vector2(x, y);
        }

        private void OnPointerLocated(Vector2 parametric)
        {
            float H = Mathf.InverseLerp(LeftPadding, 1 - RightPadding, parametric.x);
            float V = Mathf.InverseLerp(BottomPadding, 1 - TopPadding, parametric.y);
            CurrentHSV = new Vector3(H, CurrentSaturation, V);
            UpdateStoredRGB();
            OnColorChanged?.Invoke(CurrentColor);
            UpdateVisuals();

            Debug.Log(parametric.x + " " + H);
        }
    }
}