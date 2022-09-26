using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.UI
{
    public class ColorGroup : MonoBehaviour
    {
        public UnityEvent<Color> OnColorChanged;

        public Color color;
        Color storedColor;

        public bool ChangeColorOnAwake;

        private void Awake()
        {
            storedColor = color;
            if (ChangeColorOnAwake)
            {
                OnColorChanged?.Invoke(color);
            }
        }

        private void LateUpdate()
        {
            if (storedColor != color)
            {
                storedColor = color;
                OnColorChanged?.Invoke(color);
            }
        }
    }
}