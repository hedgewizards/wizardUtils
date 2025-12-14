using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WizardUtils.InspectorAttributes;

[CustomPropertyDrawer(typeof(OrderedEnumAttribute))]
public class OrderedEnumDrawer : PropertyDrawer
{
    private static Dictionary<Type, List<DisplayableEnum>> _cachedEnumMenu = new Dictionary<Type, List<DisplayableEnum>>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Enum)
        {
            EditorGUI.LabelField(position, label.text, "Use [OrderedEnum] with enum.");
            return;
        }

        Type enumType = fieldInfo.FieldType;

        if (!_cachedEnumMenu.TryGetValue(enumType, out List<DisplayableEnum> menuEntries))
        {
            menuEntries = Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(e => new DisplayableEnum(e))
                .OrderBy(e => e.Path)
                .ToList();

            _cachedEnumMenu[enumType] = menuEntries;
        }

        Rect buttonRect = EditorGUI.PrefixLabel(position, label);
        string currentValue = property.enumNames[property.enumValueIndex];

        if (GUI.Button(buttonRect, currentValue, EditorStyles.popup))
        {
            var menu = new GenericMenu();
            foreach (var entry in menuEntries)
            {
                bool selected = entry.Value.ToString() == currentValue;
                menu.AddItem(new GUIContent(entry.Path), selected, () =>
                {
                    property.enumValueIndex = Array.IndexOf(property.enumNames, entry.Value.ToString());
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }
    }

    private struct DisplayableEnum
    {
        public Enum Value;
        public string Path;

        public DisplayableEnum(Enum value)
        {
            Value = value;
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttribute<EnumOrderAttribute>();
            Path = attr?.Path ?? ObjectNames.NicifyVariableName(value.ToString());
        }
    }
}
