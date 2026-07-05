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

        Type fieldType = fieldInfo.FieldType;
        Type internalFieldType = fieldType;
        if (fieldType.IsArray)
        {
            internalFieldType = fieldType.GetElementType();
        }
        else
        {
            var enumerableType = fieldType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                internalFieldType = enumerableType.GetGenericArguments()[0];
            }
        }

        if (!_cachedEnumMenu.TryGetValue(internalFieldType, out List<DisplayableEnum> menuEntries))
        {
            menuEntries = Enum.GetValues(internalFieldType)
                .Cast<Enum>()
                .Select(e => new DisplayableEnum(e))
                .OrderBy(e => e.Path)
                .ToList();

            _cachedEnumMenu[internalFieldType] = menuEntries;
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
