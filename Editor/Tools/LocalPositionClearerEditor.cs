using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using WizardUtils.Extensions;

namespace WizardUtils.Tools
{
    [CustomEditor(typeof(LocalPositionClearer)), CanEditMultipleObjects]
    public class LocalPositionClearerEditor : Editor
    {
        LocalPositionClearer self;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Clear"))
            {
                using (new UndoScope("Clear Local Position"))
                {
                    foreach(var target in targets)
                    {
                        ClearLocalPosition(target as LocalPositionClearer);
                    }
                }

            }
        }

        private void ClearLocalPosition(LocalPositionClearer self)
        {
            Undo.RecordObject(self.transform, "");
            Transform[] children = self.transform.GetChildren();
            foreach (var child in children)
            {
                Undo.RecordObject(child, "");
                child.SetParent(null, true);
            }

            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;
            self.transform.localScale = Vector3.one;

            foreach (var child in children)
            {
                child.SetParent(self.transform, true);
                EditorUtility.SetDirty(child);
            }
            EditorUtility.SetDirty(self.transform);
        }
    }
}
