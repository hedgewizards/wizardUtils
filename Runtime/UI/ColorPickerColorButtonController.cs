using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class ColorPickerColorButtonController : MonoBehaviour
    {
        [SerializeField]
        Color color;

        public Image PreviewImage;

        public ColorPickerController ColorPickerController;

        public void SetColorPicker()
        {
            ColorPickerController.PickColor(color);
        }

        private void OnValidate()
        {
            if (PreviewImage != null) PreviewImage.color = color;
        }
    }
}