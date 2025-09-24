using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

[CustomPropertyDrawer(typeof(WizardUtils.Tools.MultiEnum<>), true)]
public class MultiEnumDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type enumType = fieldInfo.FieldType.GetGenericArguments()[0];
        string[] enumNames = Enum.GetNames(enumType);

        SerializedProperty valuesProp = property.FindPropertyRelative("Values");
        string displayValue = GetDisplayValue(enumType, valuesProp);

        // Split rect for label + button
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        Rect buttonRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y,
                                   position.width - EditorGUIUtility.labelWidth, position.height);

        EditorGUI.LabelField(labelRect, label);

        if (EditorGUI.DropdownButton(buttonRect, new GUIContent(displayValue), FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();
            foreach (string enumName in enumNames)
            {
                int enumIntValue = Convert.ToInt32(Enum.Parse(enumType, enumName));
                bool isOn = Enumerable.Range(0, valuesProp.arraySize)
                    .Any(i => valuesProp.GetArrayElementAtIndex(i).intValue == enumIntValue);
                menu.AddItem(new GUIContent(enumName), isOn, () =>
                {
                    property.serializedObject.Update();
                    if (isOn)
                    {
                        for (int i = 0; i < valuesProp.arraySize; i++)
                        {
                            if (valuesProp.GetArrayElementAtIndex(i).intValue == enumIntValue)
                            {
                                valuesProp.DeleteArrayElementAtIndex(i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        valuesProp.InsertArrayElementAtIndex(valuesProp.arraySize);
                        valuesProp.GetArrayElementAtIndex(valuesProp.arraySize - 1).intValue = enumIntValue;
                    }
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }
    }

    private static string GetDisplayValue(Type enumType, SerializedProperty valuesProp)
    {
        if (valuesProp.arraySize > 0)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valuesProp.arraySize; i++)
            {
                string name = Enum.GetName(enumType, valuesProp.GetArrayElementAtIndex(i).intValue);
                sb.Append(name);
                if (i < valuesProp.arraySize - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }
        else
        {
            return "(None)";
        }
    }
}
