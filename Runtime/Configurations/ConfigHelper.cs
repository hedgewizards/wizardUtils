﻿using System.Collections;
using System.Globalization;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace WizardUtils.Configurations
{
    public static class ConfigHelper
    {
        public static bool TryParseColor(string value, out Color color)
        {
            if (ColorUtility.TryParseHtmlString(value, out color))
            {
                return true;
            }
            return false;
        }

        public static string SerializeColor(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        public static bool TryParseBool(string value, out bool result)
        {
            if (value == "1")
            {
                result = true;
                return true;
            }
            else if (value == "0")
            {
                result = false;
                return true;
            }
            result = default;
            return false;
        }

        public static string SerializeBool(bool value)
        {
            return value ? "1" : "0";
        }

        public static bool TryParseInt(string value, out int result)
        {
            if (int.TryParse(value, out result))
            {
                return true;
            }
            return false;
        }

        public static string SerializeInt(int value)
        {
            return value.ToString();
        }

        public static bool TryParseFloat(string value, out float result)
        {
            if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            return false;
        }

        public static string SerializeFloat(float value, string format = "F")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}