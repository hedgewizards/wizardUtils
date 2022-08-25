using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
                return SaveValueHelper.ParseBool(SerializedValue);
            }
            set
            {
                SerializedValue = SaveValueHelper.SerializeBool(value);
            }
        }

        public Color ColorValue
        {
            get
            {
                if (SaveValueHelper.ParseColor(SerializedValue, out Color color))
                {
                    return color;
                }
                Debug.LogWarning($"Invalid Color {SerializedValue} @ {Key}");
                return new Color(1, 0, 1);
            }
            set
            {
                SerializedValue = SaveValueHelper.SerializeColor(value);
            }
        }

        public string SerializedValue
        {
            get => GameManager.GameInstance?.ReadMainSave(this);
            set => GameManager.GameInstance?.WriteMainSave(this, value);
        }
    }
}
