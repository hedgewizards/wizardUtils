using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;

namespace WizardUtils.Configurations.ConfigSettings
{
    [CustomEditor(typeof(SettingDescriptor), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class SettingDescriptorEditor : Editor
    {
        public DescriptorManifestAssigner<SettingManifest, SettingDescriptor> manifestAssigner;

        public override VisualElement CreateInspectorGUI()
        {
            manifestAssigner = new DescriptorManifestAssigner<SettingManifest, SettingDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            SettingDescriptor[] _targets = targets.Cast<SettingDescriptor>().ToArray();
            Validate(_targets);

            base.OnInspectorGUI();
            EditorGUILayout.Space();
            manifestAssigner.DrawRegisterButtons(_targets);
        }

        public void Validate(SettingDescriptor[] _targets)
        {
            if (!Validate(_targets, out string failReason))
            {
                if (_targets.Length == 1)
                {
                    EditorGUILayout.HelpBox($"Failed to Validate: {failReason}", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.HelpBox($"1 or more Failed to Validate: {failReason}", MessageType.Warning);
                }
            }
        }

        private bool Validate(SettingDescriptor[] _targets, out string result)
        {
            foreach(var target in _targets)
            {
                if (!target.Validate(out result))
                {
                    return false;
                }
            }
            result = default;
            return true;
        }
    }
}