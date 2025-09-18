using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(FlagBoolAttribute))]
    public class FlagBoolAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                EditorGUI.HelpBox(position,
                    $"{nameof(FlagBoolAttribute)} applied to non-boolean property {property}",
                    MessageType.Error);
                return;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                return base.GetPropertyHeight(property, label);
            }
            return 0;
        }
    }
}
