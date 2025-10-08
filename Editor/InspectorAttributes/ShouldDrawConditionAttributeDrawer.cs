using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    /// <summary>
    /// Evaluates the named method (bool Method()). if true, draw this in the inspector. otherwise hide.
    /// </summary>
    [CustomPropertyDrawer(typeof(ShouldDrawConditionAttribute))]
    public class ShouldDrawConditionAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var condition = (ShouldDrawConditionAttribute)attribute;
            bool shouldDraw = CalculateShouldDraw(property, condition.ShouldHideMethodName);

            if (shouldDraw)
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var cond = (ShouldDrawConditionAttribute)attribute;
            bool shouldDraw = CalculateShouldDraw(property, cond.ShouldHideMethodName);

            return shouldDraw ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        private bool CalculateShouldDraw(SerializedProperty property, string methodName)
        {
            if (property.serializedObject.targetObject == null) return true;

            object target = GetDeclaringObject(property);
            if (target == null)
            {
                return true;
            }
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method == null)
            {
                Debug.LogError($"ShouldDrawConditionAttribute: Could not find method '{methodName}' on {target.GetType()}");
                return true;
            }

            if (method.ReturnType != typeof(bool))
            {
                Debug.LogError($"ShouldDrawConditionAttribute: Method '{methodName}' must return bool.");
                return true;
            }

            return (bool)method.Invoke(target, null);
        }
        private object GetDeclaringObject(SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string[] elements = property.propertyPath.Split('.');

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                var type = obj.GetType();

                if (element == "Array" && i + 1 < elements.Length && elements[i + 1].StartsWith("data["))
                {
                    // handle array or list index
                    int start = elements[i + 1].IndexOf('[') + 1;
                    int end = elements[i + 1].IndexOf(']');
                    int index = int.Parse(elements[i + 1].Substring(start, end - start));

                    if (obj is System.Collections.IList list && index < list.Count)
                        obj = list[index];
                    else
                        return null;

                    i++; // skip "data[i]"
                    continue;
                }

                var field = type.GetField(element, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (field == null)
                {
                    Debug.LogError($"ShouldDrawConditionAttribute: Failed to GetDeclaringObject step '{element}' on {type}");
                    return null;
                }

                obj = field.GetValue(obj);
            }

            return obj;
        }

    }
}
