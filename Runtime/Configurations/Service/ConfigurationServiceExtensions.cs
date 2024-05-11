using System.Globalization;
using UnityEngine;

namespace WizardUtils.Configurations
{
    public static class ConfigurationServiceExtensions
    {
        public static ValueChangedEventArgs SpawnEventArgs(this IConfigurationService config, string key, string defaultValue = null)
        {
            return new ValueChangedEventArgs(key, defaultValue, config.Read(key, defaultValue));
        }

        public static void WriteColor(this IConfigurationService config, string key, Color color, bool writeToConfig = false)
        {
            config.Write(key, ConfigHelper.SerializeColor(color), writeToConfig);
        }

        public static Color ReadColor(this IConfigurationService config, string key, Color defaultValue)
        {
            if (ConfigHelper.TryParseColor(config.Read(key), out var color))
            {
                return color;
            }
            return defaultValue;
        }

        public static void WriteFloat(this IConfigurationService config, string key, float value, string format = "F", bool writeToConfig = false)
        {
            config.Write(key, value.ToString(format, CultureInfo.InvariantCulture), writeToConfig);
        }

        public static float ReadFloat(this IConfigurationService config, string key, float defaultValue)
        {
            if (float.TryParse(config.Read(key), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public static void WriteInt(this IConfigurationService config, string key, int value, bool writeToConfig = false)
        {
            config.Write(key, ConfigHelper.SerializeInt(value), writeToConfig);
        }

        public static int ReadInt(this IConfigurationService config, string key, int defaultValue)
        {
            if (ConfigHelper.TryParseInt(config.Read(key), out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public static void WriteBool(this IConfigurationService config, string key, bool value, bool writeToConfig = false)
        {
            config.Write(key, value ? "1" : "0");
        }

        public static bool ReadBool(this IConfigurationService config, string key, bool defaultValue)
        {
            string value = config.Read(key);
            if (value == "1") return true;
            if (value == "0") return false;
            return defaultValue;
        }
    }
}
