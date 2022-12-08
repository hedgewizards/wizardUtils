using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace WizardUtils.Animations
{
    [CustomEditor(typeof(SkinnedMeshRendererBoneViewer))]
    public class SkinnedMeshRendererBoneViewerEditor : Editor
    {
        SkinnedMeshRendererBoneViewer self;

        public override VisualElement CreateInspectorGUI()
        {
            self = target as SkinnedMeshRendererBoneViewer;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            // Draw the GUI unity generates normally
            DrawDefaultInspector();
            SkinnedMeshRenderer skinnedMeshRenderer = self.GetComponent<SkinnedMeshRenderer>();

            GUILayout.Label("Bones");
            EditorGUI.indentLevel++;
            for (int boneIndex = 0; boneIndex < self.GetComponent<SkinnedMeshRenderer>().bones.Length; boneIndex++)
            {
                Transform bone = skinnedMeshRenderer.bones[boneIndex];

                skinnedMeshRenderer.bones[boneIndex] = (Transform)EditorGUILayout.ObjectField(boneIndex.ToString(), bone, typeof(Transform), true);
            }
            EditorGUI.indentLevel--;
        }
    }
}