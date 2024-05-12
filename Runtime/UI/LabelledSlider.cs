using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LabelledSlider : MonoBehaviour
{
    public TextMeshProUGUI DisplayLabel;
    public string LabelDisplayFormat = "N0";

    public Slider Slider;

    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }
    public float StepSize { get; private set; }

    public UnityEvent<float> OnValueChanged;

    private void OnValidate()
    {
        fixLabels();
    }

    private void Awake()
    {
        Slider.onValueChanged.AddListener(onDisplayValueChanged);
    }

    public void Initialize(float min, float max, float value, float stepSize = 0)
    {
        StepSize = stepSize;
        if (StepSize <= 0)
        {
            Slider.wholeNumbers = false;
            Slider.minValue = min;
            Slider.maxValue = max;
        }
        else
        {
            Slider.wholeNumbers = true;
            Slider.minValue = 0;
            Slider.maxValue = (MaxValue - MinValue) / StepSize;
        }
        Slider.value = CalculateRawValue(value);
    }

    public void SetValue(float value)
    {
        Slider.value = CalculateRawValue(value);
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

        OnValueChanged?.Invoke(realValue);
        DisplayLabel.text = realValue.ToString(LabelDisplayFormat);
    }

    private void fixLabels()
    {
        if (DisplayLabel != null)
        {
            DisplayLabel.text = Slider.value.ToString(LabelDisplayFormat);
        }
    }
}
