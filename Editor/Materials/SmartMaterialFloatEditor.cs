using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.Materials
{
    [CustomEditor(typeof(SmartMaterialFloat))]
    public class SmartMaterialFloatEditor : Editor
    {
        SmartMaterialFloat self;

        public override VisualElement CreateInspectorGUI()
        {
            self = target as SmartMaterialFloat;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            // Draw the GUI unity generates normally
            DrawDefaultInspector();
            string newValue = EditorGUILayout.TextField("Parameter", self.Parameter);
            if (newValue != self.Parameter)
            {
                self.Parameter = newValue;
                self.UpdateParameterId();
            }
        }
    }
}