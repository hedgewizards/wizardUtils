using System.Collections;
using UnityEngine;

namespace WizardUtils.Saving
{
    public static class SaveValueHelper
    {
        public static bool ParseColor(string value, out Color color)
        {
            if (ColorUtility.TryParseHtmlString(value, out color))
            {
                return true;
            }
            return false;
        }

        public static string SerializeColor(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        public static bool ParseBool(string value)
        {
            return value == "1";
        }

        public static string SerializeBool(bool value)
        {
            return value ? "1" : "0";
        }
    }
}