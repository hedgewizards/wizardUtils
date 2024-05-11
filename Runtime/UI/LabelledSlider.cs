using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LabelledSlider : MonoBehaviour
{
    public Text MinLabel;
    public Text MaxLabel;
    public Text DisplayLabel;
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

    public void SetShowMinMaxLabels(bool showMinMaxLabels)
    {
        MinLabel.gameObject.SetActive(showMinMaxLabels);
        MaxLabel.gameObject.SetActive(showMinMaxLabels);
    }

    public void Initialize(float min, float max, float value, float stepSize = 0)
    {
        MinLabel.text = min.ToString(LabelDisplayFormat);

        MaxLabel.text = max.ToString(LabelDisplayFormat);

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
        if (MinLabel != null)
        {
            MinLabel.text = Slider.minValue.ToString(LabelDisplayFormat);
        }
        if (MaxLabel != null)
        {
            MaxLabel.text = Slider.maxValue.ToString(LabelDisplayFormat);
        }
        if (DisplayLabel != null)
        {
            DisplayLabel.text = Slider.value.ToString(LabelDisplayFormat);
        }
    }
}
