using System;
using UnityEngine;
using UnityEngine.UI;

public class LabelledSlider : MonoBehaviour
{
    public Text MinLabel;
    public Text MaxLabel;
    public Text DisplayLabel;
    public string LabelDisplayFormat = "N0";

    public Slider Slider;


    private void OnValidate()
    {
        fixLabels();
    }

    private void Awake()
    {
        fixLabels();
        Slider.onValueChanged.AddListener(onDisplayValueChanged);
    }

    private void onDisplayValueChanged(float displayValue)
    {
        DisplayLabel.text = displayValue.ToString(LabelDisplayFormat);
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
