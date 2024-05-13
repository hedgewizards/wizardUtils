using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Configurations.Settings
{
    [CustomEditor(typeof(RangeFloatSetting))]
    [CanEditMultipleObjects]
    public class RangeFloatSettingEditor : SettingDescriptorEditor
    {
        private SerializedProperty m_SliderInterval;
        private SerializedProperty m_SliderDisplayFormat;
        public override VisualElement CreateInspectorGUI()
        {
            m_SliderInterval = serializedObject.FindProperty(nameof(RangeFloatSetting.SliderInterval));
            m_SliderDisplayFormat = serializedObject.FindProperty(nameof(RangeFloatSetting.SliderDisplayFormat));
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            RangeFloatSetting[] items = targets.Cast<RangeFloatSetting>().ToArray();
            Validate(items);
            DrawDefaultInspector();


            if (items.Any(x => x.UseSlider))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_SliderInterval);
                EditorGUILayout.PropertyField(m_SliderDisplayFormat);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            manifestAssigner.DrawRegisterButtons(items);

            serializedObject.ApplyModifiedProperties();
        }
    }
}