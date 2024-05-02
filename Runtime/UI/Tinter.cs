using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.UI
{
    public class Tinter : MonoBehaviour
    {
        public UnityEvent<Color> OnColorChanged;

        public enum TintStyles
        {
            Multiply,
            Add
        }

        public TintStyles TintStyle;

        [SerializeField]
        private Color BaseColor;

        private Color Tint = Color.white;

        public void SetTint(Color tint)
        {
            Tint = tint;
            ColorChanged();
        }

        public void SetBaseColor(Color baseColor)
        {
            BaseColor = baseColor;
            ColorChanged();
        }

        private void ColorChanged()
        {
            Color color = TintStyle switch
            {
                TintStyles.Multiply => BaseColor * Tint,
                TintStyles.Add => BaseColor + Tint,
                _ => throw new KeyNotFoundException($"Invalid TintStyle {TintStyle}")
            };
            OnColorChanged?.Invoke(color);
        }
    }
}
