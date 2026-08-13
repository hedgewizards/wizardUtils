using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class ColorPickerController : MonoBehaviour
    {
        public enum Modes
        {
            HV_S,
            SV_H
        }

        public Modes Mode;
        [SerializeField]
        private Color currentColor = Color.white;
        public Color CurrentColor => currentColor;
        float CurrentHue => CurrentHSV.x;
        float CurrentSaturation => CurrentHSV.y;
        float CurrentValue => CurrentHSV.z;
        Vector3 CurrentHSV;

        public event Action<Color> OnColorChanged;
        public TextMeshProUGUI LabelText;
        public PointerHoldLocator XYLocator;
        public UnityEngine.UI.Slider Slider;
        public Image XYDot;
        public Image SliderDot;
        public Image XYField;
        public Image SliderField;
        public Image PreviewImage;
        ToggleableUIElement pickerMenu;

        [Range(0,1)]
        public float LeftPadding;
        [Range(0,1)]
        public float RightPadding;
        [Range(0, 1)]
        public float TopPadding;
        [Range(0, 1)]
        public float BottomPadding;

        int SliderConstantShaderProperty;
        int ShaderConstantValueXId;
        int ShaderConstantValueYId;

        private bool MaterialsInstanced;

        private void Awake()
        {
            UpdateStoredHSV();
            XYField.material = new Material(XYField.material);
            SliderField.material = new Material(SliderField.material);
            MaterialsInstanced = true;

            SliderConstantShaderProperty = Shader.PropertyToID("_ConstantComponentValue");
            ShaderConstantValueXId = Shader.PropertyToID("_ConstantX");
            ShaderConstantValueYId = Shader.PropertyToID("_ConstantY");
            XYLocator.OnDrag.AddListener(OnPointerLocated);
            pickerMenu = GetComponent<ToggleableUIElement>();
            Slider.onValueChanged.AddListener(OnSliderChanged);
            Close();
        }

        private void OnDestroy()
        {
            if (MaterialsInstanced)
            {
                Destroy(XYField.material);
                Destroy(SliderField.material);
            }
        }

        private void OnSliderChanged(float newSliderValue)
        {

            if (Mode == Modes.HV_S)
            {
                CurrentHSV = new Vector3(CurrentHue, newSliderValue, CurrentValue);
            }
            else if (Mode == Modes.SV_H)
            {
                CurrentHSV = new Vector3(newSliderValue, CurrentSaturation, CurrentValue);

            }
            UpdateStoredRGB();
            OnColorChanged?.Invoke(currentColor);
            UpdateVisuals();
        }

        public void SetLabel(string text)
        {
            LabelText.text = text;
        }

        public void Open()
        {
            pickerMenu.SetOpen(true);
        }

        public void Close()
        {
            pickerMenu.SetOpen(false);
        }

        bool isSaving;
        public void PickColor(Color color)
        {
            if (isSaving) return;
            isSaving = true;
            currentColor = color;
            UpdateStoredHSV();
            OnColorChanged?.Invoke(color);
            UpdateVisuals();
            isSaving = false;
        }

        public void PickColorSilent(Color color)
        {
            currentColor = color;
            UpdateStoredHSV();
            UpdateVisuals();
        }

        private void UpdateStoredHSV()
        {
            Color.RGBToHSV(currentColor, out float H, out float S, out float V);
            CurrentHSV = new Vector3(H, S, V);
        }

        private void UpdateStoredRGB()
        {
            currentColor = Color.HSVToRGB(CurrentHue, CurrentSaturation, CurrentValue);
        }

        private void OnValidate()
        {
            OnColorChanged?.Invoke(currentColor);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (PreviewImage != null)
            {
                PreviewImage.color = CurrentColor;
            }
            if (XYDot != null)
            {
                XYDot.color = CurrentValue > 0.5f ? Color.black : Color.white;

                var parent = XYDot.rectTransform.parent as RectTransform;
                var width = parent.rect.width;
                var height = parent.rect.height;

                Vector2 parametricPoint = CalculateColorSpaceXYDotPoint();

                XYDot.rectTransform.localPosition = new Vector3()
                {
                    x = parametricPoint.x * width,
                    y = parametricPoint.y * height,
                    z = XYDot.rectTransform.localPosition.z
                };
            }

            if (SliderDot != null)
            {
                SliderDot.color = CurrentValue > 0.5f ? Color.black : Color.white;
            }

            if (XYField != null)
            {
                if (Mode == Modes.HV_S)
                {
                    XYField.material.SetFloat(SliderConstantShaderProperty, CurrentSaturation);
                }
                else if (Mode == Modes.SV_H)
                {
                    XYField.material.SetFloat(SliderConstantShaderProperty, CurrentHue);
                }
            }

            if (SliderField != null)
            {
                if (Mode == Modes.HV_S)
                {
                    SliderField.material.SetFloat(ShaderConstantValueXId, CurrentHue);
                    SliderField.material.SetFloat(ShaderConstantValueYId, CurrentValue);
                }
                else if (Mode == Modes.SV_H)
                {
                    SliderField.material.SetFloat(ShaderConstantValueXId, 1);
                    SliderField.material.SetFloat(ShaderConstantValueYId, 1);
                }
            }

            if (Slider != null)
            {
                if (Mode == Modes.HV_S)
                {
                    Slider.value = CurrentSaturation;
                }
                else if (Mode == Modes.SV_H)
                {
                    Slider.value = CurrentHue;
                }
            }
        }

        private Vector2 CalculateColorSpaceXYDotPoint()
        {
            float rawX, rawY;

            if (Mode == Modes.HV_S)
            {
                rawX = CurrentHue;
                rawY = CurrentValue;
            }
            else if (Mode == Modes.SV_H)
            {
                rawX = CurrentSaturation;
                rawY = CurrentValue;
            }
            else
            {
                throw new NotImplementedException();
            }


            float x = Mathf.Lerp(LeftPadding, 1 - RightPadding, rawX);
            float y = Mathf.Lerp(BottomPadding, 1 - TopPadding, rawY);

            return new Vector2(x, y);
        }

        private void OnPointerLocated(Vector2 parametric)
        {
            float X = Mathf.InverseLerp(LeftPadding, 1 - RightPadding, parametric.x);
            float Y = Mathf.InverseLerp(BottomPadding, 1 - TopPadding, parametric.y);
            if (Mode == Modes.HV_S)
            {
                CurrentHSV = new Vector3(X, CurrentSaturation, Y);
            }
            else if (Mode == Modes.SV_H)
            {
                CurrentHSV = new Vector3(CurrentHue, X, Y);
            }

            UpdateStoredRGB();
            OnColorChanged?.Invoke(currentColor);
            UpdateVisuals();
        }
    }
}