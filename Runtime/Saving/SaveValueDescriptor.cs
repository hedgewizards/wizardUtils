using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    [CreateAssetMenu(fileName = "sv_Value", menuName = "WizardUtils/Saving/SaveValueDescriptor", order = 1)]
    public class SaveValueDescriptor : ScriptableObject
    {
        public string Key;
        public string DefaultValue;
        public bool HideIfDefault = true;

        [TextArea(3,3)]
        public string DeveloperNote;

        public bool BooleanValue
        {
            get
            {
                return ConfigHelper.TryParseBool(SerializedValue);
            }
            set
            {
                SerializedValue = ConfigHelper.SerializeBool(value);
            }
        }

        public Color ColorValue
        {
            get
            {
                if (ConfigHelper.TryParseColor(SerializedValue, out Color color))
                {
                    return color;
                }
                Debug.LogWarning($"Invalid Color \'{SerializedValue}\' @ {Key}");
                if (ConfigHelper.TryParseColor(DefaultValue, out color))
                {
                    return color;
                }
                Debug.LogError($"Invalid Default Color {SerializedValue} @ {Key}");
                return new Color(1, 0, 1);
            }
            set
            {
                SerializedValue = ConfigHelper.SerializeColor(value);
            }
        }

        public int IntValue
        {
            get
            {
                if (ConfigHelper.TryParseInt(SerializedValue, out int value))
                {
                    return value;
                }
                Debug.LogWarning($"Invalid Integer {SerializedValue} @ {Key}");
                if(ConfigHelper.TryParseInt(DefaultValue, out value))
                {
                    return value;
                }
                Debug.LogError($"Invalid Default Integer {SerializedValue} @ {Key}");
                return 0;
            }
        }

        public string SerializedValue
        {
            get => GameManager.Instance?.ReadMainSave(this);
            set => GameManager.Instance?.WriteMainSave(this, value);
        }
    }
}
