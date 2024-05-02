using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class ColorHelper
    {
        public static bool TryParse(string value, out Color color)
        {
            if (ColorUtility.TryParseHtmlString(value, out color))
            {
                return true;
            }
            return false;
        }

        public static Color ParseOrDefault(string value, Color defaultValue = default)
        {
            if (!TryParse(value, out var color))
            {
                return defaultValue;
            }
            return color;
        }
    }
}
