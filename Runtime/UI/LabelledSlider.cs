using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class LabelledSlider : MonoBehaviour
    {
        public TextMeshProUGUI LabelText;
        public TextMeshProUGUI ValueLabel;
        public string LabelDisplayFormat = "N0";

        public Slider Slider;

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        public float StepSize { get; private set; }

        public event Action<float> OnValueChanged;

        private bool DontNotifyOnUpdate;

        private void OnValidate()
        {
            fixLabels();
        }

        private void Awake()
        {
            Slider.onValueChanged.AddListener(onDisplayValueChanged);
        }

        public void SetLabel(string text)
        {
            LabelText.text = text;
        }

        public void Initialize(float min, float max, float value, float stepSize = 0)
        {
            MinValue = min;
            MaxValue = max;
            StepSize = stepSize;

            if (StepSize <= 0)
            {
                Slider.wholeNumbers = false;
                Slider.minValue = MinValue;
                Slider.maxValue = MaxValue;
            }
            else
            {
                Slider.wholeNumbers = true;
                Slider.minValue = 0;
                Slider.maxValue = (MaxValue - MinValue) / StepSize;
            }
            SetValueSilent(value);
        }

        public void SetValue(float value)
        {
            float rawValue = CalculateRawValue(value);
            if (rawValue < Slider.minValue)
            {
                // so if the value is out of range we should still display the set value
                Slider.value = Slider.minValue;
                ValueLabel.text = value.ToString(LabelDisplayFormat);
            }
            else if (rawValue > Slider.maxValue)
            {
                Slider.value = Slider.maxValue;
                ValueLabel.text = value.ToString(LabelDisplayFormat);
            }
            else
            {
                Slider.value = rawValue;
            }
        }

        public void SetValueSilent(float value)
        {
            float rawValue = CalculateRawValue(value);
            if (rawValue < Slider.minValue)
            {
                // so if the value is out of range we should still display the set value
                Slider.SetValueWithoutNotify(Slider.minValue);
                ValueLabel.text = value.ToString(LabelDisplayFormat);
            }
            else if (rawValue > Slider.maxValue)
            {
                Slider.SetValueWithoutNotify(Slider.maxValue);
                ValueLabel.text = value.ToString(LabelDisplayFormat);
            }
            else
            {
                Slider.SetValueWithoutNotify(rawValue);
            }

            float realValue = CalculateRealValue(rawValue);
            ValueLabel.text = realValue.ToString(LabelDisplayFormat);
        }

        private float CalculateRealValue(float rawSliderValue)
        {
            if (!Slider.wholeNumbers) return rawSliderValue;

            float t = (float)rawSliderValue / (float)Slider.maxValue;
            return MinValue + t * (MaxValue - MinValue);
        }

        private float CalculateRawValue(float realSliderValue)
        {
            if (!Slider.wholeNumbers) return realSliderValue;

            return (int)((realSliderValue - MinValue) / StepSize);
        }

        private void onDisplayValueChanged(float rawSliderValue)
        {
            float realValue = CalculateRealValue(rawSliderValue);
            ValueLabel.text = realValue.ToString(LabelDisplayFormat);

            OnValueChanged?.Invoke(realValue);
        }

        private void fixLabels()
        {
            if (ValueLabel != null)
            {
                ValueLabel.text = Slider.value.ToString(LabelDisplayFormat);
            }
        }
    }
}