using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace WizardUtils.Inspector
{
    [CustomEditor(typeof(GameSettingTracker))]
    public class GameSettingTrackerEditor : Editor
    {
        GameSettingTracker self;
        SerializedProperty m_SettingKey;
        SerializedProperty m_SettingDescriptor;

        public override VisualElement CreateInspectorGUI()
        {
            self = target as GameSettingTracker;
            m_SettingKey = serializedObject.FindProperty(nameof(self.SettingKey));
            m_SettingDescriptor = serializedObject.FindProperty(nameof(self.GameSettingDescriptor));
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_SettingDescriptor, new GUIContent("Game Setting Descriptor"));
            if (self.GameSettingDescriptor == null)
            {
                EditorGUILayout.PropertyField(m_SettingKey, new GUIContent("Manual Setting Key"));
                EditorGUILayout.HelpBox(new GUIContent("Manual Setting Keys are more error-prone than using a Descriptor!"));
            }

            serializedObject.ApplyModifiedProperties();

            // Draw the GUI unity generates normally
            DrawDefaultInspector();
        }
    }
}
