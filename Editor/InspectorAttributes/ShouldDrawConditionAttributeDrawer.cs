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

            return shouldDraw ? 0 : EditorGUI.GetPropertyHeight(property, label, true);
        }

        private bool CalculateShouldDraw(SerializedProperty property, string methodName)
        {
            if (property.serializedObject.targetObject == null) return true;

            object target = property.serializedObject.targetObject;
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
    }
}
