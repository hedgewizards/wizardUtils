using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.ManifestPattern
{
    [CustomEditor(typeof(ManifestedDescriptor), true)]
    public class ManifestedDescriptorEditor : Editor
    {
        protected virtual string RegisterButtonsHeaderText => "Manifests";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawRegisterButtons();
        }

        protected virtual void DrawRegisterButtons()
        {
            ICollection<ManifestedDescriptor> descriptors = targets.Cast<ManifestedDescriptor>().ToArray();
            string manifestTypeName = ((ManifestedDescriptor)target).GetManifestType().Name;
            GUILayout.Label(RegisterButtonsHeaderText, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            var manifests = AssetDatabase.FindAssets($"t:{manifestTypeName}")
                .Select(id => AssetDatabase.LoadAssetAtPath<DescriptorManifest<ManifestedDescriptor>>(AssetDatabase.GUIDToAssetPath(id)))
                .OrderBy(x => x.name);
            foreach (var manifest in manifests)
            {
                bool containsNone = true;
                bool containsAll = true;
                foreach (var target in descriptors)
                {
                    bool containsItem = manifest.Contains(target);
                    containsAll &= containsItem;
                    containsNone &= !containsItem;
                }

                bool toggle;
                using (new GUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.ObjectField(manifest, typeof(ManifestedDescriptor), false);
                    }

                    EditorGUI.showMixedValue = !(containsNone || containsAll);
                    toggle = EditorGUILayout.Toggle(containsAll);
                    EditorGUI.showMixedValue = false;
                }

                if (!toggle && containsAll)
                {
                    RemoveAll(manifest, descriptors);
                }
                else if (toggle && !containsAll)
                {
                    AddAll(manifest, descriptors);
                }
            }
            EditorGUI.indentLevel--;
        }

        private static void AddAll(DescriptorManifest<ManifestedDescriptor> manifest, ICollection<ManifestedDescriptor> items)
        {
            Undo.RecordObject(manifest, "Add Items to Manifest");
            foreach (var item in items)
            {
                if (!manifest.Contains(item))
                {
                    manifest.Add(item);
                }
            }
            EditorUtility.SetDirty(manifest);
            AssetDatabase.SaveAssetIfDirty(manifest);
        }

        private static void RemoveAll(DescriptorManifest<ManifestedDescriptor> manifest, ICollection<ManifestedDescriptor> items)
        {
            Undo.RecordObject(manifest, "Remove Items from Manifest");
            foreach (var item in items)
            {
                if (manifest.Contains(item))
                {
                    manifest.Remove(item);
                }
            }
            EditorUtility.SetDirty(manifest);
            AssetDatabase.SaveAssetIfDirty(manifest);
        }
    }
}
